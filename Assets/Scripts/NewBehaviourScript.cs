using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    [Range(0,1)]
    public float value;
    public Material TransitionMaterial;

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        TransitionMaterial.SetFloat("_Cutoff", value);
        Graphics.Blit(source, destination, TransitionMaterial);
    }
}
