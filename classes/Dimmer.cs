namespace BuildingLightingSystem;

public class Dimmer
{
    public int BrightnessLevel { get; private set; }

    public void SetBrightness(int level)
    {
        level = Math.Clamp(level, 0, 100);
        BrightnessLevel = level;
        Console.WriteLine($"Димер: Встановлено рівень яскравості {BrightnessLevel}%.");
    }
}
