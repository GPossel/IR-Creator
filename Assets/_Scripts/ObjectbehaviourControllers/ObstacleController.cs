using UnityEngine;
using UnityEngine.Assertions;

public class ObstacleController : MonoBehaviour
{
    // TODO: can we configure this generic thing into saying ' when we collide with X, () {}
    //public enum CollectionItemType
    //{
    //    BasicCoin,
    //    RedCoin,
    //    SpecialStar,
    //    Enemy,
    //    Pitfall,
    //    SpeedBoost
    //}
    //
    // maybe weird? To put behaviour in a switch statement, what would it look like if you try that one
    // like: if type or coin, inventory += update ../ stufff
    // like if type of speed boost => playermovementscript start coroutine

    [SerializeField] public SoundManager mySoundManager;

    private void Awake()
    {
        if (mySoundManager == null)
            mySoundManager = FindObjectOfType<SoundManager>();

        Assert.IsNotNull(mySoundManager);
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.GetComponent<IDamagable<int>>() != null)
        {
            mySoundManager.PlaySound(SoundManager.Sound.ObstacleCrash);
            col.gameObject.GetComponent<IDamagable<int>>().Damage(1, col.gameObject);
        }
    }
}