using UnityEngine;
using UnityEngine.UI;

public class PartPicker : MonoBehaviour
{
    [Header("UI Кнопки (6)")]
    public Button[] buttons = new Button[6];

    [Header("Объекты для переключения (6)")]
    public GameObject[] objects = new GameObject[6];

    private int currentIndex = -1;

    void Start()
    {
        // Проверка
        if (buttons.Length != objects.Length)
            Debug.LogWarning("Количество кнопок и объектов должно совпадать.");

        // Инициализация обработчиков
        for (int i = 0; i < buttons.Length; i++)
        {
            int index = i; // локальная копия для лямбды
            buttons[i].onClick.AddListener(() => OnButtonClicked(index));
        }

        // Скрыть все объекты в начале
        for (int j = 0; j < objects.Length; j++)
        {
            if (objects[j] != null)
                objects[j].SetActive(false);
        }
    }

    private void OnButtonClicked(int index)
    {
        // Если тот же индекс — ничего не делаем
        if (index == currentIndex)
            return;

        // Отключаем предыдущий
        if (currentIndex >= 0 && currentIndex < objects.Length && objects[currentIndex] != null)
            objects[currentIndex].SetActive(false);

        // Включаем новый
        if (index >= 0 && index < objects.Length && objects[index] != null)
            objects[index].SetActive(true);

        currentIndex = index;
    }
}
