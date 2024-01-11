using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

[CreateAssetMenu(menuName = "NewInventorySys/InventoryHolderSO")]
[Serializable]
public class InventoryHolderScriptableObject : ScriptableObject
{
    // only reference the inventory items
    [SerializeField] public List<InventorySlotSO> InventorySlots = new List<InventorySlotSO>();

    public uint SlotCount => (uint)InventorySlots.Count;
    public uint MaxSlots = 3;

    public delegate void SlotUpdateCallback(InventorySlotSO slot);
    public SlotUpdateCallback OnSlotAdded;
    public SlotUpdateCallback OnSlotUpdated;
    public SlotUpdateCallback OnSlotRemoved;

    public void CreateSlot(InventorySlotSO inventorySlotSO)
    {
        if (InventorySlots.Count > MaxSlots)
        {
            Debug.Log("Cannot add more slots!");
        }
        else
        {
            var slot = ScriptableObject.CreateInstance<InventorySlotSO>();
            slot.Bind(inventorySlotSO);
            Assert.IsNotNull(slot.SlotPrefab, "[InventoryHolderScriptableObject] If we create default slots, we need prefabs assigned that belongs to the UI");
            InventorySlots.Add(slot);
        }
    }

    public InventorySlotSO CreateSlotAndReturn(InventorySlotSO inventorySlotSO)
    {
        if (InventorySlots.Count > MaxSlots)
        {
            Debug.Log("Cannot add more slots!");
            return null;
        }
        else
        {
            var slot = ScriptableObject.CreateInstance<InventorySlotSO>();
            slot.Bind(inventorySlotSO);
            Assert.IsNotNull(slot.SlotPrefab, "[InventoryHolderScriptableObject] If we create default slots, we need prefabs assigned that belongs to the UI");
            InventorySlots.Add(slot);
            return slot;
        }
    }

    public void DestroySlot(InventorySlotSO inventorySlotSO)
    {
        InventorySlots.Remove(inventorySlotSO);
        OnSlotRemoved?.Invoke(inventorySlotSO);
    }
    public List<InventorySlotSO> GetInventorySlots() => InventorySlots;
    public List<InventoryItemSO> GetAllInventoryItems()
    {
        var allItems = InventorySlots?.Where(x => x.item != null)
                                      .ToList()
                                      .Select(x => x.item)
                                      .ToList();
        return allItems;
    }

    public InventorySlotSO FindMatchingSlot(InventoryItemSO inventoryItemSO) => InventorySlots
                                                                                               .Where(slots => slots.GetItem() == inventoryItemSO)
                                                                                               .FirstOrDefault();
    public List<InventorySlotSO> FindMatchingSlotsByType(InventoryItemSO inventoryItemSO) => InventorySlots
                                                                                                           .Where(slots => slots.GetItem() == inventoryItemSO)
                                                                                                           .Where(slots => slots.GetItem().itemType == inventoryItemSO.itemType)
                                                                                                           .Where(slots => slots.GetItem().itemType != ItemTypeEnum.None)
                                                                                                           .ToList();

    public bool IsInventoryComplete(List<InventoryItemSO> collectables)
    {
        List<InventoryItemSO> myItemsOnSlots = InventorySlots
                                                            .Where(x => x.Quanity > 0)
                                                            .Select(x => x.item)
                                                            .ToList();
        // we get the diferences in these lists
        var differences = collectables.Except(myItemsOnSlots).ToList();

        if (differences.Count == 0)
            return true;


        return false;
    }
}