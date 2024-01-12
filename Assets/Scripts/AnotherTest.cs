using System.Collections.Generic;
using UnityEngine;

public class AnotherTest : MonoBehaviour
{
    [SerializeField] Material material, newMat;

    [SerializeField] List<Texture2D> textures = new();

    [SerializeField] Renderer render;

    [SerializeField]
    int index = 0;
    [SerializeField]
    float timeToSwap = 1f, currTime = 0f;

    private void Start()
    {
        newMat = new Material(material);
        newMat.mainTexture = textures[index];
        render.material = newMat;
    }

    private void FixedUpdate()
    {
        currTime += Time.deltaTime;

        if (currTime > timeToSwap)
        {
            currTime = 0f;
            index++;

            if(index > textures.Count - 1)
                index = 0;

            newMat.mainTexture = textures[index];
            render.material = newMat;
        }
    }
}