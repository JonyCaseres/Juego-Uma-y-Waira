using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButton : MonoBehaviour
{
    [SerializeField] private string mainMenuSceneName = "MainMenu";

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        if (MenuSceneManager.Instance != null)
        {
            MenuSceneManager.Instance.PauseMusic();
            MenuSceneManager.Instance.Load(mainMenuSceneName);
            return;
        }
        SceneManager.LoadScene(mainMenuSceneName);
    }
}