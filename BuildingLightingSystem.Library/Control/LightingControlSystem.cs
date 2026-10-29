using BuildingLightingSystem.Actuators;
using BuildingLightingSystem.Sensors;

namespace BuildingLightingSystem.Control;

public class LightingControlSystem
{
    private const double LightThreshold = 300.0;

    private readonly LightSensor _lightSensor;
    private readonly MotionSensor _motionSensor;
    private readonly Relay _relay = new();
    private readonly Dimmer _dimmer = new();

    public LightingControlSystem(LightSensor lightSensor, MotionSensor motionSensor)
    {
        _lightSensor = lightSensor;
        _motionSensor = motionSensor;
    }

    public void UpdateSystemState()
    {
        double currentLight = _lightSensor.GetLightLevel();
        bool isMotion = _motionSensor.CheckMotion();

        if (isMotion && currentLight < LightThreshold)
        {
            if (!_relay.IsOn) _relay.TurnOn();
            int targetBrightness = (int)((1 - currentLight / LightThreshold) * 100);
            _dimmer.SetBrightness(targetBrightness);
        }
        else
        {
            if (_relay.IsOn) _relay.TurnOff();
            _dimmer.SetBrightness(0);
        }
    }
}
