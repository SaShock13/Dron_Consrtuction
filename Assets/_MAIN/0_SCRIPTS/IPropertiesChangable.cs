using UnityEngine;

public interface IPropertiesChangable
{
    string PartName { get; }
    Material[] AvailableMaterials { get; }
    Vector3[] SizePresets { get; }
    Vector3 LocalScale { get; }
    float Smoothness { get; }
    Color GetColor { get; }

    void Highlight(bool on);

    void SetMaterial(Material mat);
    void SetColor(Color col);
    void SetSmoothness(float value);
    void SetSize(float value);

}
