using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

public class TreasureHintHelpUIController : MonoBehaviour, IBindInventoryHolder<InventoryHolderScriptableObject>
{
    [SerializeField] private GameManager2 myGameManager;
    [SerializeField] public InventoryChannel InventoryChannel;
    [SerializeField] public InventoryHolderScriptableObject Inventory;

    [SerializeField] public List<InventoryItemSO> AllCollectableItems;
    [SerializeField] public GameObject treasureHuntHelperPrefab;

    [SerializeField] public GameObject Slot_Hint_UI_Prefab;

    private Dictionary<GameObject, InventoryItemSO> DicInventoryAndHintUI = new Dictionary<GameObject, InventoryItemSO>();


    private bool stopped = false;

    void Awake()
    {
        if (myGameManager == null)
            myGameManager = FindObjectOfType<GameManager2>();
    }

    private void Start()
    {
        Assert.IsNotNull(InventoryChannel, "[TreasureHintController] Set InventoryChannel SriptableObject");
        Assert.IsNotNull(Slot_Hint_UI_Prefab);
        Assert.IsNotNull(myGameManager);

        for (int i = 0; i < 1; i++)
        {
            var small_hint_UI = Instantiate(Slot_Hint_UI_Prefab, transform);
            DicInventoryAndHintUI.Add(small_hint_UI, null);
        }

        InventoryChannel.OnUpdateItemAlarmEvent += UpdateSlotStates;
    }

    private void OnDestroy()
    {
        InventoryChannel.OnUpdateItemAlarmEvent -= UpdateSlotStates;
    }


    GameObject closestItemToPickUp;

    private void UpdateSlotStates(InventoryItemSO item, UIDistanceAlarmTypes type)
    {
        // get closest item from spawnedobjects of the type
        GameObject closestItemToPickUp = GetClosestItemToPickUp();
        InventoryItemSO nearestObj = closestItemToPickUp.GetComponent<ItemController>().itemSO;

        var pair = DicInventoryAndHintUI.FirstOrDefault();
        DicInventoryAndHintUI[pair.Key] = nearestObj;

        switch (type)
        {
            case UIDistanceAlarmTypes.None:
                UpdateToNone(item);
                break;
            case UIDistanceAlarmTypes.Cold:
                UpdateItemToCold(item);
                break;
            case UIDistanceAlarmTypes.Warm:
                UpdateItemToWarm(item);
                break;
            case UIDistanceAlarmTypes.Hot:
                UpdateItemToHot(item);
                break;
            default:
                break;
        }
    }

    public GameObject[] spawnedItemInWorld;

    public GameObject GetClosestItemToPickUp()
    {
        spawnedItemInWorld = GameObject.FindGameObjectsWithTag("Item");
        float closestDistance = Mathf.Infinity;
        GameObject closestObj = null;

        foreach (var obj in spawnedItemInWorld)
        {
            float currentDistance;
            currentDistance = Vector3.Distance(transform.position, obj.transform.position);
            if (currentDistance < closestDistance)
            {
                closestDistance = currentDistance;
                closestObj = obj;
            }
        }
        
        return closestObj;
    }

    void Update()
    {
        closestItemToPickUp = GetClosestItemToPickUp();

        if (!stopped)
        {
            var pickedUpItems = Inventory.GetInventorySlots();

            // now we want to swap the item value on the dictionary object
            var mySlot = DicInventoryAndHintUI.FirstOrDefault();

            DicInventoryAndHintUI[mySlot.Key] = closestItemToPickUp.GetComponent<ItemController>().itemSO;
            mySlot.Key.GetComponent<SlotUIState>();
        }
    }

    public void UpdateItemToHot(InventoryItemSO parentItemObject)
    {
        var slotStateUI = DicInventoryAndHintUI.Where(x => x.Value == parentItemObject)
                                               .Select(x => x.Key)
                                               .FirstOrDefault();

        slotStateUI.GetComponent<SlotUIState>().UpdateItemToHot();
    }

    public void UpdateItemToWarm(InventoryItemSO parentItemObject)
    {
        var slotStateUI = DicInventoryAndHintUI.Where(x => x.Value == parentItemObject)
                                               .Select(x => x.Key)
                                               .FirstOrDefault();

        slotStateUI.GetComponent<SlotUIState>().UpdateItemToWarm();
    }

    public void UpdateItemToCold(InventoryItemSO parentItemObject)
    {
        Debug.Log("[TreasurehuntController] does receive an update to cold item");
        var slotStateUI = DicInventoryAndHintUI.Where(x => x.Value == parentItemObject)
                                               .Select(x => x.Key)
                                               .FirstOrDefault();

        slotStateUI.GetComponent<SlotUIState>().UpdateItemToCold();
    }

    public void UpdateToNone(InventoryItemSO parentItemObject)
    {
        var slotStateUI = DicInventoryAndHintUI.Where(x => x.Value == parentItemObject)
                                               .Select(x => x.Key)
                                               .FirstOrDefault();

        slotStateUI.GetComponent<SlotUIState>().UpdateToNone();
    }

    public void StopUIAnimations()
    {
        stopped = true;

        foreach (var slot in DicInventoryAndHintUI.Keys)
        {
            var setState = slot.GetComponent<SlotUIState>();
            setState.StopAllAnimationsOfSlots();
        }
    }

    public void Bind(InventoryHolderScriptableObject inventory)
    {
        this.Inventory = inventory;
    }
}