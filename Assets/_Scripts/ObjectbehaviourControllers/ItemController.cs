using UnityEngine;
using UnityEngine.Assertions;

public class ItemController : MonoBehaviour
{
    [SerializeField] public InventoryChannel channel;
    [SerializeField] public InventoryItemSO itemSO;

    [SerializeField] public SoundManager mySoundManager;

    [SerializeField] public bool IsPickedUp = false;

    private void Awake()
    {
        if (mySoundManager == null)
            mySoundManager = FindObjectOfType<SoundManager>();

        Assert.IsNotNull(mySoundManager);
    }

    private void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.GetComponent<InventoryHolder>() != null)
        {
            IsPickedUp = true;
            mySoundManager.PlaySound(SoundManager.Sound.PickUpCoin);
            channel.RaiseAddItem(itemSO);
            channel.RaiseUpdateItemAlarmEvent(itemSO, UIDistanceAlarmTypes.None);
        }
    }

    private void Update()
    {
        if (gameObject != null)
        {
            if (IsPickedUp)
            {
                Destroy(gameObject);
            }
        }
    }
}