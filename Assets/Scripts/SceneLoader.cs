using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private string sceneToLoad;
    [SerializeField] private Animator transition;

    public void LoadScene()
    {
        StartCoroutine(SceneTransition());
    }

    IEnumerator SceneTransition()
    {
        // play transition animation
        transition.SetTrigger("CrossfadeStart");

        // wait
        yield return new WaitForSecondsRealtime(1);

        // load scene
        SceneManager.LoadScene(sceneToLoad);
    }
}
