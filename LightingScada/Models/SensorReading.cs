namespace LightingScada.Models;

public record SensorReading(
    string Topic,
    double Value,
    string Unit,
    DateTimeOffset MeasuredAtUtc,
    DateTimeOffset ReceivedAtUtc);
