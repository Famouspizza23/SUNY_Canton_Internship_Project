using UnityEngine;

public class SkidMarkGenerator : MonoBehaviour
{
    [Header("Wheel Settings")]
    public Transform[] wheelPositions = new Transform[4];
    public float wheelRadius = 0.35f;

    [Header("Skid Mark Appearance")]
    public Material skidMarkMaterial;
    public float markWidth = 0.2f;
    public Color markColor = Color.black;

    [Header("Physics Thresholds")]
    public float slipThreshold = 0.5f;
    public float brakeThreshold = 2f;

    private CrashVehicle vehicle;
    private Rigidbody rb;
    private TrailRenderer[] wheelTrails;
    private Vector3 lastVelocity;

    void Start()
    {
        vehicle = GetComponent<CrashVehicle>();
        rb = GetComponent<Rigidbody>();

        SetupWheelTrails();
        lastVelocity = rb.linearVelocity;
    }

    void SetupWheelTrails()
    {
        wheelTrails = new TrailRenderer[wheelPositions.Length];

        for (int i = 0; i < wheelPositions.Length; i++)
        {
            if (wheelPositions[i] == null)
            {
                Debug.LogWarning($"Skid Marks: Wheel {i} position not assigned");
                continue;
            }

            //Create trail renderer for this wheel
            GameObject trailObj = new GameObject($"SkidMark_Wheel{i}");
            trailObj.transform.SetParent(wheelPositions[i]);
            trailObj.transform.localPosition = Vector3.zero;

            TrailRenderer trail = trailObj.AddComponent<TrailRenderer>();
            wheelTrails[i] = trail;

            //Configure trail appearance
            trail.time = 300f;
            trail.startWidth = markWidth;
            trail.endWidth = markWidth;
            trail.material = skidMarkMaterial;
            trail.startColor = markColor;
            trail.endColor = new Color(markColor.r, markColor.g, markColor.b, 0.5f);
            trail.emitting = false;
            trail.minVertexDistance = 0.1f;

            trail.transform.localPosition = new Vector3(0, -wheelRadius, 0);
        }

        Debug.Log($"Skid Marks: Setup {wheelTrails.Length} wheel trails");
    }

    void FixedUpdate()
    {
        if (rb == null || wheelTrails == null) return;

        bool isSkidding = DetectSkidding();

        //Enable/disable trails based on skidding
        foreach (TrailRenderer trail in wheelTrails)
        {
            if (trail != null)
            {
                trail.emitting = isSkidding;
            }
        }

        lastVelocity = rb.linearVelocity;
    }

    bool DetectSkidding()
    {
        //Check for sideways slip
        Vector3 localVelocity = transform.InverseTransformDirection(rb.linearVelocity);
        float sidewaysVel = Mathf.Abs(localVelocity.x);

        //Check for hard braking
        Vector3 acceleration = (rb.linearVelocity - lastVelocity) / Time.fixedDeltaTime;
        float forwardDecel = -Vector3.Dot(acceleration, transform.forward);

        //Check if vehicle has collided
        bool hasCollided = vehicle != null && vehicle.HasCollided();

        //Skid conditions
        bool isSlidingSideways = sidewaysVel > slipThreshold;
        bool isBrakingHard = forwardDecel > brakeThreshold;
        bool isMovingFast = rb.linearVelocity.magnitude > UnitConverter.MphToUnityVelocity(5f);

        return (isSlidingSideways || isBrakingHard || hasCollided) && isMovingFast;
    }

    public void EnableSkidMarks(bool enable)
    {
        foreach (TrailRenderer trail in wheelTrails)
        {
            if (trail != null)
            {
                trail.emitting = enable;
            }
        }
    }

    public void ClearSkidMarks()
    {
        foreach (TrailRenderer trail in wheelTrails)
        {
            if (trail != null)
            {
                trail.Clear();
            }
        }
    }
}
