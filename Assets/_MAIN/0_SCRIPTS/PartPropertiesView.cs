using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class PartPropertiesView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TMP_Dropdown materialDropdown;
    [SerializeField] private Slider smoothnessSlider;
    [SerializeField] private Slider sizeSlider;
    [SerializeField] private Button colorButton; 
    private IPropertiesChangable currentPart;
    private MaterialList _materialList;


    [Inject]
    public void Construct(MaterialList materialList)
    {
        _materialList = materialList;
    }


    void OnEnable()
    {
        FindFirstObjectByType<Selector>().OnPartSelected += ShowPartPanel;
    }
    void OnDisable()
    {
        FindFirstObjectByType<Selector>().OnPartSelected -= ShowPartPanel;

    }

    private void ShowPartPanel(IPropertiesChangable part) 
    {

        Debug.Log($"ShowForPart {this}");
        currentPart = part;
        nameText.text = part.PartName;

        //Материалы
        materialDropdown.onValueChanged.RemoveAllListeners();
        materialDropdown.ClearOptions();
        var names = _materialList.list.Select(m => m.name).ToList();
        materialDropdown.AddOptions(names);
        materialDropdown.onValueChanged.AddListener(i => part.SetMaterial(_materialList.list[i]));

        // Smoothness
        smoothnessSlider.onValueChanged.RemoveAllListeners();
        smoothnessSlider.value = part.Smoothness; 
        smoothnessSlider.onValueChanged.AddListener(v => part.SetSmoothness(v));

        // Масштаб
        sizeSlider.onValueChanged.RemoveAllListeners();
        sizeSlider.value = part.LocalScale.x;
        sizeSlider.onValueChanged.AddListener(v => part.SetSize(v));

        // Цвет
        colorButton.onClick.AddListener(() =>
        {
            // todo При первом нажатии нулевые слайдеры, сделать текущие цвета (при повторном работает норм)
            ColorPicker.Show(part.GetColor, color => currentPart.SetColor(color));
        });

        gameObject.SetActive(true);
    }
}
