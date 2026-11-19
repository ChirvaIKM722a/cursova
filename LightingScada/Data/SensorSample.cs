namespace LightingScada.Data;

/// <summary>Один запис історії показань датчика (рядок таблиці SensorSamples).</summary>
public class SensorSample
{
    public int Id { get; set; }
    public string Topic { get; set; } = "";
    public double Value { get; set; }
    public string Unit { get; set; } = "";
    public DateTimeOffset MeasuredAtUtc { get; set; }
}
