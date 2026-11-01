namespace LightingScada.Models;

public class MqttSettings
{
    public string Host { get; set; } = "127.0.0.1";
    public int Port { get; set; } = 1883;
    public string TopicFilter { get; set; } = "lighting/1/sensors/#";
    public string ClientId { get; set; } = "lighting-scada-web";
}
