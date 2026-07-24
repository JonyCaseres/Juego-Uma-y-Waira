using UnityEngine;
using System;

public class PauseController : MonoBehaviour
{
    // Variable estática accesible desde cualquier script, pero solo modificable aquí
    public static bool isGamePaused { get; private set; } = false;

    public static event Action<bool> OnPauseChanged;

    // Método para activar o desactivar la pausa
    public static void SetPause(bool pause)
    {
        SetPaused(pause);
    }

    public static void SetPaused(bool pause)
    {
        if (isGamePaused == pause)
            return;

        isGamePaused = pause;
        OnPauseChanged?.Invoke(isGamePaused);
    }

    public static void Toggle()
    {
        SetPaused(!isGamePaused);
    }
}
