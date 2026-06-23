using UnityEngine;

public class VehicleHoverDetector : MonoBehaviour
{
    public VehicleTooltipUI tooltip;
    private CrashVehicle currentVehicle;

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            CrashVehicle vehicle = hit.collider.GetComponent<CrashVehicle>();

            if (vehicle != null)
            {
                if (currentVehicle != vehicle)
                {
                    currentVehicle = vehicle;
                    tooltip.Show(vehicle);
                }
                return;
            }
        }

        if (currentVehicle != null)
        {
            currentVehicle = null;
            tooltip.Hide();
        }
    }
}
