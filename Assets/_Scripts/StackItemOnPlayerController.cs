using UnityEngine;
using UnityEngine.Assertions;

public class StackItemOnPlayerController : MonoBehaviour
{
    [SerializeField] public int StackValue = 1;
    [SerializeField] public SoundManager mySoundManager;

    private void Awake()
    {
        if (mySoundManager == null)
            mySoundManager = FindObjectOfType<SoundManager>();

        Assert.IsNotNull(mySoundManager);
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.GetComponent<IStackObject<int>>() != null)
        {
            mySoundManager.PlaySound(SoundManager.Sound.Woosh);
            col.gameObject.GetComponent<IStackObject<int>>().StackObject(1, col.gameObject);
        }
    }
}
