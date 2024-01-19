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
    private enum State { Idle, Active }
    [SerializeField] private State state;

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

    [Header("Dragging")]
    //Used for dragging
    public float dragSpeed = 10;
    private Vector3 dragOrigin;

    [Header("Zooming")]
    //Used for zooming
    [SerializeField] private float zoomSpeed = 5;
    [SerializeField] private float zoomCapLow = 10;
    [SerializeField] private float zoomCapHigh = 90;
    private float startFov;

    [Header("Rotation")]
    //How quickly the camera rotates
    [SerializeField] private float rotateSpeed = 10;
    private Vector3 rotationOrigin;
    private Vector2 rotationDirection;

    //Used for manually controlling rotation
    private float rotationX = 0f;
    private float rotationY = 0f;

    //The limits of rotation
    [SerializeField] private float rotationCapMin = 20;
    [SerializeField] private float rotationCapMax = 90;

    [Header("Idle Timer")]
    //How long the program has to go without input before returning to idle
    [SerializeField] private float timeToReturnToIdle = 10;
    //How much longer until the program goes idle
    private float currentTimer;

    #endregion

    private void Start()
    {
        //Save the camera component
        cam = GetComponent<Camera>();

        //Save the start position
        startPosition = transform.position;

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
            Rotate();
            Move();
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

    //The code for rotating the camera
    //Disabled for demo
    private void Rotate()
    {
        /*
        if (Input.GetMouseButton(1))
        {
            //Multiplying by the field of view makes it so that when the user is zoomed in further,
            //the rotation is more precise

            #region Mouse
            //Field of view is divided by 10 to make sure rotateSpeed doesn't have to be a really low number

            //Temporarily commented out for demo, introduces bugs when 
            //rotationX += Input.GetAxis("Mouse X") * rotateSpeed * (cam.fieldOfView / 10);

            rotationY += Input.GetAxis("Mouse Y") * rotateSpeed * (cam.fieldOfView / 10);
            #endregion

            rotationY = Mathf.Clamp(rotationY, rotationCapMin, rotationCapMax);
            transform.localEulerAngles = new Vector3(rotationY, -rotationX, 0);

            ResetTimer();
        }
        */
        transform.localEulerAngles = new Vector3(45, 0, 0);
    }

    //The code for moving the camera
    //As of right now, moving the camera doesn't work quite as ideally as we would like.
    //It is the next priority, alongside making it work with mobile.
    private void Move()
    {
        //To prevent moving while zooming, rotating, etc.
        if (Input.touchCount <= 1)
        {
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, 0, transform.localEulerAngles.z);
            //Get a tap
            if (Input.touchCount == 1)
            {
                if (Input.GetTouch(0).phase == TouchPhase.Began)
                {
                    dragOrigin = Input.GetTouch(0).position - Input.GetTouch(0).deltaPosition;
                    ResetTimer();
                    return;
                }
            }

            else if (Input.GetMouseButtonDown(0))
            {
                dragOrigin = Input.mousePosition;
                ResetTimer();
                return;
            }
            if (!Input.GetMouseButton(0)) return;

            //In case the user holds for longer than the timer duration
            if (Input.GetMouseButton(0)) ResetTimer();

            //Move the camera
            Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - dragOrigin);
            Vector3 move = Vector3.zero;

            move = new Vector3(pos.x * dragSpeed * Time.deltaTime, 0, pos.y * dragSpeed * Time.deltaTime);

            /*
                    Commented out for demo

                    //This is a very roundabout way of going about it, and it isn't the best, but it works
                    if (transform.localEulerAngles.y < 45 && transform.localEulerAngles.y > 0 || (transform.localEulerAngles.y > 315 && transform.localEulerAngles.y < 360))
                    {
                        move = new Vector3(pos.x * dragSpeed * Time.deltaTime, 0, pos.y * dragSpeed * Time.deltaTime);
                    }
                    else if (transform.localEulerAngles.y > 135 && transform.localEulerAngles.y < 225)
                    {
                        move = new Vector3(-pos.x * dragSpeed * Time.deltaTime, 0, -pos.y * dragSpeed * Time.deltaTime);
                    }
                    else if (transform.localEulerAngles.y > 45 && transform.localEulerAngles.y < 135)
                    {
                        move = new Vector3(pos.y * dragSpeed * Time.deltaTime, 0, pos.x * dragSpeed * Time.deltaTime);
                    }
                    else if (transform.localEulerAngles.y > 225 && transform.localEulerAngles.y < 315)
                    {
                        move = new Vector3(-pos.y * dragSpeed * Time.deltaTime, 0, -pos.x * dragSpeed * Time.deltaTime);
                    }
            */
            transform.Translate(-move, Space.World);
        }

    }

    //Reset the timer
    private void ResetTimer() => currentTimer = timeToReturnToIdle;

    //Change state
    private void UpdateState(State newState)
    {
        //Update the state
        state = newState;
        //Reset the position
        transform.position = startPosition;

        if (newState == State.Idle)
        {
            //Reset the FOV
            cam.fieldOfView = startFov;
            //Look at the orbit target right away

            transform.LookAt(orbitTarget.position);
        }
        else if (newState == State.Active)
        {
            ResetTimer();
            //Save the rotation angles

            Vector3 lookrot = new Vector3(45, 0, 0);
            transform.localEulerAngles = lookrot;

            rotationX = -transform.rotation.eulerAngles.y;
            rotationY = transform.rotation.eulerAngles.x;
        }
    }
}
