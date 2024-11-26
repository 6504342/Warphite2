using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class Outline : MonoBehaviour
{
    public Color outlineColor = Color.black;
    public float outlineWidth = 0.1f;

    private void OnEnable()
    {
        Renderer renderer = GetComponent<Renderer>();
        Material[] materials = renderer.materials;

        foreach (Material mat in materials)
        {
            mat.SetColor("_OutlineColor", outlineColor);
            mat.SetFloat("_Outline", outlineWidth);
        }
    }
}

