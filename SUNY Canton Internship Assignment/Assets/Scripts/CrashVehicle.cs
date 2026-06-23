using UnityEngine;

[RequireComponent(typeof(Rigidbody))] //Needs Rigidbody for calculations
public class CrashVehicle : MonoBehaviour
{
    [Header("Vehicle Identity")]
    public string vehicleName = "Vehicle";

    [Header("Vehicle Properties")]
    public float weightPounds = 3500f;

    public float initialSpeedMph = 30f;

    public float direction = 1f; //Forward (1), Backward (-1)

    [Header("Wheel Configuration")]
    public Transform[] wheelPositions = new Transform[4];
    public float wheelRadius = 0.35f;

    [Header("Runtime Data")]
    [SerializeField] private float currentSpeedMph;
    [SerializeField] private Vector3 velocityMph;

    private Rigidbody rb;
    private bool hasCollided = false;
    private Vector3 preCollisionVelocity;
    private bool velocityApplied = false;
    private Vector3 lastVelocity;

    private void Start()
    {
        SetupPhysics();
        ApplyInitialVelocity();
    }

    void SetupPhysics()
    {
        rb = GetComponent<Rigidbody>();

        //Convert weight to mass (kg)
        rb.mass = UnitConverter.PoundsToKg(weightPounds);

        //Realistic drag and angular drag
        //rb.linearDamping = 0f;
        rb.angularDamping = 0.5f;

        //Continuous collision detection for high-speed impacts
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;

        Debug.Log($"[{vehicleName}] Physics setup: {weightPounds} lbs ({rb.mass} kg)");
    }

    void ApplyInitialVelocity()
    {
        //Convert MPH to Unity velocity
        float unitySpeed = UnitConverter.MphToUnityVelocity(initialSpeedMph);

        //Apply in forward direction
        rb.linearVelocity = transform.forward * unitySpeed;

        //Debug.Log($"[{vehicleName}] Initial velocity: {initialSpeedMph} MPH = {unitySpeed} Unity units/sec");
    }

    void FixedUpdate()
    {
        lastVelocity = rb.linearVelocity;

        if (!hasCollided)
        {
            float unitySpeed = UnitConverter.MphToUnityVelocity(initialSpeedMph);
            rb.linearVelocity = transform.forward * unitySpeed;
        }

        UpdateRuntimeData();
    }

    void UpdateRuntimeData()
    {
        //Calculate current speed in MPH
        float unitySpeed = rb.linearVelocity.magnitude;
        currentSpeedMph = UnitConverter.UnityVelocityToMph(unitySpeed);
        velocityMph = UnitConverter.UnityVelocityToMph(rb.linearVelocity);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (hasCollided) 
            return;

        CrashVehicle otherVehicle = collision.gameObject.GetComponent<CrashVehicle>();
        if (otherVehicle != null)
        {
            hasCollided = true;
            preCollisionVelocity = lastVelocity;

            //Calculate impact data
            Vector3 impactPoint = collision.contacts[0].point;
            Vector3 impactNormal = collision.contacts[0].normal;
            float impactForce = collision.impulse.magnitude / Time.fixedDeltaTime;

            Debug.Log($"[COLLISION] {vehicleName} hit {otherVehicle.vehicleName}");
            Debug.Log($"Impact Point: {UnitConverter.UnityToFeet(impactPoint)} feet");
            Debug.Log($"Impact Force: {UnitConverter.NewtonsToPoundForce(impactForce):F0} lbf");
            Debug.Log($"Pre-collision Speed: {UnitConverter.UnityVelocityToMph(preCollisionVelocity.magnitude):F1} MPH");
        }
    }

    public float GetCurrentSpeedMph() => currentSpeedMph;
    public Vector3 GetVelocityMph() => velocityMph;
    public Rigidbody GetRigidbody() => rb;
    public bool HasCollided() => hasCollided;
    public Vector3 GetPreCollisionVelocity() => preCollisionVelocity;
}
