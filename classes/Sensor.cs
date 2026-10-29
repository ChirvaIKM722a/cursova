namespace BuildingLightingSystem;

public abstract class Sensor
{
    protected static readonly Random Rng = new();

    public string Name { get; }
    public string Unit { get; }
    public double Value { get; protected set; }

    protected Sensor(string name, string unit)
    {
        Name = name;
        Unit = unit;
    }

    public abstract double ReadValue();
}
