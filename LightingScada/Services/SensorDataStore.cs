using System.Collections.Concurrent;
using LightingScada.Models;

namespace LightingScada.Services;

/// <summary>Потокобезпечне сховище поточних показань у пам'яті (за MQTT-темою).</summary>
public class SensorDataStore
{
    private readonly ConcurrentDictionary<string, SensorReading> _readings = new();

    public volatile bool IsConnected;

    public void Update(SensorReading reading) => _readings[reading.Topic] = reading;

    public IReadOnlyCollection<SensorReading> GetAll() =>
        _readings.Values.OrderBy(r => r.Topic).ToList();
}
