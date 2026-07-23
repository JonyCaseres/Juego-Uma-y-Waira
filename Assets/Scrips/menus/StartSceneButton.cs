using UnityEngine;
using UnityEngine.SceneManagement;

public class StartSceneButton : MonoBehaviour
{
    [SerializeField] private string sceneName;

    public void StartGame()
    {
        if (MenuSceneManager.Instance != null)
            MenuSceneManager.Instance.PauseMusic();

        if (!string.IsNullOrEmpty(sceneName))
        {
            if (MenuSceneManager.Instance != null)
            {
                MenuSceneManager.Instance.Load(sceneName);
                return;
            }
            SceneManager.LoadScene(sceneName);
            return;
        }

        int current = SceneManager.GetActiveScene().buildIndex;
        int next = current + 1;
        if (next < SceneManager.sceneCountInBuildSettings)
            SceneManager.LoadScene(next);
        else
            Debug.LogWarning("No scene specified and no siguiente escena en Build Settings.");
    }
}