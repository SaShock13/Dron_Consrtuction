using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;

public class MoveVignette : MonoBehaviour
{
    [Header("Vignette Settings")]
    [SerializeField] VolumeProfile volumeProfile;
    [SerializeField] float minIntensity = 0.2f;
    [SerializeField] float maxIntensity = 0.6f;
    [SerializeField] float speedForMax = 5f; // скорость, при которой виньетка достигает max

    private Vignette vignette;
    private CharacterController controller; // может быть и другой компонент движения

    void Awake()
    {
        controller = GetComponent<CharacterController>();

        // Ищем Vignette внутри VolumeProfile
        if (!volumeProfile.TryGet<Vignette>(out vignette))
        {
            Debug.LogError("Vignette override не найден в указанном VolumeProfile!");
        }
    }

    void Update()
    {
        if (vignette == null) return;

        // Получаем текущую скорость
        float speed = controller.velocity.magnitude;
        // Нормализуем в диапазон [0,1]
        float t = Mathf.Clamp01(speed / speedForMax);
        // Интерполируем интенсивность
        float intensity = Mathf.Lerp(minIntensity, maxIntensity, t);
        vignette.intensity.value = intensity;
    }
}
