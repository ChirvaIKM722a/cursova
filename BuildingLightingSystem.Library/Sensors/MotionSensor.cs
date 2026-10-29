namespace BuildingLightingSystem.Sensors;

public class MotionSensor : Sensor
{
    // true - рух виявлено, false - руху немає
    public bool IsMotionDetected { get; set; }

    public MotionSensor(bool initialState) : base("motion", "bool")
    {
        IsMotionDetected = initialState;
        Value = initialState ? 1 : 0;
    }

    public bool CheckMotion() => IsMotionDetected;

    // Імітація PIR-датчика: з імовірністю 30% стан змінюється
    public override double ReadValue()
    {
        if (Rng.NextDouble() < 0.3)
            IsMotionDetected = !IsMotionDetected;
        Value = IsMotionDetected ? 1 : 0;
        return Value;
    }
}
