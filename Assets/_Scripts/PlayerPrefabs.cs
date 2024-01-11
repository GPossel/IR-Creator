using UnityEngine;
using UnityEngine.Assertions;

public class PlayerPrefabs : MonoBehaviour, IPickUp<InventoryItemSO>
{
    [SerializeField] public SelectedCharacterScriptableObject characterScriptableObject;
    [SerializeField] public InventoryChannel inventoryChannel;
    private void Awake()
    {
        Assert.IsNotNull(characterScriptableObject, "[PlayerPrefab] Player needs a characterScriptableObject");
        Assert.IsNotNull(inventoryChannel, "[PlayerPrefab] Player needs a inventoryChannel");
    }

    private void Update()
    {
    }

    private void Start()
    {
    }

    public void PickUp(InventoryItemSO itemSO)
    {
        inventoryChannel.RaiseAddItem(itemSO);
    }

}