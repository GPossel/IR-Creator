using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Inventory/InventoryChannel")]
public class InventoryChannel : ScriptableObject
{
    public delegate void AddItemDelegate(InventoryItemSO inventroyItemSO);
    public event AddItemDelegate OnAddItemEvent;

    public delegate void UpdateItemInSlotDelegate(InventoryItemSO inventoryItemSO);
    public event UpdateItemInSlotDelegate OnUpdateItemEvent;

    public delegate void RemoveItemInSlotDelegate(InventoryItemSO inventoryItemSO);
    public event RemoveItemInSlotDelegate OnRemoveItemEvent;

    public delegate void CreateItemSlotDelegate(InventorySlotSO inventorySlotSO);
    public event CreateItemSlotDelegate OnCreateItemSlotEvent;

    public delegate void UpdateSlotDelegate(InventorySlotSO inventorySlotSO);
    public event UpdateSlotDelegate OnUpdateItemSlotEvent;

    public delegate void RemoveSlotDelegate(InventorySlotSO inventorySlotSO);
    public event RemoveSlotDelegate OnRemoveSlotEvent;

    public delegate void UpdateItemAlarmDelegate(InventoryItemSO inventorySlotSO, UIDistanceAlarmTypes distanceAlarmTypes);
    public event UpdateItemAlarmDelegate OnUpdateItemAlarmEvent;

    public void RaiseAddItem(InventoryItemSO inventoryItemSO)
    {
        OnAddItemEvent?.Invoke(inventoryItemSO);
    }

    public void RaiseUpdateItem(InventoryItemSO inventoryItemSO)
    {
        OnUpdateItemEvent?.Invoke(inventoryItemSO);
    }

    public void RaiseRemoveItemEvent(InventoryItemSO inventoryItemSO)
    {
        OnRemoveItemEvent?.Invoke(inventoryItemSO);
    }

    public void RaiseCreateSlot(InventorySlotSO inventorySlotSO)
    {
        OnCreateItemSlotEvent?.Invoke(inventorySlotSO);
    }

    public void RaiseUpdateSlot(InventorySlotSO inventorySlotSO)
    {
        OnUpdateItemSlotEvent?.Invoke(inventorySlotSO);
    }

    public void RaiseRemoveSlot(InventorySlotSO inventorySlotSO)
    {
        OnRemoveSlotEvent?.Invoke(inventorySlotSO);
    }

    public void RaiseUpdateItemAlarmEvent(InventoryItemSO inventoryItemSO, UIDistanceAlarmTypes distanceAlarmTypes)
    {
        OnUpdateItemAlarmEvent?.Invoke(inventoryItemSO, distanceAlarmTypes);
    }
}
