namespace BuildingLightingSystem;

public class LightSensor : Sensor
{
    // Поточний рівень природного освітлення
    public double CurrentLightLevel { get; set; }

    public LightSensor(double initialLevel) : base("light", "lx")
    {
        CurrentLightLevel = initialLevel;
        Value = initialLevel;
    }

    public double GetLightLevel() => CurrentLightLevel;

    // Імітація зміни природного освітлення
    public override double ReadValue()
    {
        CurrentLightLevel += Rng.NextDouble() * 120 - 60;
        CurrentLightLevel = Math.Clamp(CurrentLightLevel, 0, 1000);
        Value = CurrentLightLevel;
        return Value;
    }
}
