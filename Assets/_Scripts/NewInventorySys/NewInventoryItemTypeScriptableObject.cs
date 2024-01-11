using UnityEngine;

[CreateAssetMenu(menuName = "NewInventorySys/NewInventoryItemTypeScriptableObject")]
public class NewInventoryItemTypeScriptableObject : ScriptableObject
{
    [SerializeField]
    public GameObject ItemTypePref;
    [SerializeField]
    public GameObject TextField;
    [SerializeField]
    public GameObject QuantityTextField;
    [SerializeField]
    public string Name;
    [SerializeField]
    public bool CanStack = false;
    [SerializeField]
    public ItemTypeEnum itemType = ItemTypeEnum.None;
    [SerializeField]
    public int MaxStackSize;
    [SerializeField]
    public int ItemSize;
    [SerializeField]
    public Sprite SpriteSource;
}