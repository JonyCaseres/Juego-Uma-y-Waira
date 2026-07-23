using UnityEngine;
using UnityEngine.SceneManagement;

public class StartSceneButton : MonoBehaviour
{
    [SerializeField] private string sceneName;

    public void StartGame()
    {
        if (!string.IsNullOrEmpty(sceneName))
        {
            var managerType = typeof(MenuSceneManager);
            var managerField = managerType.GetProperty("Instance");
            if (managerField != null)
            {
                var manager = managerField.GetValue(null, null) as MonoBehaviour;
                if (manager != null)
                {
                    var loadMethod = managerType.GetMethod("Load");
                    if (loadMethod != null)
                    {
                        loadMethod.Invoke(manager, new object[] { sceneName });
                        return;
                    }
                }
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