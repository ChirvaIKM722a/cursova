using System;

namespace BuildingLightingSystem
{
    public class Dimmer
    {
        public int BrightnessLevel { get; private set; }

        public void SetBrightness(int level)
        {
            if (level < 0) level = 0;
            if (level > 100) level = 100;

            BrightnessLevel = level;
            Console.WriteLine($"Димер: Встановлено рівень яскравості { BrightnessLevel}%."); 
        }
    }
}