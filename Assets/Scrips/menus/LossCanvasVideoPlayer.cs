using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class LossCanvasVideoPlayer : MonoBehaviour
{
    [SerializeField] private GameObject lossCanvas;
    [SerializeField] private RawImage fullScreenRawImage;
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private VideoClip videoClip;
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioClip musicClip;
    [SerializeField] private bool loopMusic = false;
    [SerializeField] private bool muteVideoAudio = true;

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
        // Usar FindFirstObjectByType para evitar el warning deprecado
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
        Debug.Log($"Vidas restantes: {lives}/{max}");
        if (shown) return;
        if (lives <= 0)
            StartCoroutine(PlayLossMedia());
    }

    private IEnumerator PlayLossMedia()
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

        yield break;
    }
}