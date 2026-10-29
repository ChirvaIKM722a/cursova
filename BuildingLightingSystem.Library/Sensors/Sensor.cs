namespace BuildingLightingSystem.Sensors;

/// <summary>Базовий клас сенсора: назва, одиниця виміру, останнє показання.</summary>
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

    /// <summary>Зчитує нове показання, зберігає його у Value і повертає.</summary>
    public abstract double ReadValue();
}
