using UnityEngine;

public class TapeMeasureTool : MonoBehaviour
{
    public static bool isActive = false;
    public void ToggleTape()
    {
        isActive = !isActive;
    }
}
