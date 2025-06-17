using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class DronePart : MonoBehaviour, IPropertiesChangable
{

    //public PartData data;

    //public PartType Type => data.type;


    [SerializeField] private string partName;
    [SerializeField] private Material[] availableMaterials;
    [SerializeField] private Vector3[] sizePresets;
    public float Smoothness => materialPropertyBlock.GetFloat("_Smoothness");



    [SerializeField] private Renderer renderer;
    private MaterialPropertyBlock materialPropertyBlock;

    public string PartName => partName;
    public Material[] AvailableMaterials => availableMaterials;
    public Vector3[] SizePresets => sizePresets;
    public Vector3 LocalScale => transform.localScale;

    public Color GetColor
    {
        get
        {
            renderer.GetPropertyBlock(materialPropertyBlock);
            if (materialPropertyBlock.HasColor("_BaseColor"))
                return materialPropertyBlock.GetColor("_BaseColor");
            else
            {
                return renderer.sharedMaterial.GetColor("_BaseColor");
            }
        }
    }

    private void Awake()
    {
        materialPropertyBlock = new MaterialPropertyBlock();
        renderer.material = new Material(renderer.sharedMaterial);
    }

    public void SetMaterial(Material mat)
    {
        renderer.sharedMaterial = mat;
    }

    public void SetColor(Color col)
    {
        renderer.GetPropertyBlock(materialPropertyBlock);
        materialPropertyBlock.SetColor("_BaseColor", col);
        renderer.SetPropertyBlock(materialPropertyBlock);
    }

    public void SetSmoothness(float value)
    {
        renderer.GetPropertyBlock(materialPropertyBlock);
        materialPropertyBlock.SetFloat("_Smoothness", value);
        renderer.SetPropertyBlock(materialPropertyBlock);
    }

    public void SetSize(float value)
    {
        var newScale = new Vector3(value, value, value);
        Debug.Log($"newScale {newScale}");
        transform.localScale = newScale ;
        Debug.Log($"[Check] Applied scale = {transform.localScale}");
    }

    public void Highlight(bool on) // todo пережелать на конт урное выделение
    {
        renderer.GetPropertyBlock(materialPropertyBlock);
        float intensity = on ? 0.3f : 0f;
        Color emission = Color.yellow * intensity;
        Color emColor = on ? emission : Color.black;
        materialPropertyBlock.SetColor("_EmissionColor", emColor);
        renderer.SetPropertyBlock(materialPropertyBlock);
        if (on)
            renderer.material.EnableKeyword("_EMISSION");
        else
        {
            renderer.material.DisableKeyword("_EMISSION");

        }
    }
}
