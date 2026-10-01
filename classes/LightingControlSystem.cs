using BuildingLightingSystem;
using System;

public class LightingControlSystem
{

    private LightSensor _lightSensor = new LightSensor();
    private MotionSensor _motionSensor = new MotionSensor();
    private Relay _relay = new Relay();
    private Dimmer _dimmer = new Dimmer();


    private const double LightThreshold = 300.0;


    public void UpdateSystemState()
    {
        double currentLight = _lightSensor.GetLightLevel();
        bool isMotion = _motionSensor.CheckMotion();


        if (isMotion && currentLight < LightThreshold)
        {
            if (!_relay.IsOn)
            {
                _relay.TurnOn();
            }

            int targetBrightness = (int)((1 - (currentLight /
LightThreshold)) * 100);
            _dimmer.SetBrightness(targetBrightness);
        }
        else
        {

            if (_relay.IsOn)
            {
                _relay.TurnOff();
            }
            _dimmer.SetBrightness(0);
        }
    }
}