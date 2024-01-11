using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;


public class SoundAssetReferenceManager : SingletonDontDestroyOnLoad<SoundAssetReferenceManager>
{
    [Header("Sound References")]
    public SoundAudioClip[] soundAudioClipArray;

    [System.Serializable]
    public class SoundAudioClip
    {
        public SoundManager.Sound sound;
        public AudioClip audioClip;
    }

    private void Start()
    {
        Assert.IsNotNull(Instance);
    }

    public AudioClip GetMatchingAudioClip(SoundManager.Sound sound) => soundAudioClipArray.Where(x => x.sound == sound)
                                                                                          .Select(x => x.audioClip)
                                                                                          .FirstOrDefault();
}