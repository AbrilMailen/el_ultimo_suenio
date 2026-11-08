using TMPro;
using UnityEngine;

// Muestra el tiempo de sueño en pantalla. Solo escucha al DreamTimer, no lo modifica.
public class DreamTimerUI : MonoBehaviour
{
    [SerializeField] private DreamTimer _timer;
    [SerializeField] private TMP_Text _timeText;
    [SerializeField] private Color _normalColor = Color.white;
    [SerializeField] private Color _lowTimeColor = Color.red;
    [SerializeField] private float _lowTimeThreshold = 10f;

    private void OnEnable()
    {
        _timer.OnTimeChanged += UpdateText;
    }

    private void OnDisable()
    {
        _timer.OnTimeChanged -= UpdateText;
    }

    private void UpdateText(float seconds)
    {
        int total = Mathf.CeilToInt(seconds);
        _timeText.text = $"{total / 60:00}:{total % 60:00}";
        _timeText.color = seconds <= _lowTimeThreshold ? _lowTimeColor : _normalColor;
    }
}
