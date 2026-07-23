using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuSceneManager : MonoBehaviour
{
    public static MenuSceneManager Instance { get; private set; }

    [Header("UI de carga (opcional)")]
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private Slider progressBar;
    [SerializeField] private Text progressText;
    [SerializeField] private float minLoadingTime = 0.25f;

    [Header("Música de menú (opcional)")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioClip menuMusic;
    [SerializeField] [Range(0f, 1f)] private float musicVolume = 1f;

    public event Action<string> OnSceneLoaded;
    public event Action<string> OnSceneUnloaded;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        SetupMusic();
    }

    private void SetupMusic()
    {
        if (musicSource == null)
            musicSource = gameObject.AddComponent<AudioSource>();

        if (menuMusic != null)
            musicSource.clip = menuMusic;

        musicSource.loop = true;
        musicSource.volume = musicVolume;

        if (!musicSource.isPlaying)
            musicSource.Play();
    }

    public void PauseMusic()
    {
        if (musicSource != null && musicSource.isPlaying)
            musicSource.Pause();
    }

    public void ResumeMusic()
    {
        if (musicSource != null && !musicSource.isPlaying)
            musicSource.UnPause();
    }

    public void Load(string sceneName)
    {
        StartCoroutine(LoadSceneCoroutine(sceneName, LoadSceneMode.Single));
    }

    public void LoadAdditive(string sceneName)
    {
        StartCoroutine(LoadSceneCoroutine(sceneName, LoadSceneMode.Additive));
    }

    public void ReloadCurrent()
    {
        var current = SceneManager.GetActiveScene().name;
        Load(current);
    }

    public void Unload(string sceneName)
    {
        StartCoroutine(UnloadSceneCoroutine(sceneName));
    }

    private IEnumerator LoadSceneCoroutine(string sceneName, LoadSceneMode mode)
    {
        if (loadingScreen != null) loadingScreen.SetActive(true);

        var startTime = Time.realtimeSinceStartup;
        var op = SceneManager.LoadSceneAsync(sceneName, mode);
        op.allowSceneActivation = false;

        while (op.progress < 0.9f)
        {
            UpdateProgressUI(op.progress);
            yield return null;
        }

        UpdateProgressUI(1f);

        var elapsed = Time.realtimeSinceStartup - startTime;
        if (elapsed < minLoadingTime)
            yield return new WaitForSeconds(minLoadingTime - elapsed);

        op.allowSceneActivation = true;
        while (!op.isDone)
            yield return null;

        UpdateProgressUI(1f);

        OnSceneLoaded?.Invoke(sceneName);

        if (loadingScreen != null) loadingScreen.SetActive(false);
    }

    private IEnumerator UnloadSceneCoroutine(string sceneName)
    {
        if (!Application.CanStreamedLevelBeLoaded(sceneName))
        {
            Debug.LogWarning($"MenuSceneManager: La escena '{sceneName}' no está disponible para descargar.");
            yield break;
        }

        var op = SceneManager.UnloadSceneAsync(sceneName);
        if (op == null)
        {
            Debug.LogWarning($"MenuSceneManager: No se pudo iniciar la descarga de '{sceneName}'.");
            yield break;
        }

        while (!op.isDone)
            yield return null;

        OnSceneUnloaded?.Invoke(sceneName);
    }

    private void UpdateProgressUI(float progress01)
    {
        if (progressBar != null) progressBar.value = progress01;
        if (progressText != null) progressText.text = $"{Mathf.RoundToInt(progress01 * 100f)}%";
    }
}