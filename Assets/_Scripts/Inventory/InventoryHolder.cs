using UnityEngine;
using UnityEngine.Assertions;

public class InventoryHolder : MonoBehaviour, IBindInventoryHolder<InventoryHolderScriptableObject>
{
    [SerializeField] public InventoryChannel InventoryChannel;
    [SerializeField] public InventoryHolderScriptableObject InventoryHolderSO;
    [SerializeField] public uint DefaultSlotCount = 0;
    [SerializeField] public bool CanCreateSlots = false;
    [SerializeField] public GameManager2 myGameManager;

    private void Awake()
    {
        if (myGameManager == null)
            myGameManager = FindObjectOfType<GameManager2>();

        Assert.IsNotNull(myGameManager);
    }

    private void Start()
    {
        Assert.IsNotNull(InventoryChannel);

        InventoryChannel.OnCreateItemSlotEvent += OnAddSlot;
        InventoryChannel.OnAddItemEvent += AddItemToSlot;
        InventoryChannel.OnRemoveItemEvent += RemoveItem;
        InventoryChannel.OnUpdateItemEvent += UpdateItem;
        InventoryChannel.OnUpdateItemSlotEvent += UpdateSlot;
    }

    private void OnDestroy()
    {
        InventoryChannel.OnCreateItemSlotEvent -= OnAddSlot;
        InventoryChannel.OnAddItemEvent -= AddItemToSlot;
        InventoryChannel.OnRemoveItemEvent -= RemoveItem;
        InventoryChannel.OnUpdateItemEvent -= UpdateItem;
        InventoryChannel.OnUpdateItemSlotEvent -= UpdateSlot;
    }


    // only for testing purposes -> slots are created by the SimpleScoreUIController
    private void OnAddSlot(InventorySlotSO inventorySlotSO)
    {
        InventoryHolderSO.CreateSlot(inventorySlotSO);
    }

    private void AddItemToSlot(InventoryItemSO inventoryItemSO)
    {
        var listOfSlots = InventoryHolderSO.GetInventorySlots();
        if (listOfSlots.Count == 0) { Debug.Log("You have no slots"); return; } // you have no slots!
        var currSlot = listOfSlots[0];

        if (inventoryItemSO.CanStack)
        {
            var slotsOfSameType = InventoryHolderSO.FindMatchingSlotsByType(inventoryItemSO);
            if (slotsOfSameType != null)
            {
                foreach (var slot in slotsOfSameType)
                {
                    if (slot.Quanity < inventoryItemSO.MaxStackSize)
                    {
                        slot.StackItems();
                        currSlot = slot;
                        UpdateSlot(currSlot);
                        return;
                    }
                }
            }
        }

        foreach (var slot in listOfSlots)
        {
            // find the non occupied slot (low to high)
            if (slot.GetItem() == null)
            {
                slot.HoldItem(inventoryItemSO);
                currSlot = slot;
                currSlot.StackItems(); // we add one, the first one
                UpdateSlot(currSlot);
                return;
            }
        }
    }

    private void UpdateItem(InventoryItemSO inventoryItemSO)
    {
        var slotInList = InventoryHolderSO.FindMatchingSlot(inventoryItemSO);
        if (slotInList == null)
        {
            Debug.Log("SlotsUIController error, The inventorySlot is not found in the list");
            return;
        }

        slotInList.HoldItem(inventoryItemSO);
        slotInList.GetItem().SpriteSource = inventoryItemSO.SpriteSource;
    }

    private void RemoveItem(InventoryItemSO inventoryItemSO)
    {
        var slotInList = InventoryHolderSO.FindMatchingSlot(inventoryItemSO);
        slotInList.RemoveItem();
        UpdateSlot(slotInList);
    }

    private void UpdateSlot(InventorySlotSO inventorySlotSO)
    {
        var slotInList = InventoryHolderSO.FindMatchingSlot(inventorySlotSO.item);
        if (slotInList == null)
        {
            Debug.Log("SlotsUIController error, The inventorySlot is not found in the list");
            return;
        }

        slotInList.HoldItem(inventorySlotSO.GetItem());
    }

    public void Bind(InventoryHolderScriptableObject inventory)
    {
        this.InventoryHolderSO = inventory;
    }

}