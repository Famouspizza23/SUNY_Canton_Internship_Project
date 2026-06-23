using UnityEngine;
using TMPro;

public class TapeMeasurer : MonoBehaviour
{
    public LineRenderer line;
    public TextMeshProUGUI distanceText;

    private Vector3 startPoint;
    private bool measuring = false;

    void Update()
    {
        if (!TapeMeasureTool.isActive)
        {
            line.enabled = false;
            distanceText.gameObject.SetActive(false);
            return;
        }
        else
        {
            distanceText.gameObject.SetActive(true);

        }

        if (Input.GetMouseButtonDown(0))
        {
            startPoint = GetMouseWorldPosition();
            measuring = true;

            line.enabled = true;
            line.positionCount = 2;
        }

        if (Input.GetMouseButton(0) && measuring)
        {
            Vector3 currentPoint = GetMouseWorldPosition();

            line.SetPosition(0, startPoint);
            line.SetPosition(1, currentPoint);

            float distance = Vector3.Distance(startPoint, currentPoint);

            //Convert Unity units -> inches (example scale)
            float inches = distance * 39.37f;

            int feet = Mathf.FloorToInt(inches / 12f);
            float remainingInches = inches % 12f;
            distanceText.text = feet + " ft " + remainingInches.ToString("F1") + " in";
        }

        if (Input.GetMouseButtonUp(0))
        {
            measuring = false;
        }
    }

    Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 10f;
        return Camera.main.ScreenToWorldPoint(mousePos);
    }
}
