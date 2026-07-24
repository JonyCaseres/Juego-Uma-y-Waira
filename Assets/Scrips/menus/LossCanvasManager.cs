using System.Collections;
using UnityEngine;

public class LossCanvasManager : MonoBehaviour
{
    [SerializeField] private GameObject lossCanvas;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip lossAudio;
    [SerializeField] private bool pauseGame = true;
    [SerializeField] private float delayBeforeShow = 0f;

    private BarraCorazones barra;
    private bool shown;

    private void Awake()
    {
        if (lossCanvas != null) lossCanvas.SetActive(false);
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
        if (shown) return;
        if (lives <= 0)
            StartCoroutine(ShowLossCoroutine());
    }

    private IEnumerator ShowLossCoroutine()
    {
        if (delayBeforeShow > 0f) yield return new WaitForSeconds(delayBeforeShow);

        if (lossCanvas != null) lossCanvas.SetActive(true);

        if (audioSource != null && lossAudio != null)
        {
            audioSource.clip = lossAudio;
            audioSource.loop = false;
            audioSource.Play();
        }

        if (pauseGame) Time.timeScale = 0f;

        shown = true;
    }

    public void HideLossUI()
    {
        if (lossCanvas != null) lossCanvas.SetActive(false);
        if (pauseGame) Time.timeScale = 1f;
        shown = false;
    }
}