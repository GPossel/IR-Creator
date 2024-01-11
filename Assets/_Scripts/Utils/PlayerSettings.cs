using UnityEditor;
using UnityEngine;

public static class PlayerSettings
{
    public static readonly string isBackgroundMusicOn = "isOn_BackgroundMusic";
    public static readonly string backgroundMusicVolume = "volume_BackgroundMusic";
    public static readonly string isSFOn = "isOn_SF";
    public static readonly string sfVolume = "volume_SF";

    public static readonly string checkPointZValue = "checkPointZValue";
    public static readonly string numberOfRuns = "numberOfRuns";
    public static readonly string highScore = "highScore";

    public static bool GetisOnBackgroundMusic() => PlayerPrefs.GetInt(isBackgroundMusicOn) == 0;
    public static float GetMyBackgroundMusicVolume() => PlayerPrefs.GetFloat(backgroundMusicVolume);
    public static bool GetisOnSF() => PlayerPrefs.GetInt(isSFOn) == 0;
    public static float GetMySoundEffectsVolume() => PlayerPrefs.GetFloat(sfVolume);
    public static int GetHighScore() => PlayerPrefs.GetInt(highScore);
    public static void SetisOnBackgroundMusic(bool isOn)
    {
        var newValue = (isOn)
                             ? 0
                             : 1;

        PlayerPrefs.SetInt(isBackgroundMusicOn, newValue);
    }

    public static void SetMyBackgroundMusicVolume(float value) => PlayerPrefs.SetFloat(backgroundMusicVolume, value);
    public static void SetisOnSF(bool setSFOn)
    {
        var newValue = (setSFOn)
                                ? 0
                                : 1;

        PlayerPrefs.SetInt(isSFOn, newValue);
    }
    public static void SetMySoundEffectsVolume(float value) => PlayerPrefs.SetFloat(sfVolume, value);
    public static void SetNumbOfRuns(int value) => PlayerPrefs.SetInt(numberOfRuns, value);
    public static void SetHighScore(int value) => PlayerPrefs.SetInt(highScore, value);
    public static void UpdateTotalOfRuns()
    { 
        int runsTot = PlayerPrefs.GetInt(numberOfRuns);
        int value = 1 + runsTot;
        PlayerPrefs.SetInt(numberOfRuns, value);
    }
}

#if (UNITY_EDITOR)
[ExecuteInEditMode]
public class PlayerPrefsRemover : EditorWindow
{
    [MenuItem("Tools/Player Prefs Remover")]
    public static void DeletePlayer()
    {
        PlayerPrefs.DeleteAll();
        Debug.Log("Delete Everything..");
    }
}
// ref: https://support.unity.com/hc/en-us/articles/208456906-Excluding-Scripts-and-Assets-from-builds#:~:text=To%20do%20that%2C%20create%20a,the%20start%20of%20your%20code.
#endif
