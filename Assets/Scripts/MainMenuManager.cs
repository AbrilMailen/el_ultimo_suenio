
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    [Header("Paneles")]
    [SerializeField] private GameObject creditsPanel;
    [SerializeField] private GameObject optionsPanel;

    public void PlayGame()
{
    Time.timeScale = 1f;
    UnityEngine.SceneManagement.SceneManager.LoadScene("Nivel_01");
}
    public void OpenCredits()
    {
        creditsPanel.SetActive(true);
    }

    public void CloseCredits()
    {
        creditsPanel.SetActive(false);
    }
     public void OpenOptions()
    {
        optionsPanel.SetActive(true);
    }

    public void CloseOptions()
    {
        optionsPanel.SetActive(false);
    }
    // Cierra la aplicación cuando se ejecuta la Build.
    // En el Editor de Unity no produce ningún efecto.
    public void ExitGame()
{
    Application.Quit();
}
}
