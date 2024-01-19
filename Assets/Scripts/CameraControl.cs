using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [SerializeField] private float speed = 40.0f;
    [SerializeField] private OrbitCamera orbitCamera;
    private bool gyroEnabled;
    private Gyroscope gyro;
    private GameObject cameraContainer;
    private Quaternion rot;
    private bool isMoving = false;
    private Quaternion lastRot;

    private void Start()
    {
        cameraContainer = new GameObject("Camera Container");
        cameraContainer.transform.position = transform.position;
        transform.SetParent(cameraContainer.transform);

        gyroEnabled = EnableGyro();
    }

    private bool EnableGyro()
    {
        if(SystemInfo.supportsGyroscope)
        {
            gyro = Input.gyro;
            gyro.enabled = true;

            cameraContainer.transform.rotation = Quaternion.Euler(90f, 90f, 0f);
            rot = new Quaternion(0, 0, 1, 0);

            return true;
        }
        return false;
    }

    private void Update()
    {
        if (gyroEnabled && orbitCamera.state == OrbitCamera.State.Active)
        {
            transform.localRotation = gyro.attitude * rot;
        }

        if(lastRot != transform.rotation)
        {
            orbitCamera.ResetTimer();
        }

        if (isMoving)
        {
            MoveCamera();
        }
        lastRot = transform.rotation;
    }

    // Call this method when a button is pressed
    public void MoveCamera()
    {
        Vector3 direction = transform.forward;
        direction.y = 0; // Remove the Y component to keep movement on the XZ plane
        direction.Normalize(); // Normalize the vector to ensure consistent movement speed

        cameraContainer.transform.Translate(direction * speed * Time.deltaTime, Space.World);
    }

    // Call this method to start moving
    public void StartMoving()
    {
        isMoving = true;
    }

    // Call this method to stop moving
    public void StopMoving()
    {
        isMoving = false;
    }
}
