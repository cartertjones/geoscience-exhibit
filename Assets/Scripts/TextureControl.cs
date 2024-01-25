using System;
using System.Collections.Generic;
using UnityEngine;

public class TextureControl : MonoBehaviour
{
    [SerializeField] List<Texture2D> overlayTextures = new List<Texture2D>();

    [SerializeField] Renderer render;

    [SerializeField] bool defaultOverlayState = true;

    Material newMat;

    // Variable to control the blend factor
    [Range(0, 1)] public float blendFactor = 1.0f;

    // Variable to store the original texture
    private Texture2D originalTexture;

    private void Start()
    {
        // Creates a new material to edit without editing the original
        newMat = new Material(render.material);
        render.material = newMat;

        // Store the original texture
        originalTexture = (Texture2D)render.material.mainTexture;
        newMat.SetFloat("_Blend", 0);

        // Set initial overlay state, was requiring multiple presses of the toggle to work before
        ToggleTextureOverlay(defaultOverlayState);
    }

    public void OnSliderValueChanged(float value)
    {
        // Convert slider value to an index
        int index = Mathf.FloorToInt(value);
        UpdateOverlayTexture(index);
    }

    // Updates texture based on passthrough of index assigned to a slider change value
    public void UpdateOverlayTexture(int index)
    {
        index = Mathf.Clamp(index, 0, overlayTextures.Count - 1);
        // Check if the index is valid
        if (index >= 0 && index < overlayTextures.Count)
        {
            Texture2D selectedOverlay = overlayTextures[index];

            // Assuming 'render' is your Renderer component
            Material mat = render.material; // Get a reference to the material

            // Update the overlay texture
            mat.SetTexture("_OverlayTex", selectedOverlay);
            mat.SetFloat("_Blend", blendFactor);
        }
        else
        {
            Debug.LogError("Invalid texture index." + index);
        }
    }

    // Method to enable or disable texture overlay
    public void ToggleTextureOverlay(bool isEnabled)
    {
        if (isEnabled)
        {
            newMat = render.material;
            // Restore the blend factor to show the overlay
            newMat.SetFloat("_Blend", blendFactor);
        }
        else
        {
            // Set blend factor to 0 to show only the base color map
            newMat.SetFloat("_Blend", 0);
        }
    }
}
