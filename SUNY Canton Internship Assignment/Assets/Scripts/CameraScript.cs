using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;

public class CameraScript : MonoBehaviour
{
    [Header("References")]
    public Camera cam;

    [Header("Zoom Settings")]
    public float zoomSpeed = 10f;
    public float minZoom = 5f;
    public float maxZoom = 15f;
    public float zoomSmoothSpeed = 10f;

    [Header("Movement Settings")]
    public float moveSpeed = 10f;

    [Header("Bounds (X = left/right, Y = bottom/top)")]
    public Vector2 minBounds;
    public Vector2 maxBounds;

    [Header("Default State")]
    public Vector3 defaultPosition;
    public float defaultZoom = 10f;

    private bool isMagnified = false;
    private float targetZoom;

    public Image magnifyButtonImage;
    public Sprite isInMagnified, isNotInMagnified;

    void Start()
    {
        if (cam == null)
            cam = Camera.main;

        // Initialize
        defaultPosition = transform.position;
        targetZoom = defaultZoom;

        if (cam.orthographic)
            cam.orthographicSize = defaultZoom;
        else
            cam.fieldOfView = defaultZoom;
    }

    void Update()
    {
        if (isMagnified)
        {
            HandleZoom();
            HandleMovement();
        }

        ApplyZoom();
        ClampPosition();
        HandleReturnToDefault();
    }

    public void HandleToggle()
    {
        isMagnified = !isMagnified;

        if (!isMagnified)
        {
            targetZoom = defaultZoom;
            magnifyButtonImage.sprite = isNotInMagnified;
        }
        else
        {
            magnifyButtonImage.sprite = isInMagnified;
        }
    }

    void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll != 0f)
        {
            targetZoom -= scroll * zoomSpeed;
            targetZoom = Mathf.Clamp(targetZoom, minZoom, maxZoom);
        }
    }

    void ApplyZoom()
    {
        if (cam.orthographic)
        {
            cam.orthographicSize = Mathf.Lerp(
                cam.orthographicSize,
                targetZoom,
                Time.deltaTime * zoomSmoothSpeed
            );
        }
        else
        {
            cam.fieldOfView = Mathf.Lerp(
                cam.fieldOfView,
                targetZoom,
                Time.deltaTime * zoomSmoothSpeed
            );
        }
    }

    void HandleMovement()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 move = new Vector3(h, 0f, v) * moveSpeed * Time.deltaTime;
        transform.position += move;
    }

    void ClampPosition()
    {
        Vector3 pos = transform.position;

        float camHeight;
        float camWidth;

        if (cam.orthographic)
        {
            camHeight = cam.orthographicSize;
            camWidth = camHeight * cam.aspect;
        }
        else
        {
            camHeight = Mathf.Tan(cam.fieldOfView * 0.5f * Mathf.Deg2Rad) * transform.position.y;
            camWidth = camHeight * cam.aspect;
        }

        pos.x = Mathf.Clamp(pos.x, minBounds.x + camWidth, maxBounds.x - camWidth);
        pos.z = Mathf.Clamp(pos.z, minBounds.y + camHeight, maxBounds.y - camHeight);

        transform.position = pos;
    }

    void HandleReturnToDefault()
    {
        if (!isMagnified)
        {
            transform.position = Vector3.Lerp(
                transform.position,
                defaultPosition,
                Time.deltaTime * 5f
            );
        }
    }
}
