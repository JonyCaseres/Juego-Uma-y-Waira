using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoPlayerController : MonoBehaviour
{
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private RawImage rawImage;
    [SerializeField] private RenderTexture renderTexture;
    [SerializeField] private bool playOnStart = true;
    [SerializeField] private bool loop = true;

    private void Awake()
    {
        if (videoPlayer == null) videoPlayer = GetComponent<VideoPlayer>();
        if (rawImage == null) rawImage = GetComponent<RawImage>();
        if (videoPlayer == null) return;
        videoPlayer.isLooping = loop;
        if (renderTexture != null) videoPlayer.targetTexture = renderTexture;
        if (rawImage != null)
            rawImage.texture = videoPlayer.targetTexture;
    }

    private void Start()
    {
        if (playOnStart && videoPlayer != null) videoPlayer.Play();
    }

    public void Play() { if (videoPlayer != null) videoPlayer.Play(); }
    public void Pause() { if (videoPlayer != null) videoPlayer.Pause(); }
    public void Stop() { if (videoPlayer != null) videoPlayer.Stop(); }
    public void Toggle() { if (videoPlayer == null) return; if (videoPlayer.isPlaying) videoPlayer.Pause(); else videoPlayer.Play(); }

    public void SetClip(VideoClip clip)
    {
        if (videoPlayer == null) return;
        videoPlayer.source = VideoSource.VideoClip;
        videoPlayer.clip = clip;
        videoPlayer.Prepare();
    }

    public void SetURL(string url)
    {
        if (videoPlayer == null) return;
        videoPlayer.source = VideoSource.Url;
        videoPlayer.url = url;
        videoPlayer.Prepare();
    }
}