using UnityEngine;
using TMPro;

public class VehicleTooltipUI : MonoBehaviour
{
    public GameObject panel;
    public TextMeshProUGUI text;

    public void Show(CrashVehicle vehicle)
    {
        float speed = vehicle.GetCurrentSpeedMph();
        float preCrashSpeed = vehicle.GetPreCollisionVelocity().magnitude;
        float preCrashSpeedMph = UnitConverter.UnityVelocityToMph(preCrashSpeed);

        text.text =
            $"Name: {vehicle.vehicleName}\n" +
            $"Weight: {vehicle.weightPounds} lbs\n" +
            $"Pre-Crash Speed: {preCrashSpeedMph:F1} MPH";

        panel.SetActive(true);
    }

    public void Hide()
    {
        panel.SetActive(false);
    }
}
