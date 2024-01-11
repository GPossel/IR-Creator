using UnityEngine;

public static class PauzeMenuTimer
{
    public static bool isPauzed = false;

    public static void SetTimeToNormal()
    {
        Time.timeScale = 1f;
    }

    public static void SetTimeToStop()
    {
        Time.timeScale = 0f;
    }
}
