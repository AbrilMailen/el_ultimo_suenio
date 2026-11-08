using System;
using UnityEngine;

// El tiempo de sueño ES la vida de la niña.
// Baja 1 segundo por segundo; los golpes restan segundos extra.
public class DreamTimer : MonoBehaviour, IDamageable
{
    [SerializeField] private float _maxTime = 120f; // cada nivel define el suyo

    private float _currentTime;
    private int _lastShownSecond;
    private bool _hasEnded;

    public float CurrentTime => _currentTime;
    public float MaxTime => _maxTime;

    // La UI y el audio solo escuchan estos eventos
    public event Action<float> OnTimeChanged;
    public event Action OnDreamEnded;

    private void Awake()
    {
        _currentTime = _maxTime;
        _lastShownSecond = Mathf.CeilToInt(_currentTime);
    }

    private void Start()
    {
        // Start y no Awake, para que la UI ya esté suscripta
        OnTimeChanged?.Invoke(_currentTime);
    }

    private void Update()
    {
        if (_hasEnded)
            return;

        _currentTime -= Time.deltaTime;

        if (_currentTime <= 0f)
        {
            Finish();
            return;
        }

        // Avisa solo cuando cambia el segundo que se ve en pantalla
        int second = Mathf.CeilToInt(_currentTime);
        if (second != _lastShownSecond)
        {
            _lastShownSecond = second;
            OnTimeChanged?.Invoke(_currentTime);
        }
    }

    // Golpe enemigo: resta segundos extra
    public void TakeDamage(float seconds)
    {
        if (_hasEnded)
            return;

        _currentTime -= seconds;

        if (_currentTime <= 0f)
        {
            Finish();
            return;
        }

        OnTimeChanged?.Invoke(_currentTime);
    }

    // Reloj de arena: recupera tiempo (sin pasar del máximo)
    public void AddTime(float seconds)
    {
        if (_hasEnded)
            return;

        _currentTime = Mathf.Min(_currentTime + seconds, _maxTime);
        OnTimeChanged?.Invoke(_currentTime);
    }

    private void Finish()
    {
        _currentTime = 0f;
        _hasEnded = true;
        OnTimeChanged?.Invoke(_currentTime);
        OnDreamEnded?.Invoke(); // la niña despierta: derrota
    }
}
