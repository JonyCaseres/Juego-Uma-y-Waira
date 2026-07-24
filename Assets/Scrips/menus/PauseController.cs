using System;

public static class PauseController
{
    public static bool isGamePaused { get; private set; }

    public static event Action<bool> OnPauseChanged;

    public static void SetPaused(bool paused)
    {
        if (isGamePaused == paused) return;
        isGamePaused = paused;
        OnPauseChanged?.Invoke(isGamePaused);
    }

    public static void Toggle()
    {
        SetPaused(!isGamePaused);
    }
}
