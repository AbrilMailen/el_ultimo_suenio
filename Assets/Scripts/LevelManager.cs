
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelManager : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private DreamTimer _dreamTimer;
    [SerializeField] private GameObject _defeatPanel;
    [SerializeField] private GameObject _victoryPanel;

    [Header("Objetivo del nivel")]
    [SerializeField] private TMP_Text _objectiveText;

    [Header("Salida")]
    [SerializeField] private GameObject _exitBarrier; // bloquea la salida hasta recoger la flor (opcional)

    [Header("Siguiente nivel")]
    [SerializeField] private string _nextSceneName; // debe estar en Build Settings

    private bool _hasCollectedFlower;
    private bool _isFinished; // ya ganó o perdió: no se procesa nada más

    public bool HasCollectedFlower => _hasCollectedFlower;

    private void Start()
    {
        _hasCollectedFlower = false;

        if (_objectiveText != null)
        {
            _objectiveText.text =
                "Encontrá la flor del sueño.\nTodavía no la encontraste.";
        }
    }

    private void OnEnable()
    {
        if (_dreamTimer != null)
            _dreamTimer.OnDreamEnded += HandleDreamEnded;
    }

    private void OnDisable()
    {
        if (_dreamTimer != null)
            _dreamTimer.OnDreamEnded -= HandleDreamEnded;
    }

    private void HandleDreamEnded()
    {
        if (_isFinished)
            return;

        _isFinished = true;
        _defeatPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    // Lo llama la salida cuando la jugadora la toca.
    // Solo se gana si ya se recogió la flor.
    public void TryCompleteLevel()
    {
        if (_isFinished)
            return;

        if (!_hasCollectedFlower)
        {
            if (_objectiveText != null)
                _objectiveText.text = "La salida está cerrada.\nNecesitás la flor del sueño.";
            return;
        }

        _isFinished = true;
        _victoryPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void CollectFlower()
    {
        if (_hasCollectedFlower)
            return;

        _hasCollectedFlower = true;

        // Con la flor, la salida se abre
        if (_exitBarrier != null)
            _exitBarrier.SetActive(false);

        if (_objectiveText != null)
        {
            _objectiveText.text =
                "¡Flor encontrada!\nAhora buscá la salida.";
        }

        Debug.Log("Objetivo completado: flor encontrada.");
    }

    public void RetryLevel()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex
        );
    }

    // Botón "Siguiente nivel" del panel de victoria
    public void LoadNextLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(_nextSceneName);
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MenuPrincipal");
    }
}
