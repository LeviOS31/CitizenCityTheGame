using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;

public class MaskInverter : MonoBehaviour, IMaterialModifier
{
    private static readonly int _stencilComp = Shader.PropertyToID("_StencilComp");

    // This method modifies the material to invert the stencil comparison function, allowing for masking effects in the UI.
    public Material GetModifiedMaterial(Material baseMaterial)
    {
        var resultMaterial = new Material(baseMaterial);
        resultMaterial.SetFloat(_stencilComp, Convert.ToSingle(CompareFunction.NotEqual));
        return resultMaterial;
    }
}
