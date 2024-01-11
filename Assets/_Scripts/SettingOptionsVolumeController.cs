using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class SettingOptionsVolumeController : MonoBehaviour
{
    [SerializeField] public Toggle muteBackgroundMusic;
    [SerializeField] public Slider myBackgroundMusicVolume;
    [SerializeField] public Toggle muteSFvolume;
    [SerializeField] public Slider mySFvolume;

    [SerializeField] public SoundManager mySoundManager;

    private void Awake()
    {
        if (mySoundManager == null)
            mySoundManager = FindObjectOfType<SoundManager>();

        Assert.IsNotNull(mySoundManager);
        Assert.IsNotNull(muteBackgroundMusic);
        Assert.IsNotNull(myBackgroundMusicVolume);
        Assert.IsNotNull(muteSFvolume);
        Assert.IsNotNull(mySFvolume);

        muteBackgroundMusic.isOn = PlayerSettings.GetisOnBackgroundMusic();
        myBackgroundMusicVolume.value = PlayerSettings.GetMyBackgroundMusicVolume();
        muteSFvolume.isOn = PlayerSettings.GetisOnSF();
        mySFvolume.value = PlayerSettings.GetMySoundEffectsVolume();
    }

    public void ChangeBackgroundMusicIsOnValue()
    {
        PlayerSettings.SetisOnBackgroundMusic(muteBackgroundMusic.isOn);
        UpdateMySettings();
    }

    public void ChangeBackgroundMusicValue()
    {
        PlayerSettings.SetMyBackgroundMusicVolume(myBackgroundMusicVolume.value);
        UpdateMySettings();
    }

    public void ChangeSFIsOnValue()
    {
        PlayerSettings.SetisOnSF(muteSFvolume.isOn);
        UpdateMySettings();
    }

    public void ChangeSFValue()
    {
        // we want an example for the user how it sounds, when it changes

        if (PlayerSettings.GetMySoundEffectsVolume() != mySFvolume.value)
        {
            PlayerSettings.SetMySoundEffectsVolume(mySFvolume.value);
            mySoundManager.PlaySound(SoundManager.Sound.PickUpCoin, mySFvolume.value);
            UpdateMySettings();
        }
    }

    private void UpdateMySettings()
    {
        muteBackgroundMusic.isOn = PlayerSettings.GetisOnBackgroundMusic();
        myBackgroundMusicVolume.value = PlayerSettings.GetMyBackgroundMusicVolume();
        muteSFvolume.isOn = PlayerSettings.GetisOnSF();
        mySFvolume.value = PlayerSettings.GetMySoundEffectsVolume();
    }
}
