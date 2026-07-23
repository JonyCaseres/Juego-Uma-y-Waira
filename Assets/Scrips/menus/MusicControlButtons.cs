using UnityEngine;

public class MusicControlButtons : MonoBehaviour
{
    private AudioPauseController audioPauseController;

    private void Awake()
    {
        audioPauseController = FindObjectOfType<AudioPauseController>();
    }

    public void PauseMusic()
    {
        if (MenuSceneManager.Instance != null) MenuSceneManager.Instance.PauseMusic();
        if (audioPauseController != null) audioPauseController.PauseAll();
    }

    public void ResumeMusic()
    {
        if (MenuSceneManager.Instance != null) MenuSceneManager.Instance.ResumeMusic();
        if (audioPauseController != null) audioPauseController.ResumeAll();
    }
}