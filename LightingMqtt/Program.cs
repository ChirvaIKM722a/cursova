using System.Text.Json;
using BuildingLightingSystem.Control;
using BuildingLightingSystem.Sensors;
using MQTTnet;

Console.OutputEncoding = System.Text.Encoding.UTF8;

const string BrokerHost = "127.0.0.1";
const int BrokerPort = 1883;
const string BaseTopic = "lighting/1/sensors/room1";


var lightSensor = new LightSensor(250);
var motionSensor = new MotionSensor(true);
var controlSystem = new LightingControlSystem(lightSensor, motionSensor);


var sensors = new List<SensorConfiguration>
{
    new(lightSensor,  $"{BaseTopic}/light",  "lx"),
    new(motionSensor, $"{BaseTopic}/motion", "bool")
};


var factory = new MqttClientFactory();
using var mqttClient = factory.CreateMqttClient();

var options = new MqttClientOptionsBuilder()
    .WithTcpServer(BrokerHost, BrokerPort)
    .WithClientId("lighting-simulator-1")
    .Build();

await mqttClient.ConnectAsync(options);
Console.WriteLine($"Підключено до MQTT-брокера {BrokerHost}:{BrokerPort}");

for (int iteration = 1; iteration <= 10; iteration++)
{
    Console.WriteLine($"--- Цикл {iteration} ---");

    foreach (var (sensor, topic, unit) in sensors)
    {
        sensor.ReadValue();

        string payload = JsonSerializer.Serialize(new
        {
            value = Math.Round(sensor.Value, 2),
            unit = unit,
            measuredAtUtc = DateTimeOffset.UtcNow
        });

        var message = new MqttApplicationMessageBuilder()
            .WithTopic(topic)
            .WithPayload(payload)
            .Build();

        await mqttClient.PublishAsync(message);
        Console.WriteLine($"[PUB] {topic} {payload}");
    }


    controlSystem.UpdateSystemState();

    await Task.Delay(TimeSpan.FromSeconds(2));
}

await mqttClient.DisconnectAsync();
Console.WriteLine("Симуляцію завершено.");

record SensorConfiguration(Sensor Sensor, string Topic, string Unit);
