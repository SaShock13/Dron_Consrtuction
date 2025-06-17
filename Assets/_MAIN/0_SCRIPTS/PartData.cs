using UnityEngine;

[CreateAssetMenu(fileName = "dronePart", menuName ="Scriptable Objects/ Drone Parts")]
public class PartData : ScriptableObject
{
    public PartType type;
    public GameObject partPrefab;
    public string id;
    public Vector3 localPositionOffset;
    public Vector3 localRotationOffset;
}
