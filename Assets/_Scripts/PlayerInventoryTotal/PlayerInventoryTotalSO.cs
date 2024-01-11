using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/PlayerInventoryTotalSO")]
public class PlayerInventoryTotalSO : ScriptableObject
{
    [SerializeField]
    public List<ItemTypeEnum> totalCollection = new List<ItemTypeEnum>();

    public void AddNewItems(InventoryHolderScriptableObject inventoryHolderSO)
    {
        foreach (var slot in inventoryHolderSO.InventorySlots)
        {
            var quantity = slot.Quanity;
            AddItem(slot, quantity);
        }
    }

    private void AddItem(InventorySlotSO slot, uint quantity)
    {
        for (int i = 0; i < quantity; i++)
        {
            totalCollection.Add(slot.item.itemType);
        }
    }

    public bool RemoveItems(ItemTypeEnum itemType, int quantity)
    {
        var count = CountOf(itemType);

        if (count - quantity < 0)
        {
            return false;
        }

        var removeList = totalCollection.Where(x => x == itemType).ToList();

        totalCollection.OrderBy(x => x == itemType);
        var indexOfFirstItemType = totalCollection.Where(x => x == itemType)
                                                  .Select(x => totalCollection.IndexOf(x))
                                                  .FirstOrDefault();

        totalCollection.RemoveRange(indexOfFirstItemType, quantity);

        return true;
    }

    public List<ItemTypeEnum> GetCollectedItemsDistinct() => totalCollection.Distinct()
                                                                          .ToList();

    public int CountOf(ItemTypeEnum itemType) => totalCollection.Where(x => x == itemType)
                                                                .Count();

    public void EmptyTotalCollection() => totalCollection = new List<ItemTypeEnum>();

}