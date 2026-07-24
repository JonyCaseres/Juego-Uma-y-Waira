using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicPauseListener : MonoBehaviour
{
    [Tooltip("AudioSource que se pausará/reanudará. Si está vacío se usará el AudioSource del mismo GameObject.")]
    public AudioSource musicSource;

    [Tooltip("Si está activado, usará Pause()/UnPause() en lugar de Stop()/Play().")]
    public bool useAudioPause = true;

    [Tooltip("Duración del fundido al pausar/reanudar (segundos). 0 = sin fundido.")]
    public float fadeDuration = 0.3f;

    private Coroutine fadeCoroutine;
    private float originalVolume;

    private void Awake()
    {
        if (musicSource == null)
            musicSource = GetComponent<AudioSource>();

        if (musicSource != null)
            originalVolume = musicSource.volume;
    }

    private void OnEnable()
    {
        PauseController.OnPauseChanged += OnPauseChanged;
    }

    private void OnDisable()
    {
        PauseController.OnPauseChanged -= OnPauseChanged;
    }

    private void OnPauseChanged(bool paused)
    {
        if (musicSource == null) return;

        // Si no hay fundido, aplicar inmediatamente
        if (fadeDuration <= 0f)
        {
            if (paused)
            {
                if (useAudioPause)
                    musicSource.Pause();
                else
                    musicSource.Stop();
            }
            else
            {
                if (useAudioPause)
                    musicSource.UnPause();
                else
                {
                    if (!musicSource.isPlaying)
                        musicSource.Play();
                }
            }
            return;
        }

        // Con fundido: usamos tiempo real (WaitForSecondsRealtime) para que funcione con Time.timeScale = 0
        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
        fadeCoroutine = StartCoroutine(FadeAndToggle(paused));
    }

    private IEnumerator FadeAndToggle(bool pause)
    {
        float startVol = musicSource.volume;
        float targetVol = pause ? 0f : originalVolume;
        float t = 0f;

        // Si vamos a reanudar y no está reproduciéndose, arrancar antes de subir volumen
        if (!pause && !musicSource.isPlaying)
        {
            if (useAudioPause)
                musicSource.UnPause();
            else
                musicSource.Play();

            // asegurar volumen inicial 0 para el fade in
            musicSource.volume = 0f;
            startVol = 0f;
        }

        while (t < fadeDuration)
        {
            t += Time.unscaledDeltaTime;
            musicSource.volume = Mathf.Lerp(startVol, targetVol, t / fadeDuration);
            yield return null;
        }

        musicSource.volume = targetVol;

        if (pause)
        {
            if (useAudioPause)
                musicSource.Pause();
            else
                musicSource.Stop();

            // restaurar volumen a valor original para la próxima reproducción
            musicSource.volume = originalVolume;
        }

        fadeCoroutine = null;
    }
}