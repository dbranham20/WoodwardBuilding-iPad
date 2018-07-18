using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinchZoom : MonoBehaviour 
{
    private static readonly float PanSpeed = 60f;
    private static readonly float ZoomSpeedTouch = 0.1f;
    private static readonly float ZoomSpeedMouse = 0.5f;

    private static readonly float[] BoundsX = new float[] { -10f, 100f };
    private static readonly float[] BoundsZ = new float[] { -18f, 200f };
    private static readonly float[] ZoomBounds = new float[] { 10f, 85f };

    private Camera cam;

    private Vector3 lastPanPosition;
    private int panFingerId; // Touch mode only

    private bool wasZoomingLastFrame; // Touch mode only
    private Vector2[] lastZoomPositions; // Touch mode only

    void Awake()
    {
        cam = GetComponent<Camera>();
    }

    void Update() //runs every frame
    {
        if (Input.touchSupported && Application.platform != RuntimePlatform.WebGLPlayer) //if using mobile device
        {
            HandleTouch();
        }
        else //if using mouse. for testing purposes only right now
        {
            HandleMouse();
        }
    }

    void HandleTouch()
    {
        switch (Input.touchCount) //get touch count
        {

            case 1: // if one touch, we are panning
                wasZoomingLastFrame = false;

                Touch touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Began) //if the touch began, save its screen position and unique ID
                {
                    lastPanPosition = touch.position;
                    panFingerId = touch.fingerId;
                }

                else if (touch.fingerId == panFingerId && touch.phase == TouchPhase.Moved) //if touching finger moved, call PanCamera()
                {
                    PanCamera(touch.position);
                }
                break;

            case 2: // if two touch, we are zooming
                Vector2[] newPositions = new Vector2[] { Input.GetTouch(0).position, Input.GetTouch(1).position };
                if (!wasZoomingLastFrame)
                {
                    lastZoomPositions = newPositions;
                    wasZoomingLastFrame = true;
                }
                else
                {
                    // Zoom based on the distance between the new positions compared to the 
                    // distance between the previous positions.
                    float newDistance = Vector2.Distance(newPositions[0], newPositions[1]);
                    float oldDistance = Vector2.Distance(lastZoomPositions[0], lastZoomPositions[1]);
                    float offset = newDistance - oldDistance;

                    ZoomCamera(offset, ZoomSpeedTouch);

                    lastZoomPositions = newPositions;
                }
                break;

            default:
                wasZoomingLastFrame = false;
                break;
        }
    }

    void HandleMouse()
    {
        // On mouse down, capture it's position.
        // Otherwise, if the mouse is still down, pan the camera.
        if (Input.GetMouseButtonDown(0))
        {
            lastPanPosition = Input.mousePosition;
        }
        else if (Input.GetMouseButton(0))
        {
            PanCamera(Input.mousePosition);
        }

        // Check for scrolling to zoom the camera
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        ZoomCamera(scroll, ZoomSpeedMouse);
    }

    void PanCamera(Vector3 newPanPosition)
    {
        // Determine how much to move the camera
        Vector3 offset = cam.ScreenToViewportPoint(lastPanPosition - newPanPosition);
        Vector3 move = new Vector3(-(offset.y * PanSpeed), 0, offset.x * PanSpeed);

        // Perform the movement
        transform.Translate(move, Space.World);

        // Ensure the camera remains within bounds.
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(transform.position.x, BoundsX[0], BoundsX[1]);
        pos.z = Mathf.Clamp(transform.position.z, BoundsZ[0], BoundsZ[1]);
        transform.position = pos;

        // Cache the position
        lastPanPosition = newPanPosition;
        //Debug.Log(transform.position);
    }

    void ZoomCamera(float offset, float speed)
    {
        if (offset == 0.0f)
        {
            return;
        }

        cam.fieldOfView = Mathf.Clamp(cam.fieldOfView - (offset * speed), ZoomBounds[0], ZoomBounds[1]);
    }
}
