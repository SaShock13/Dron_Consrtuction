using UnityEngine;

[CreateAssetMenu(fileName = "MaterialList", menuName ="Scriptable Objects/materialList")]
public class MaterialList : ScriptableObject
{

    [SerializeField] public Material[] list ;
}
