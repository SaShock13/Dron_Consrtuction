using TMPro;
using UnityEngine;

public class FPS_View : MonoBehaviour
{
    [SerializeField, Tooltip("Интервал обновления в секундах")]
    private float updateInterval = 0.5f;

    private float timeLeft;
    private float currentFps;


    [SerializeField] private TMP_Text text;

    private void Start()
    {
        timeLeft = updateInterval;
    }

    private void Update()
    {
        timeLeft -= Time.unscaledDeltaTime;
        if (timeLeft <= 0f)
        {
            // текущее мгновенное FPS
            currentFps = 1f / Time.unscaledDeltaTime;
            timeLeft = updateInterval;
            text.text = currentFps.ToString("F0");
        }
    }
}
