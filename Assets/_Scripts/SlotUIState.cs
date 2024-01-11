using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class SlotUIState : MonoBehaviour
{
    [SerializeField] public Animation treasureHintHelpAnimation;
    bool isFound = false;
    bool isStopped = false;

    public UIDistanceAlarmTypes currentAlarmType;

    public void UpdateItemToHot() => currentAlarmType = UIDistanceAlarmTypes.Hot;
    public void UpdateItemToWarm() => currentAlarmType = UIDistanceAlarmTypes.Warm;
    public void UpdateItemToCold() => currentAlarmType = UIDistanceAlarmTypes.Cold;
    public void UpdateToNone() => currentAlarmType = UIDistanceAlarmTypes.None;

    private void Awake()
    {}

    private void Start()
    {
        treasureHintHelpAnimation = GetComponent<Animation>();
        Assert.IsNotNull(treasureHintHelpAnimation);
        Assert.IsFalse(isFound);
    }
    // ref: https://answers.unity.com/questions/1812575/wait-for-animation-to-finish-4.html
    // ref : https://answers.unity.com/questions/584106/the-animation-state-could-not-be-played-because-it-1.html
    private void Update()
    {
        switch (currentAlarmType)
        {
            case UIDistanceAlarmTypes.Hot:
                PlaySlotAnimation("slot_hint_UI_hot");
                break;
            case UIDistanceAlarmTypes.Warm:
                PlaySlotAnimation("slot_hint_UI_warm");
                break;
            case UIDistanceAlarmTypes.Cold:
                PlaySlotAnimation("slot_hint_UI_cold");
                break;
            case UIDistanceAlarmTypes.None:
                // ref: https://www.youtube.com/watch?v=Rla7y6gCYqo
                if (treasureHintHelpAnimation.clip != null)
                    StartCoroutine(WaitOnAnimationToFinish(treasureHintHelpAnimation.clip.length));
                break;
            default:
                break;
        }

        if (isFound)
        {
            this.GetComponent<Image>().color -= new Color(0, 0, 0, 0.5f);
            this.transform.GetChild(0).GetComponent<Image>().color -= new Color(0, 0, 0, 0.5f);
        }

        return;
    }

    IEnumerator WaitOnAnimationToFinish(float _delay = 0)
    {
        yield return new WaitForSeconds(_delay);
        PlayStopAllAnimations();
    }

    public void UpdateToFound()
    {
        isFound = true;
        currentAlarmType = UIDistanceAlarmTypes.None;
    }

    public void StopAllAnimationsOfSlots()
    {
        isStopped = true;
    }

    // ref: https://forum.unity.com/threads/simple-ui-animation-fade-in-fade-out-c.439825/
    // ref: https://stackoverflow.com/questions/50446427/how-to-check-if-a-certain-animation-state-from-an-animator-is-running

    public void PlaySlotAnimation(string nameClip)
    {
        treasureHintHelpAnimation.Play(nameClip);
    }

    public void PlayStopAllAnimations()
    {
        // ref: https://forum.unity.com/threads/animations-freeze-on-animation-stop.376894/
        treasureHintHelpAnimation.Stop();
    }
}