using UnityEngine;
using System.Collections.Generic;
using System.Text;

public class CollisionDataTracker : MonoBehaviour
{
    [Header("Data Collection")]
    public CrashVehicle vehicle1;
    public CrashVehicle vehicle2;

    [Header("Export Settings")]
    public string exportFileName = "crash_data";
    public KeyCode exportKey = KeyCode.E;

    private List<CrashDataPoint> dataPoints = new List<CrashDataPoint>();
    private bool isTracking = false;
    private float trackingStartTime;

    [System.Serializable]
    public class CrashDataPoint
    {
        public float timestamp;

        //Vehicle 1 data
        public string v1_name;
        public Vector3 v1_position_feet;
        public Vector3 v1_velocity_mph;
        public float v1_speed_mph;

        //Vehicle 2 data
        public string v2_name;
        public Vector3 v2_position_feet;
        public Vector3 v2_velocity_mph;
        public float v2_speed_mph;

        //Collision data
        public bool collisionOccurred;
        public Vector3 impactPoint_feet;
        public float impactForce_lbf;
    }

    void Start()
    {
        if (vehicle1 == null || vehicle2 == null)
        {
            Debug.LogError("Data Tracker: Assign both vehicles!");
            return;
        }

        StartTracking();
    }

    void StartTracking()
    {
        isTracking = true;
        trackingStartTime = Time.time;
        Debug.Log("Data Tracker: Started tracking crash data");
    }

    void FixedUpdate()
    {
        if (!isTracking) 
            return;

        RecordDataPoint();
    }

    void RecordDataPoint()
    {
        CrashDataPoint point = new CrashDataPoint
        {
            timestamp = Time.time - trackingStartTime,

            //Vehicle 1
            v1_name = vehicle1.name,
            v1_position_feet = UnitConverter.UnityToFeet(vehicle1.transform.position),
            v1_velocity_mph = vehicle1.GetVelocityMph(),
            v1_speed_mph = vehicle1.GetCurrentSpeedMph(),

            //Vehicle 2
            v2_name = vehicle2.name,
            v2_position_feet = UnitConverter.UnityToFeet(vehicle2.transform.position),
            v2_velocity_mph = vehicle2.GetVelocityMph(),
            v2_speed_mph = vehicle2.GetCurrentSpeedMph(),

            //Collision status
            collisionOccurred = vehicle1.HasCollided() || vehicle2.HasCollided()
        };

        dataPoints.Add(point);
    }

    void Update()
    {
        if (Input.GetKeyDown(exportKey))
        {
            ExportToCSV();
        }
    }

    public void ExportToCSV()
    {
        if (dataPoints.Count == 0)
        {
            Debug.LogWarning("Data Tracker: No data to export");
            return;
        }

        StringBuilder csv = new StringBuilder();

        //Data rows
        foreach (var point in dataPoints)
        {
            csv.AppendLine($"Timestape(s): {point.timestamp:F3}\n" +
                          $"Vehicle Name: {point.v1_name}, "+
                          $"Position 1_X(ft): {point.v1_position_feet.x:F2}, Position 1_Y(ft): {point.v1_position_feet.y:F2}, Position 1_Z(ft): {point.v1_position_feet.z:F2}, " +
                          $"Vel 1_X(mph): {point.v1_velocity_mph.x:F2}, Vel 1_Y(mph): {point.v1_velocity_mph.y:F2}, Vel 1_Z(mph): {point.v1_velocity_mph.z:F2}, " +
                          $"Vel 1_Speed(mph): {point.v1_speed_mph:F2}\n"+                          
                          $"Vehicle Name: {point.v2_name}, " +
                          $"Position 2_X(ft): {point.v2_position_feet.x:F2}, Position 2_Y(ft): {point.v2_position_feet.y:F2}, Position 2_Z(ft): {point.v2_position_feet.z:F2}, " +
                          $"Vel 2_X(mph): {point.v2_velocity_mph.x:F2}, Vel 2_Y(mph): {point.v2_velocity_mph.y:F2}, Vel 2_Z(mph): {point.v2_velocity_mph.z:F2}, " +
                          $"Vel 2_Speed(mph): {point.v2_speed_mph:F2}\n" +
                          $"Collision: {(point.collisionOccurred ? "YES" : "NO")}\n");
        }

        //Save to file
        string path = $"{Application.dataPath}/{exportFileName}.csv";
        System.IO.File.WriteAllText(path, csv.ToString());

        Debug.Log($"Data Tracker: Exported {dataPoints.Count} data points to:\n{path}");
    }
}
