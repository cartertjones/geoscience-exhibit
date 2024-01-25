using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SkyboxRotate : MonoBehaviour
{
    [SerializeField] private float skyRotSpeed = -0.5f;

    private void Start()
    {
        RenderSettings.skybox.SetFloat("_Rotation", 0);
    }

    private void Update()
    {
        float rotation = Mathf.Repeat(SkyboxRotation() + skyRotSpeed * Time.deltaTime, 360f);
        RenderSettings.skybox.SetFloat("_Rotation", rotation);
    }

    private float SkyboxRotation()
    {
        return RenderSettings.skybox.GetFloat("_Rotation");
    }

    private void OnApplicationQuit()
    {
#if UNITY_EDITOR
        RenderSettings.skybox.SetFloat("_Rotation", 0);
#endif
    }
}
