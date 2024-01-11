using System;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Inventory/InventorySlotSO")]
[Serializable]
public class InventorySlotSO : ScriptableObject
{
    [SerializeField] public GameObject SlotPrefab;
    [SerializeField] public InventoryItemSO item = null;
    [SerializeField] public uint Quanity = 0;

    public InventoryItemSO GetItem() => item;
    public bool HasItem() => item != null;

    public void HoldItem(InventoryItemSO newItem)
    {
        item = newItem;
        Debug.Log($"{item.name} added to slot");
    }

    public void StackItems()
    {
        Quanity++;
    }
    public void RemoveItem()
    {
        Quanity--;
        if (Quanity <= 0) item = null;
    }

    public void Bind(InventorySlotSO inventorySlotSO)
    {
        SlotPrefab = inventorySlotSO.SlotPrefab;
        item = inventorySlotSO.item;
        Quanity = inventorySlotSO.Quanity;
    }
}
