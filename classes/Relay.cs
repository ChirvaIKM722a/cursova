using System;

namespace BuildingLightingSystem
{
    public class Relay
    {
        public bool IsOn { get; private set; }

        public void TurnOn()
        {
            IsOn = true;
            Console.WriteLine("Реле: Живлення світильників УВІМКНЕНО."); 
        }

        public void TurnOff()
        {
            IsOn = false;
            Console.WriteLine("Реле: Живлення світильників BИМКНЕНО."); 
        }
    }
}