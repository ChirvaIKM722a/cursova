using System;

namespace BuildingLightingSystem
{
    public class LightSensor
    {
        // Поточний рівень природного освітлення (в Люксах) 
        public double CurrentLightLevel { get; set; }

        public LightSensor(double initialLevel)
        {
            CurrentLightLevel = initialLevel;
        }

        public double GetLightLevel()
        {
            return CurrentLightLevel;
        }
    }
}