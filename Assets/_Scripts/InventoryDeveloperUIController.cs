using UnityEngine;
using UnityEngine.Assertions;

public class InventoryDeveloperUIController : MonoBehaviour
{
    [SerializeField] public InventoryChannel InventoryChannel;

    private void Awake()
    {
        Assert.IsNotNull(InventoryChannel, "[InventoryDeveloperUIController] Set InventoryChannel SriptableObject");
    }

    private void Start()
    {
        InventoryChannel.OnAddItemEvent += OnAddItemEvent;
    }

    private void OnDestroy()
    {
        InventoryChannel.OnAddItemEvent -= OnAddItemEvent;
    }

    private void OnAddItemEvent(InventoryItemSO inventroyItemSO)
    {
        Debug.Log("InventoryUIChannel Is listening to the inventoryChannel");
        Debug.Log($"{inventroyItemSO.name} InventoryUIChannel Is listening to the inventoryChannel");
    }
}
