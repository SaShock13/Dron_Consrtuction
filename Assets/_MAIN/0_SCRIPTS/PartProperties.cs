using TMPro;
using UnityEngine;

public class PartProperties : MonoBehaviour
{

    [SerializeField] private TMP_Text text ;

    public void ShowProperties(GameObject part)
    {
        text.text = part.name;
    }
}
