using System;
using UnityEngine;
using UnityEngine.UI;

public class ColorPicker : MonoBehaviour
{
    public static ColorPicker Instance;

    [Header("UI References")]
    public GameObject panel;
    public Image colorPreview;
    public Slider redSlider;
    public Slider greenSlider;
    public Slider blueSlider;
    public Slider alphaSlider;
    public Button applyButton;
    public Button cancelButton;

    private Action<Color> OnColorSelected;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        panel.SetActive(false);

        // Слушатели UI
        redSlider.onValueChanged.AddListener(_ => UpdatePreview());
        greenSlider.onValueChanged.AddListener(_ => UpdatePreview());
        blueSlider.onValueChanged.AddListener(_ => UpdatePreview());
        alphaSlider.onValueChanged.AddListener(_ => UpdatePreview());

        applyButton.onClick.AddListener(OnApplyClicked);
        cancelButton.onClick.AddListener(() => panel.SetActive(false));
    }

    public static void Show(Color startColor, Action<Color> callback)
    {

        Debug.Log($"startColor {startColor}");
        if (Instance == null)
        {
            Debug.LogError("ColorPicker not found in scene!");
            return;
        }

        Instance.panel.SetActive(true);
        Instance.OnColorSelected = callback;
        Instance.SetSliders(startColor);
    }

    private void SetSliders(Color color)
    {
        redSlider.SetValueWithoutNotify(color.r);
        greenSlider.SetValueWithoutNotify(color.g);
        blueSlider.SetValueWithoutNotify(color.b);
        //alphaSlider.SetValueWithoutNotify(color.a);
        UpdatePreview();
    }

    private void UpdatePreview()
    {
        Color currentColor = new Color(
            redSlider.value,
            greenSlider.value,
            blueSlider.value,
            1
        );
        colorPreview.color = currentColor;
    }

    private void OnApplyClicked()
    {
        Color selectedColor = colorPreview.color;
        panel.SetActive(false);
        OnColorSelected?.Invoke(selectedColor);
    }
}
