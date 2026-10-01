using System;

namespace BuildingLightingSystem
{
    public class MotionSensor
    {
        // Стан: true - рух виявлено, false - руху немає 
        public bool IsMotionDetected { get; set; }

        public MotionSensor(bool initialState)
        {
            IsMotionDetected = initialState;
        }

        public bool CheckMotion()
        {
            return IsMotionDetected;
        }
    }
}