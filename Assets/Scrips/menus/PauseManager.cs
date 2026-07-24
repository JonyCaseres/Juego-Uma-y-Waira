using UnityEngine;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseCanvas;
    [SerializeField] private AudioPauseController audioPauseController;
    [SerializeField] private bool listenEscape = true;

    private void Awake()
    {
        if (pauseCanvas != null) pauseCanvas.SetActive(false);
        if (audioPauseController == null) audioPauseController = FindObjectOfType<AudioPauseController>();
    }

    private void OnEnable()
    {
        PauseController.OnPauseChanged += ApplyPauseState;
    }

    private void OnDisable()
    {
        PauseController.OnPauseChanged -= ApplyPauseState;
    }

    private void Update()
    {
        if (!listenEscape) return;
        if (Input.GetKeyDown(KeyCode.Escape))
            TogglePause();
    }

    public void Pause()
    {
        PauseController.SetPaused(true);
    }

    public void Resume()
    {
        PauseController.SetPaused(false);
    }

    public void TogglePause()
    {
        PauseController.Toggle();
    }

    private void ApplyPauseState(bool paused)
    {
        if (paused)
        {
            Time.timeScale = 0f;
            audioPauseController?.PauseAll();
            if (pauseCanvas != null) pauseCanvas.SetActive(true);
        }
        else
        {
            Time.timeScale = 1f;
            audioPauseController?.ResumeAll();
            if (pauseCanvas != null) pauseCanvas.SetActive(false);
        }
    }
}