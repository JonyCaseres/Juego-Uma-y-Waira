using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class LossCanvasVideoPlayer : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject lossCanvas;              // Canvas (puede estar desactivado)
    [SerializeField] private RawImage fullScreenRawImage;        // RawImage que cubrirá la pantalla

    [Header("Video")]
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private VideoClip videoClip;
    [SerializeField] private bool muteVideoAudio = true;

    [Header("Audio (música)")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioClip musicClip;
    [SerializeField] private bool loopMusic = false;

    [Header("Comportamiento")]
    [SerializeField] private bool loadSceneAfterMedia = false;
    [SerializeField] private string lossSceneName = "";
    [SerializeField] private bool autoShowOnZeroLives = true;

    private BarraCorazones barra;
    private RenderTexture renderTexture;
    private bool shown;
    private Coroutine waitForBarraCoroutine;

    private void Awake()
    {
        if (lossCanvas != null) lossCanvas.SetActive(false);

        if (videoPlayer != null)
        {
            videoPlayer.playOnAwake = false;
            videoPlayer.isLooping = false;
            videoPlayer.renderMode = VideoRenderMode.RenderTexture;
            if (videoClip != null)
            {
                videoPlayer.source = VideoSource.VideoClip;
                videoPlayer.clip = videoClip;
            }
        }

        if (musicSource != null) musicSource.playOnAwake = false;
    }

    private void OnEnable()
    {
        TrySubscribeToBarra();
    }

    private void OnDisable()
    {
        UnsubscribeFromBarra();
        if (waitForBarraCoroutine != null) StopCoroutine(waitForBarraCoroutine);
        waitForBarraCoroutine = null;
    }

    private void TrySubscribeToBarra()
    {
        barra = Object.FindFirstObjectByType<BarraCorazones>();
        if (barra != null)
        {
            barra.OnLivesChanged += OnLivesChanged;
            Debug.Log("LossCanvasVideoPlayer: suscrito a BarraCorazones.");
        }
        else
        {
            if (waitForBarraCoroutine == null)
                waitForBarraCoroutine = StartCoroutine(WaitForBarra());
        }
    }

    private IEnumerator WaitForBarra()
    {
        float timeout = 5f;
        float t = 0f;
        while (t < timeout)
        {
            barra = Object.FindFirstObjectByType<BarraCorazones>();
            if (barra != null)
            {
                barra.OnLivesChanged += OnLivesChanged;
                Debug.Log("LossCanvasVideoPlayer: BarraCorazones encontrada y suscrita.");
                waitForBarraCoroutine = null;
                yield break;
            }
            t += 0.25f;
            yield return new WaitForSeconds(0.25f);
        }
        Debug.LogWarning("LossCanvasVideoPlayer: no encontró BarraCorazones en la escena (timeout).");
        waitForBarraCoroutine = null;
    }

    private void UnsubscribeFromBarra()
    {
        if (barra != null)
        {
            barra.OnLivesChanged -= OnLivesChanged;
            barra = null;
        }
    }

    private void OnLivesChanged(int lives, int max)
    {
        Debug.Log($"LossCanvasVideoPlayer: Vidas restantes: {lives}/{max}");
        if (!autoShowOnZeroLives) return;
        if (shown) return;
        if (lives <= 0)
            StartCoroutine(PlayLossMediaAndMaybeLoadScene());
    }

    public void TriggerLossImmediate(bool loadSceneImmediately = false)
    {
        Debug.Log("LossCanvasVideoPlayer: TriggerLossImmediate llamado.");
        if (loadSceneImmediately && loadSceneAfterMedia && !string.IsNullOrEmpty(lossSceneName))
        {
            StartCoroutine(PlayLossMediaAndLoadSceneImmediately());
            return;
        }
        StartCoroutine(PlayLossMediaAndMaybeLoadScene());
    }

    private IEnumerator PlayLossMediaAndMaybeLoadScene()
    {
        shown = true;
        if (lossCanvas != null) lossCanvas.SetActive(true);

        if (videoPlayer != null)
        {
            if (videoPlayer.targetTexture == null)
            {
                renderTexture = new RenderTexture(Screen.width, Screen.height, 0);
                videoPlayer.targetTexture = renderTexture;
            }
            else
            {
                renderTexture = videoPlayer.targetTexture;
            }

            if (fullScreenRawImage != null)
                fullScreenRawImage.texture = renderTexture;

            if (muteVideoAudio)
            {
                try { videoPlayer.SetDirectAudioMute(0, true); } catch { }
            }

            if (!videoPlayer.isPrepared)
            {
                videoPlayer.Prepare();
                while (!videoPlayer.isPrepared) yield return null;
            }

            videoPlayer.Play();
        }

        if (musicSource != null && musicClip != null)
        {
            musicSource.clip = musicClip;
            musicSource.loop = loopMusic;
            musicSource.Play();
        }

        // Esperar a que termine el vídeo (si hay) o la música; usar tiempo real para evitar problemas con timeScale
        float wait = 0f;
        if (videoPlayer != null && videoPlayer.clip != null)
            wait = (float)videoPlayer.clip.length;
        else if (musicSource != null && musicSource.clip != null)
            wait = musicSource.clip.length;
        else
            wait = 2f;

        Debug.Log($"LossCanvasVideoPlayer: reproduciendo media. Esperando {wait} segundos (reloj real).");
        yield return new WaitForSecondsRealtime(wait);

        if (loadSceneAfterMedia && !string.IsNullOrEmpty(lossSceneName))
        {
            Debug.Log($"LossCanvasVideoPlayer: cargando escena '{lossSceneName}' tras media.");
            SceneManager.LoadScene(lossSceneName);
        }
    }

    private IEnumerator PlayLossMediaAndLoadSceneImmediately()
    {
        // reproduce media y carga escena inmediatamente (sin esperar)
        shown = true;
        if (lossCanvas != null) lossCanvas.SetActive(true);

        if (videoPlayer != null)
        {
            if (videoPlayer.targetTexture == null)
            {
                renderTexture = new RenderTexture(Screen.width, Screen.height, 0);
                videoPlayer.targetTexture = renderTexture;
            }
            if (fullScreenRawImage != null)
                fullScreenRawImage.texture = videoPlayer.targetTexture;
            if (muteVideoAudio)
            {
                try { videoPlayer.SetDirectAudioMute(0, true); } catch { }
            }
            if (!videoPlayer.isPrepared)
            {
                videoPlayer.Prepare();
                while (!videoPlayer.isPrepared) yield return null;
            }
            videoPlayer.Play();
        }

        if (musicSource != null && musicClip != null)
        {
            musicSource.clip = musicClip;
            musicSource.loop = loopMusic;
            musicSource.Play();
        }

        if (!string.IsNullOrEmpty(lossSceneName))
        {
            Debug.Log($"LossCanvasVideoPlayer: cargando escena '{lossSceneName}' inmediatamente.");
            SceneManager.LoadScene(lossSceneName);
        }
        yield break;
    }
}