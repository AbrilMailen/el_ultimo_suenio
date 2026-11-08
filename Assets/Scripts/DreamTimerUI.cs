using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Muestra el tiempo de sueño en pantalla (texto y barra). Solo escucha al DreamTimer, no lo modifica.
public class DreamTimerUI : MonoBehaviour
{
    [SerializeField] private DreamTimer _timer;
    [SerializeField] private TMP_Text _timeText;
    [SerializeField] private Color _normalColor = Color.white;
    [SerializeField] private Color _lowTimeColor = Color.red;
    [SerializeField] private float _lowTimeThreshold = 10f;

    [Header("Barra de tiempo")]
    [SerializeField] private Slider _slider;  // UI > Slider, con Interactable apagado
    [SerializeField] private Image _fillImage; // la imagen "Fill" del slider (para cambiarle el color)
    [SerializeField] private Color _fullColor = new Color(0.3f, 0.85f, 0.4f);  // verde
    [SerializeField] private Color _midColor = new Color(1f, 0.85f, 0.2f);     // amarillo
    [SerializeField] private Color _emptyColor = new Color(0.9f, 0.2f, 0.2f);  // rojo

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

        UpdateBar(seconds);
    }

    private void UpdateBar(float seconds)
    {
        if (_slider == null)
            return;

        // 1 = tiempo completo, 0 = se acabó
        float fraction = Mathf.Clamp01(seconds / _timer.MaxTime);
        _slider.value = fraction;

        if (_fillImage == null)
            return;

        // Verde -> amarillo en la primera mitad, amarillo -> rojo en la segunda
        _fillImage.color = fraction > 0.5f
            ? Color.Lerp(_midColor, _fullColor, (fraction - 0.5f) * 2f)
            : Color.Lerp(_emptyColor, _midColor, fraction * 2f);
    }
}
