using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AnotherTest : MonoBehaviour
{

    [SerializeField] List<Texture2D> textures = new();

    [SerializeField] Renderer render;

    Material newMat;

    private void Start()
    {
        // Creates a new material to edit without editing the og
        newMat = new Material(render.material);
        render.material = newMat;

        // Sets the secondary albedo to the first texture in the index
    }

    // Updates texture based on passthrough of index assaigned to a slider change value
    public void UpdateTexture(Single index)
    {
        int temp = index.ConvertTo<Int32>();

        newMat.mainTexture = textures[temp - 1];
        render.material = newMat;
    }

}