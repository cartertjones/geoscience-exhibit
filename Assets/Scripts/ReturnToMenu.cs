using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnToMenu : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private OrbitCamera orbitCamera;
    [SerializeField] private SceneLoader sceneLoader;

    [Space(5)]
    [Header("Timer")]
    [SerializeField] private float menuReturnTime = 20;
    private float timer;

    private void Start()
    {
        timer = menuReturnTime;
    }

    private void Update()
    {
        if (orbitCamera.state == OrbitCamera.State.Idle)
        {
            if (timer > 0)
                timer -= Time.deltaTime;
            else
                sceneLoader.LoadScene();
        }
        else
            timer = menuReturnTime;
    }


}

