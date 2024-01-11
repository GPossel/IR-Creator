using System;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Inventory/InventroyItemSO")]
[Serializable]
public class InventoryItemSO : ScriptableObject
{
    // for the ingame object spawning
    [SerializeField] public GameObject ItemTypePref;

    // for the UI view
    [SerializeField] public GameObject TextField;
    [SerializeField] public GameObject QuantityTextField;
    [SerializeField] public Sprite SpriteSource;

    // inventory management
    [SerializeField] public string Name;
    [SerializeField] public bool CanStack = false;
    [SerializeField] public ItemTypeEnum itemType = ItemTypeEnum.None;
    [SerializeField] public int MaxStackSize;
    [SerializeField] public int ItemSize;

    public virtual void UseItem() { }

    internal InventoryItemSO Bind(InventoryItemSO coinSpawnPrefab)
    {
        Name = coinSpawnPrefab.name;
        CanStack = coinSpawnPrefab.CanStack;
        itemType = coinSpawnPrefab.itemType;
        MaxStackSize = coinSpawnPrefab.MaxStackSize;
        ItemSize = coinSpawnPrefab.ItemSize;
        TextField = coinSpawnPrefab.TextField;
        QuantityTextField = coinSpawnPrefab.QuantityTextField;
        SpriteSource = coinSpawnPrefab.SpriteSource;
        return this;
    }
}