using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class LossSceneManager : MonoBehaviour
{
    [SerializeField] private GameObject lossOverlay;
    [SerializeField] private RawImage fullScreenRawImage;
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip lossAudio;
    [SerializeField] private string lossSceneName;
    [SerializeField] private float extraDelay = 0.25f;

    private BarraCorazones barra;

    private void Awake()
    {
        if (lossOverlay != null) lossOverlay.SetActive(false);
        if (videoPlayer != null)
        {
            videoPlayer.playOnAwake = false;
            videoPlayer.isLooping = false;
            videoPlayer.renderMode = VideoRenderMode.RenderTexture;
            videoPlayer.SetDirectAudioMute(0, true);
        }
        if (audioSource != null) audioSource.playOnAwake = false;
    }

    private void Start()
    {
        barra = FindObjectOfType<BarraCorazones>();
        if (barra != null) barra.OnLivesChanged += OnLivesChanged;
    }

    private void OnDestroy()
    {
        if (barra != null) barra.OnLivesChanged -= OnLivesChanged;
    }

    private void OnLivesChanged(int lives, int max)
    {
        if (lives <= 0)
            StartCoroutine(PlayLossSequence());
    }

    private IEnumerator PlayLossSequence()
    {
        if (lossOverlay != null) lossOverlay.SetActive(true);

        if (videoPlayer != null)
        {
            if (videoPlayer.targetTexture == null)
            {
                var rt = new RenderTexture(Screen.width, Screen.height, 0);
                videoPlayer.targetTexture = rt;
            }

            if (fullScreenRawImage != null)
                fullScreenRawImage.texture = videoPlayer.targetTexture;

            if (!videoPlayer.isPrepared)
            {
                videoPlayer.Prepare();
                while (!videoPlayer.isPrepared) yield return null;
            }

            videoPlayer.Play();
        }

        if (audioSource != null && lossAudio != null)
        {
            audioSource.clip = lossAudio;
            audioSource.loop = false;
            audioSource.Play();
        }

        float wait = 0f;
        if (videoPlayer != null && videoPlayer.clip != null)
            wait = (float)videoPlayer.clip.length;
        else if (audioSource != null && audioSource.clip != null)
            wait = audioSource.clip.length;
        else
            wait = 2f;

        yield return new WaitForSeconds(wait + extraDelay);

        if (!string.IsNullOrEmpty(lossSceneName))
            SceneManager.LoadScene(lossSceneName);
    }
}