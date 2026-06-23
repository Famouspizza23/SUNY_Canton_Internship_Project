using UnityEngine;
using UnityEngine.SceneManagement;

public class CrashSceneManager : MonoBehaviour
{
    [Header("Scenario Setup")]
    public CrashVehicle vehicle1;
    public CrashVehicle vehicle2;

    [Header("Starting Positions")]
    public float separationDistance_Feet = 100f;
    public bool headOnCollision = true;

    [Header("Controls")]
    public KeyCode resetKey = KeyCode.R;
    public KeyCode pauseKey = KeyCode.Space;

    [Header("Physics Settings")]
    public PhysicsMaterial vehiclePhysicsMaterial;
    [Range(0f, 1f)] public float bounciness = 0.3f;
    [Range(0f, 1f)] public float friction = 0.6f;

    private Vector3 v1StartPos;
    private Vector3 v2StartPos;
    private Quaternion v1StartRot;
    private Quaternion v2StartRot;
    private bool isPaused = false;

    void Start()
    {
        ValidateSetup();
        SaveStartPositions();
    }

    void ValidateSetup()
    {
        if (vehicle1 == null || vehicle2 == null)
        {
            Debug.LogError("Scene Manager: Assign both vehicles in inspector!");
            return;
        }

        Debug.Log("Scene Manager: Crash simulation ready");
    }

    void SaveStartPositions()
    {
        if (vehicle1 != null)
        {
            v1StartPos = vehicle1.transform.position;
            v1StartRot = vehicle1.transform.rotation;
        }

        if (vehicle2 != null)
        {
            v2StartPos = vehicle2.transform.position;
            v2StartRot = vehicle2.transform.rotation;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(resetKey))
        {
            ResetSimulation();
        }

        if (Input.GetKeyDown(pauseKey))
        {
            TogglePause();
        }
    }

    void ResetSimulation()
    {
        Debug.Log("Scene Manager: Resetting simulation...");

        //Reset vehicle 1
        if (vehicle1 != null)
        {
            vehicle1.transform.position = v1StartPos;
            vehicle1.transform.rotation = v1StartRot;
            Rigidbody rb1 = vehicle1.GetRigidbody();
            if (rb1 != null)
            {
                rb1.linearVelocity = Vector3.zero;
                rb1.angularVelocity = Vector3.zero;
            }
        }

        //Reset vehicle 2
        if (vehicle2 != null)
        {
            vehicle2.transform.position = v2StartPos;
            vehicle2.transform.rotation = v2StartRot;
            Rigidbody rb2 = vehicle2.GetRigidbody();
            if (rb2 != null)
            {
                rb2.linearVelocity = Vector3.zero;
                rb2.angularVelocity = Vector3.zero;
            }
        }

        //Clear skid marks
        SkidMarkGenerator[] skidMarks = FindObjectsOfType<SkidMarkGenerator>();
        foreach (var sm in skidMarks)
        {
            sm.ClearSkidMarks();
        }

        //Reload scene to reset collision flags
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void TogglePause()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0f : 1f;
    }
}
