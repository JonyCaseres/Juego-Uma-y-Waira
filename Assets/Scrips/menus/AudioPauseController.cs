using UnityEngine;

public class AudioPauseController : MonoBehaviour
{
    [SerializeField] private AudioSource[] gameMusicSources;
    private bool isPaused;

    private void Awake()
    {
        if (gameMusicSources == null || gameMusicSources.Length == 0)
            gameMusicSources = FindObjectsOfType<AudioSource>();
    }

    public void PauseAll()
    {
        if (isPaused) return;
        if (MenuSceneManager.Instance != null) MenuSceneManager.Instance.PauseMusic();
        foreach (var src in gameMusicSources)
        {
            if (src == null) continue;
            if (src.isPlaying) src.Pause();
        }
        isPaused = true;
    }

    public void ResumeAll()
    {
        if (!isPaused) return;
        if (MenuSceneManager.Instance != null) MenuSceneManager.Instance.ResumeMusic();
        foreach (var src in gameMusicSources)
        {
            if (src == null) continue;
            src.UnPause();
        }
        isPaused = false;
    }

    public void TogglePause()
    {
        if (isPaused) ResumeAll(); else PauseAll();
    }
}