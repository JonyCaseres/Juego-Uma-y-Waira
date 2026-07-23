using UnityEngine;
using UnityEngine.UI;

public class GifPlayer : MonoBehaviour
{
    [SerializeField] private Sprite[] frames;
    [SerializeField] private float fps = 12f;
    [SerializeField] private bool playOnStart = false;
    [SerializeField] private bool useImage = true;

    private Image uiImage;
    private SpriteRenderer spriteRenderer;
    private int index;
    private float timer;
    private bool playing;

    private void Awake()
    {
        uiImage = GetComponent<Image>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        playing = playOnStart;
        if (playOnStart) PlayFromStart();
    }

    private void Update()
    {
        if (!playing || frames == null || frames.Length == 0) return;
        timer += Time.deltaTime;
        float interval = 1f / Mathf.Max(0.0001f, fps);
        if (timer >= interval)
        {
            timer -= interval;
            index = (index + 1) % frames.Length;
            if (useImage && uiImage != null) uiImage.sprite = frames[index];
            else if (!useImage && spriteRenderer != null) spriteRenderer.sprite = frames[index];
        }
    }

    public void Play()
    {
        playing = true;
    }

    public void Stop()
    {
        playing = false;
    }

    public void Toggle()
    {
        playing = !playing;
    }

    public void PlayFromStart()
    {
        index = 0;
        timer = 0f;
        playing = true;
        if (frames != null && frames.Length > 0)
        {
            if (useImage && uiImage != null) uiImage.sprite = frames[index];
            else if (!useImage && spriteRenderer != null) spriteRenderer.sprite = frames[index];
        }
    }
}