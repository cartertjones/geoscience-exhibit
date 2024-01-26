//A script to control the camera
//By Mae Bridgeman 2024

//Attach to the Main Camera

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitCamera : MonoBehaviour
{
    #region Variables
    //Used to manage if the player has recently interacted
    //Idle = The player has not recently interacted, orbit around the model
    //Active = The player has recently interacted, and can move about the model freely
    public enum State { Idle, Active }
    public State state;

    //The camera component
    private Camera cam;

    [Header("Orbit")]
    //How quickly the camera orbits around the model when the player is idle
    //In degrees per second
    [SerializeField] private float orbitSpeed = 20f;

    //The point we want the camera to orbit around
    //Orbit distance is controlled by the start position of the camera
    [SerializeField] private Transform orbitTarget;

    //The position of the camera when the program starts
    private Vector3 startPosition;
    [SerializeField] private GameObject orbitStartPoint;

    [Header("Zooming")]
    //Used for zooming
    [SerializeField] private float zoomSpeed = 5;
    [SerializeField] private float zoomCapLow = 10;
    [SerializeField] private float zoomCapHigh = 90;
    private float startFov;

    [Header("Idle Timer")]
    //How long the program has to go without input before returning to idle
    [SerializeField] private float timeToReturnToIdle = 10;
    //How much longer until the program goes idle
    private float currentTimer;

    [SerializeField] private CameraControl cameraControl;

    #endregion

    private void Start()
    {
        //Save the camera component
        cam = GetComponent<Camera>();

        //Save the start position
        startPosition = orbitStartPoint.transform.position;

        //Save the start FOV
        startFov = cam.fieldOfView;

        //When the program starts, make it start in the idle state if it isn't already
        UpdateState(State.Idle);
    }

    private void Update()
    {
        //If the state is Idle...
        if (state == State.Idle)
        {
            //Rotate around the orbitTarget
            transform.RotateAround(orbitTarget.position, Vector3.up, orbitSpeed * Time.deltaTime);

            //If the player interacts, set the state to Active
            if (Input.GetMouseButtonDown(0)) UpdateState(State.Active);
        }
        //If the state is Active...
        else if (state == State.Active)
        {
            //Run the code for the timer, zooming, rotating, and moving
            Timer();
            Zoom();

            if (Input.GetMouseButton(0)) ResetTimer();
        }
    }

    //The timer for returning to idle
    private void Timer()
    {
        //If there is time left on the timer, reduce it by the amount of time since the last frame
        if (currentTimer > 0)
        {
            currentTimer -= Time.deltaTime;
        }
        else
        {
            //If time runs out, return to idle and send a Debug statement
            UpdateState(State.Idle);
            Debug.Log("User didn't interact for " + timeToReturnToIdle + " seconds. Returning to orbit.");
        }
    }

    //The code for zooming in and out
    private void Zoom()
    {
        //Mobile
        if (Input.touchCount == 2)
        {
            // Store both touches.
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            // Find the position in the previous frame of each touch.
            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            // Find the magnitude of the vector (the distance) between the touches in each frame.
            float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

            // Find the difference in the distances between each frame.
            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

            cam.fieldOfView += (deltaMagnitudeDiff * zoomSpeed) / 50;
            cam.fieldOfView = Mathf.Clamp(cam.fieldOfView, zoomCapLow, zoomCapHigh);
        }

        //Zoom with the scrollwheel
        float scrollWheelChange = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scrollWheelChange) > 0)
        {
            cam.fieldOfView -= scrollWheelChange * zoomSpeed;
            cam.fieldOfView = Mathf.Clamp(cam.fieldOfView, zoomCapLow, zoomCapHigh);

            ResetTimer();
        }
    }

    //Reset the timer
    public void ResetTimer() => currentTimer = timeToReturnToIdle;

    //Change state
    private void UpdateState(State newState)
    {
        //Update the state
        state = newState;

        if (newState == State.Idle)
        {
            //Reset the position
            transform.position = startPosition;

            //Reset the FOV
            cam.fieldOfView = startFov;

            //Look at the orbit target right away
            transform.LookAt(orbitTarget.position);
        }
        else if (newState == State.Active)
        {
            cameraControl.ResetCameraContainer();

            ResetTimer();
        }
    }
}
