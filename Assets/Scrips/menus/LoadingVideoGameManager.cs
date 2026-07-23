using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class LoadingVideoGameManager : MonoBehaviour
{
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private RawImage videoRawImage;
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private VideoClip videoClip;
    [SerializeField] private string nextSceneName;
    [SerializeField] private float loadAfterSeconds;
    [SerializeField] private float squareSize = 360f;
    [SerializeField] private Corner corner = Corner.BottomRight;
    [SerializeField] private Vector2 padding = new Vector2(10f, 10f);
    [SerializeField] private bool playOnStart = true;
    [SerializeField] private float scale = 1f;
    [SerializeField] private float minScale = 0.5f;
    [SerializeField] private float maxScale = 2f;

    public enum Corner { TopLeft, TopRight, BottomLeft, BottomRight }

    private RenderTexture renderTexture;
    private Coroutine playCoroutine;

    private void Awake()
    {
        if (videoPlayer == null) videoPlayer = GetComponent<VideoPlayer>();
        if (videoPlayer == null) return;
        videoPlayer.playOnAwake = false;
        videoPlayer.isLooping = true;
        videoPlayer.renderMode = VideoRenderMode.RenderTexture;
        videoPlayer.SetDirectAudioMute(0, true);
        if (videoClip != null)
        {
            videoPlayer.source = VideoSource.VideoClip;
            videoPlayer.clip = videoClip;
        }
    }

    private void Start()
    {
        if (videoRawImage != null)
        {
            var rt = videoRawImage.rectTransform;
            float maxSize = Mathf.Min(Screen.width * 0.6f, Screen.height * 0.6f);
            float baseSize = Mathf.Clamp(squareSize, 64f, maxSize);
            rt.sizeDelta = new Vector2(baseSize * Mathf.Clamp(scale, minScale, maxScale), baseSize * Mathf.Clamp(scale, minScale, maxScale));
            SetAnchorToCorner(rt, corner, padding);
        }
        if (playOnStart) StartLoading(nextSceneName, loadAfterSeconds);
    }

    private void SetAnchorToCorner(RectTransform rt, Corner c, Vector2 pad)
    {
        switch (c)
        {
            case Corner.TopLeft:
                rt.anchorMin = rt.anchorMax = new Vector2(0f, 1f);
                rt.pivot = new Vector2(0f, 1f);
                rt.anchoredPosition = new Vector2(pad.x, -pad.y);
                break;
            case Corner.TopRight:
                rt.anchorMin = rt.anchorMax = new Vector2(1f, 1f);
                rt.pivot = new Vector2(1f, 1f);
                rt.anchoredPosition = new Vector2(-pad.x, -pad.y);
                break;
            case Corner.BottomLeft:
                rt.anchorMin = rt.anchorMax = new Vector2(0f, 0f);
                rt.pivot = new Vector2(0f, 0f);
                rt.anchoredPosition = new Vector2(pad.x, pad.y);
                break;
            case Corner.BottomRight:
                rt.anchorMin = rt.anchorMax = new Vector2(1f, 0f);
                rt.pivot = new Vector2(1f, 0f);
                rt.anchoredPosition = new Vector2(-pad.x, pad.y);
                break;
        }
    }

    public void StartLoading(string sceneName, float afterSeconds = 0f)
    {
        if (playCoroutine != null) StopCoroutine(playCoroutine);
        playCoroutine = StartCoroutine(PlayAndLoadCoroutine(sceneName, afterSeconds));
    }

    private IEnumerator PlayAndLoadCoroutine(string sceneName, float afterSeconds)
    {
        if (loadingScreen != null) loadingScreen.SetActive(true);
        if (videoPlayer == null)
        {
            if (afterSeconds > 0f) yield return new WaitForSeconds(afterSeconds);
            if (!string.IsNullOrEmpty(sceneName)) LoadScene(sceneName);
            yield break;
        }

        if (videoClip != null)
        {
            videoPlayer.clip = videoClip;
            videoPlayer.source = VideoSource.VideoClip;
        }

        if (!videoPlayer.isPrepared)
        {
            videoPlayer.Prepare();
            while (!videoPlayer.isPrepared) yield return null;
        }

        Texture tex = videoPlayer.texture;
        int w = tex != null && tex.width > 0 ? tex.width : Mathf.Max(64, (int)squareSize);
        int h = tex != null && tex.height > 0 ? tex.height : Mathf.Max(64, (int)squareSize);
        if (renderTexture == null || renderTexture.width != w || renderTexture.height != h)
        {
            if (renderTexture != null) renderTexture.Release();
            renderTexture = new RenderTexture(w, h, 0);
        }

        videoPlayer.targetTexture = renderTexture;

        if (videoRawImage != null)
        {
            videoRawImage.texture = renderTexture;
            UpdateRawImageSize();
        }

        videoPlayer.Play();

        float wait = afterSeconds;
        if (wait <= 0f)
        {
            if (videoPlayer.clip != null) wait = (float)videoPlayer.clip.length;
            else
            {
                double len = videoPlayer.length;
                wait = len > 0.0 ? (float)len : 0f;
            }
        }

        if (wait > 0f) yield return new WaitForSeconds(wait);
        else yield return null;

        if (!string.IsNullOrEmpty(sceneName)) LoadScene(sceneName);
    }

    private void UpdateRawImageSize()
    {
        if (videoRawImage == null) return;
        Texture tex = videoPlayer != null ? videoPlayer.texture : null;
        float maxSize = Mathf.Min(Screen.width * 0.6f, Screen.height * 0.6f);
        float baseSize = Mathf.Clamp(squareSize, 64f, maxSize);
        int texW = tex != null && tex.width > 0 ? tex.width : 1;
        int texH = tex != null && tex.height > 0 ? tex.height : 1;
        float aspect = (float)texW / texH;
        float width, height;
        if (aspect >= 1f)
        {
            width = baseSize;
            height = baseSize / aspect;
        }
        else
        {
            width = baseSize * aspect;
            height = baseSize;
        }
        float s = Mathf.Clamp(scale, minScale, maxScale);
        videoRawImage.rectTransform.sizeDelta = new Vector2(width * s, height * s);
    }

    public void SetScale(float newScale)
    {
        scale = Mathf.Clamp(newScale, minScale, maxScale);
        UpdateRawImageSize();
    }

    public void ChangeScaleDelta(float delta)
    {
        SetScale(scale + delta);
    }

    private void LoadScene(string sceneName)
    {
        if (MenuSceneManager.Instance != null) MenuSceneManager.Instance.Load(sceneName);
        else SceneManager.LoadScene(sceneName);
    }
}