using System.Text;
using System.Text.Json;
using LightingScada.Models;
using Microsoft.Extensions.Options;
using MQTTnet;

namespace LightingScada.Services;

public class MqttSubscriberService : BackgroundService
{
    private readonly MqttSettings _settings;
    private readonly SensorDataStore _store;
    private readonly ILogger<MqttSubscriberService> _logger;

    public MqttSubscriberService(
        IOptions<MqttSettings> settings,
        SensorDataStore store,
        ILogger<MqttSubscriberService> logger)
    {
        _settings = settings.Value;
        _store = store;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var factory = new MqttClientFactory();
        using var client = factory.CreateMqttClient();

        client.ApplicationMessageReceivedAsync += e =>
        {
            try
            {
                string topic = e.ApplicationMessage.Topic;
                string payload = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);

                using var doc = JsonDocument.Parse(payload);
                var root = doc.RootElement;

                var reading = new SensorReading(
                    topic,
                    root.GetProperty("value").GetDouble(),
                    root.GetProperty("unit").GetString() ?? "",
                    root.GetProperty("measuredAtUtc").GetDateTimeOffset(),
                    DateTimeOffset.UtcNow);

                _store.Update(reading);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Не вдалося обробити MQTT-повідомлення");
            }
            return Task.CompletedTask;
        };

        var options = new MqttClientOptionsBuilder()
            .WithTcpServer(_settings.Host, _settings.Port)
            .WithClientId(_settings.ClientId)
            .Build();

        var subscribeOptions = new MqttClientSubscribeOptionsBuilder()
            .WithTopicFilter(f => f.WithTopic(_settings.TopicFilter))
            .Build();

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                if (!client.IsConnected)
                {
                    await client.ConnectAsync(options, stoppingToken);
                    await client.SubscribeAsync(subscribeOptions, stoppingToken);
                    _logger.LogInformation("MQTT: підключено, підписка на {Filter}", _settings.TopicFilter);
                }
                _store.IsConnected = client.IsConnected;
            }
            catch (OperationCanceledException)
            {
                break;
            }
            catch (Exception ex)
            {
                _store.IsConnected = false;
                _logger.LogWarning("MQTT: помилка підключення: {Message}", ex.Message);
            }

            try { await Task.Delay(TimeSpan.FromSeconds(2), stoppingToken); }
            catch (OperationCanceledException) { break; }
        }

        _store.IsConnected = false;
    }
}
