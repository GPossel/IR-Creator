using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

public class TreasureHuntItemAlarmHelper : MonoBehaviour, ITreasureHuntAlarm
{
    [SerializeField] public InventoryChannel InventoryChannel;
    [SerializeField] public SoundManager SoundManager;

    [SerializeField] public SphereCollider HotDistance;
    [SerializeField] public SphereCollider WarmDistence;
    [SerializeField] public SphereCollider ColdDistence;

    private Dictionary<SphereCollider, UIDistanceAlarmTypes> matchDistanceWithTypes = new Dictionary<SphereCollider, UIDistanceAlarmTypes>();

    private SphereCollider currentSphereCollider;

    private bool stopSlotUpdates = false;

    private void Start()
    {
        if (SoundManager == null)
            SoundManager = FindObjectOfType<SoundManager>();

        Assert.IsNotNull(SoundManager);
        Assert.IsNotNull(HotDistance);
        Assert.IsNotNull(WarmDistence);
        Assert.IsNotNull(ColdDistence);
        matchDistanceWithTypes.Add(HotDistance, UIDistanceAlarmTypes.Hot);
        matchDistanceWithTypes.Add(WarmDistence, UIDistanceAlarmTypes.Warm);
        matchDistanceWithTypes.Add(ColdDistence, UIDistanceAlarmTypes.Cold);
    }


    public void Update()
    {
        var itemController = this.transform.parent.gameObject.GetComponent<ItemController>();
        if(itemController == null || itemController.IsPickedUp == true) // object alrerady destroyed
            InventoryChannel.RaiseUpdateItemAlarmEvent(itemController.itemSO, UIDistanceAlarmTypes.None);

        if (!stopSlotUpdates)
        {
            if (currentSphereCollider != null)
            {
                if (currentSphereCollider == HotDistance)
                {
                    SoundManager.PlaySound(SoundManager.Sound.AlarmTreasure, SoundManager.Speed.Fast);
                    InventoryChannel.RaiseUpdateItemAlarmEvent(itemController.itemSO, UIDistanceAlarmTypes.Hot);
                }
                else if (currentSphereCollider == WarmDistence)
                {
                    SoundManager.PlaySound(SoundManager.Sound.AlarmTreasure, SoundManager.Speed.Normal);
                    InventoryChannel.RaiseUpdateItemAlarmEvent(itemController.itemSO, UIDistanceAlarmTypes.Warm);
                }
                else if (currentSphereCollider == ColdDistence)
                {
                    SoundManager.PlaySound(SoundManager.Sound.AlarmTreasure, SoundManager.Speed.Slow);
                    InventoryChannel.RaiseUpdateItemAlarmEvent(itemController.itemSO, UIDistanceAlarmTypes.Cold);
                }
            
                return;
            }
                        
            //Debug.Log("No colliders are close witin the Treasure hunter alarm");
            return;
        }
        else
        {
            InventoryChannel.RaiseUpdateItemAlarmEvent(itemController.itemSO, UIDistanceAlarmTypes.None);
        }
    }

    internal void SetCurrentCollider(SphereCollider newSphereCollider)
    {
        currentSphereCollider = newSphereCollider;
    }

    // used if player walks out of the cold area
    internal void RemoveCurrentCollider(SphereCollider closestCollider)
    {
        var typeOfAlarm = matchDistanceWithTypes.Where(x => x.Key == closestCollider)
                                                .Select(x => x.Value)
                                                .FirstOrDefault();

        var itemOfObject = this.transform.parent.gameObject.GetComponent<ItemController>().itemSO;

        // keep in mind: when I leave the XX_warmth area => it becomes 'colder'
        switch (typeOfAlarm)
        {
            case UIDistanceAlarmTypes.Hot:
                currentSphereCollider = WarmDistence;
                InventoryChannel.RaiseUpdateItemAlarmEvent(itemOfObject, UIDistanceAlarmTypes.Warm);
                break;
            case UIDistanceAlarmTypes.Warm:
                currentSphereCollider = ColdDistence;
                InventoryChannel.RaiseUpdateItemAlarmEvent(itemOfObject, UIDistanceAlarmTypes.Cold);
                break;
            case UIDistanceAlarmTypes.Cold:
                currentSphereCollider = null;
                InventoryChannel.RaiseUpdateItemAlarmEvent(itemOfObject, UIDistanceAlarmTypes.None);
                break;
            default:
                break;
        }

    }

    public void CreateColliders()
    {
        HotDistance = this.gameObject.AddComponent<SphereCollider>();
        HotDistance.radius = 4;
        HotDistance.isTrigger = true;
        WarmDistence = this.gameObject.AddComponent<SphereCollider>();
        WarmDistence.radius = 7;
        WarmDistence.isTrigger = true;
        ColdDistence = this.gameObject.AddComponent<SphereCollider>();
        ColdDistence.radius = 10;
        ColdDistence.isTrigger = true;
    }
}
