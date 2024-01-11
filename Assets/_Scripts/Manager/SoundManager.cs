using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SoundManager : SingletonDontDestroyOnLoad<SoundManager>
{
    private SoundAssetReferenceManager mySoundAssestRefgerenceManager;

    [Header("Background Music")]
    public GameObject _currentBackgroundClip = null;

    [Header("Sound Effects")]
    public List<GameObject> _currentSFX = new List<GameObject>();

    public bool backGroundMusicIsOn;
    public float backGroundMusicVolume;
    public bool sfIsOn;
    public float sfVolume;

    public enum Sound
    {
        MusicInRun,
        MusicPlayOptionsQuitScreen,
        MusicCharacterSelectionScreen,
        PickUpCoin,
        ObstacleCrash,
        AlarmTreasure,
        CompleteLevel,
        FailLevel,
        FallOffField,
        Jump,
        Woosh,
        LoadingPowerUpSF
    }

    public enum Speed
    {
        Slow, // make the interval bigger
        Normal,
        Fast // make the interval smaller
    }

    private static Dictionary<Sound, float> soundTimerDictionary;
    private static Dictionary<Sound, SoundConfigs> soundsConfig;

    public class SoundConfigs
    {
        public float volume = 1f;
        public float playerMoveTimerMax { get; set; }
        public Speed adjustSpeed { get { return adjustSpeed; } set { value = Speed.Normal; } }
    }

    private void Start()
    {
        if (mySoundAssestRefgerenceManager == null)
            mySoundAssestRefgerenceManager = FindObjectOfType<SoundAssetReferenceManager>();

        soundTimerDictionary = new Dictionary<Sound, float>();
        soundTimerDictionary[Sound.PickUpCoin] = 0;
        soundTimerDictionary[Sound.ObstacleCrash] = 0;
        soundTimerDictionary[Sound.AlarmTreasure] = 0;
        soundTimerDictionary[Sound.CompleteLevel] = 0;
        soundTimerDictionary[Sound.FailLevel] = 0;
        soundTimerDictionary[Sound.FallOffField] = 0;
        soundTimerDictionary[Sound.Jump] = 0;
        soundTimerDictionary[Sound.Woosh] = 0;
        soundTimerDictionary[Sound.LoadingPowerUpSF] = 0;

        // basic values? maybe throw out?
        soundsConfig = new Dictionary<Sound, SoundConfigs>();
        soundsConfig.Add(Sound.PickUpCoin, new SoundConfigs() { volume = 0.08f, playerMoveTimerMax = 0.6f });
        soundsConfig.Add(Sound.ObstacleCrash, new SoundConfigs() { volume = 0.08f, playerMoveTimerMax = 1.5f });
        soundsConfig.Add(Sound.AlarmTreasure, new SoundConfigs() { volume = 0.08f, playerMoveTimerMax = 0.8f });
        soundsConfig.Add(Sound.CompleteLevel, new SoundConfigs() { volume = 0.08f, playerMoveTimerMax = 0.8f });
        soundsConfig.Add(Sound.FailLevel, new SoundConfigs() { volume = 0.08f, playerMoveTimerMax = 0.8f });
        soundsConfig.Add(Sound.FallOffField, new SoundConfigs() { volume = 0.08f, playerMoveTimerMax = 0.8f });
        soundsConfig.Add(Sound.Jump, new SoundConfigs() { volume = 0.08f, playerMoveTimerMax = 0.8f });
        soundsConfig.Add(Sound.Woosh, new SoundConfigs() { volume = 0.08f, playerMoveTimerMax = 0.8f });
        soundsConfig.Add(Sound.LoadingPowerUpSF, new SoundConfigs() { volume = 0.08f, playerMoveTimerMax = 0.8f });

        // background music
        soundsConfig.Add(Sound.MusicInRun, new SoundConfigs() { volume = 0.08f, playerMoveTimerMax = 0.8f });
        soundsConfig.Add(Sound.MusicPlayOptionsQuitScreen, new SoundConfigs() { volume = 0.08f, playerMoveTimerMax = 0.8f });
        soundsConfig.Add(Sound.MusicCharacterSelectionScreen, new SoundConfigs() { volume = 0.08f, playerMoveTimerMax = 0.8f });

        PlayBackgroundSound();
    }

    private void Update()
    {
        StartCoroutine(CheckUsedSoundEffects());
        CheckOnPrefsChanges();
    }

    private void CheckOnPrefsChanges()
    {
        backGroundMusicIsOn = PlayerSettings.GetisOnBackgroundMusic();
        backGroundMusicVolume = PlayerSettings.GetMyBackgroundMusicVolume();
        sfVolume = PlayerSettings.GetMySoundEffectsVolume();
        sfIsOn = PlayerSettings.GetisOnSF();

        if (_currentBackgroundClip != null && _currentBackgroundClip.GetComponent<AudioSource>() != null)
        {
            _currentBackgroundClip.GetComponent<AudioSource>().volume = backGroundMusicVolume;
            _currentBackgroundClip.GetComponent<AudioSource>().enabled = backGroundMusicIsOn;
        }
    }

    public IEnumerator CheckUsedSoundEffects()
    {
        for (int i = 0; i < _currentSFX.Count; i++)
        {
            var soundFX = _currentSFX[i];
            if (soundFX != null) // weird but prevents the playsound from spaming
            {
                yield return new WaitForSeconds(3);
                var audioSource = soundFX.GetComponent<AudioSource>();

                if (audioSource != null)
                {
                    if (audioSource?.clip != null)
                    {
                        _currentSFX.Remove(soundFX);
                        Destroy(soundFX);
                    }
                }
            }
        }
    }


    public void PlaySound(Sound sound)
    {
        try
        {
            if (CanPlaySound(sound) && sfIsOn)
            {
                GameObject oneSound = new GameObject();
                oneSound.transform.parent = this.transform;
                AudioSource audioSource = oneSound.AddComponent<AudioSource>();
                audioSource.volume = sfVolume;
                oneSound.name = $"{sound}_v{audioSource.volume}";
                audioSource.PlayOneShot(GetAudioClip(sound));
                _currentSFX.Add(oneSound);
            }
        }
        catch (Exception e)
        {
            Debug.LogWarning(e + ": Couldnt play sound");
        }
    }

    public void PlaySound(Sound sound, float volume)
    {
        try
        {
            if (CanPlaySound(sound) && sfIsOn)
            {
                GameObject oneSound = new GameObject();
                oneSound.transform.parent = this.transform;
                AudioSource audioSource = oneSound.AddComponent<AudioSource>();
                audioSource.volume = volume;
                oneSound.name = $"{sound}_v{audioSource.volume}";
                audioSource.PlayOneShot(GetAudioClip(sound));
                _currentSFX.Add(oneSound);
            }
        }
        catch (Exception e)
        {
            Debug.LogWarning(e + ": Could not play sound");
        }
    }

    public void PlaySound(Sound sound, Speed speed)
    {
        try
        {
            if (CanPlaySound(sound, speed) && sfIsOn)
            {
                GameObject oneSound = new GameObject();
                oneSound.transform.parent = this.transform;
                AudioSource audioSource = oneSound.AddComponent<AudioSource>();
                oneSound.name = $"{sound}_v{audioSource.volume}";
                audioSource.volume = sfVolume;
                audioSource.PlayOneShot(GetAudioClip(sound));
                _currentSFX.Add(oneSound);
            }
        }
        catch (Exception e)
        {
            Debug.LogWarning(e + ": Could not play sound");
        }
    }

    public void StopSound(SoundManager.Sound sound)
    {
        StopOneSound(sound);
    }

    public void PlayBackgroundSound(bool shouldContinuePrevious = false)
    {
        PlayBackgroundSound(SoundManager.Sound.MusicInRun, shouldContinuePrevious);
    }

    public void PlayBackgroundSoundCharacterSelectionUI(bool shouldContinuePrevious = false)
    {
        PlayBackgroundSound(SoundManager.Sound.MusicCharacterSelectionScreen, shouldContinuePrevious);
    }
    public void PlayBackgroundSoundPlayOptionsQuitUI(bool shouldContinuePrevious = false)
    {
        PlayBackgroundSound(SoundManager.Sound.MusicPlayOptionsQuitScreen, shouldContinuePrevious);
    }

    private void PlayBackgroundSound(Sound sound, bool shouldContinuePrevious = false)
    {
        if (backGroundMusicIsOn)
        {
            GameObject myBackgroundMusic = _currentBackgroundClip != null ? _currentBackgroundClip : new GameObject();
            myBackgroundMusic.transform.parent = this.transform;

            var audioSource = myBackgroundMusic.GetComponent<AudioSource>();

            if (audioSource == null)
                audioSource = myBackgroundMusic.AddComponent<AudioSource>();


            audioSource.volume = backGroundMusicVolume;
            myBackgroundMusic.name = $"backgroundMusic_v{audioSource.volume}";
            audioSource.loop = true;

            var clipToPlay = shouldContinuePrevious && _currentBackgroundClip.GetComponent<AudioClip>() != null
                                                            ? _currentBackgroundClip.GetComponent<AudioClip>()
                                                            : GetAudioClip(sound);

            audioSource.clip = clipToPlay;
            _currentBackgroundClip = myBackgroundMusic;
            audioSource.Play();
        }
    }

    public void StopBackgroundMusic()
    {
        StopOneSound(Sound.MusicInRun);
        StopOneSound(Sound.MusicPlayOptionsQuitScreen);
        StopOneSound(Sound.MusicCharacterSelectionScreen);
    }

    private void StopOneSound(Sound sound)
    {
        // I.e. get audiosource for playerMove, stop immediate when jumping
        var sources = this.gameObject.GetComponents<AudioSource>();

        var soundClipNameToFind = mySoundAssestRefgerenceManager.soundAudioClipArray
                                                                                    .Where(x => x.sound == sound)
                                                                                    .Select(x => x.audioClip.name)
                                                                                    .FirstOrDefault();

        if (soundClipNameToFind == null) return;

        foreach (var source in sources)
        {
            if (source.clip != null) // check for null, happens when interval is small between clips that get destroyed 
            {
                if (source.clip.name == soundClipNameToFind)
                {
                    Destroy(source);
                }
            }
        }
    }

    private AudioClip GetAudioClip(Sound sound)
    {
        foreach (SoundAssetReferenceManager.SoundAudioClip soundAudioClip in mySoundAssestRefgerenceManager.soundAudioClipArray)
        {
            if (soundAudioClip.sound == sound)
            {
                return soundAudioClip.audioClip;
            }
        }

        Debug.LogError("Sound " + sound + " not found!");
        return null;
    }

    private bool CanPlaySound(Sound sound, Speed speed = Speed.Normal) // default
    {
        switch (sound)
        {
            // use this to monitor how frequently we play the sound
            case Sound.AlarmTreasure:
                if (soundTimerDictionary.ContainsKey(sound))
                {
                    float lastTimePlayed = soundTimerDictionary[sound];
                    float playerMoveTimerMax = soundsConfig[sound].playerMoveTimerMax;

                    if (speed == Speed.Slow)
                        playerMoveTimerMax *= 1.5f;

                    if (speed == Speed.Fast)
                        playerMoveTimerMax *= 0.8f;


                    if (lastTimePlayed + playerMoveTimerMax < Time.time)
                    {
                        soundTimerDictionary[sound] = Time.time;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                return true;
            // use this to monitor how frequently we play the sound
            case Sound.PickUpCoin:
                if (soundTimerDictionary.ContainsKey(sound))
                {
                    float lastTimePlayed = soundTimerDictionary[sound];
                    float playerMoveTimerMax = soundsConfig[sound].playerMoveTimerMax;

                    if (speed == Speed.Slow)
                        playerMoveTimerMax *= 1.5f;

                    if (speed == Speed.Fast)
                        playerMoveTimerMax *= 0.8f;


                    if (lastTimePlayed + playerMoveTimerMax < Time.time)
                    {
                        soundTimerDictionary[sound] = Time.time;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                return true;
            default:
                return true;
        }
    }
}