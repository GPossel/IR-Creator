using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class SimpleScoreUIController : MonoBehaviour,
                                       IBindInventoryHolder<InventoryHolderScriptableObject>,
                                       IBindPossibleCollectablesInLevel<List<InventoryItemSO>>
{
    public Text Text_Score;
    public Transform playerTrans; // position of player
    [SerializeField] public InventoryHolderScriptableObject Inventory;
    [SerializeField] public SlotsUIController Dedicated_Panel_For_Slots;
    [SerializeField] public List<InventoryItemSO> AllCollectableItems;
    [SerializeField] public InventorySlotSO InventorySlotSO_prefab_XS;
    [SerializeField] public bool turnOffTextOnSlot;

    private void Start()
    {
        Assert.IsNotNull(Text_Score, "[SimpleScoreUIController] Text Score is empty!");
        Assert.IsNotNull(Dedicated_Panel_For_Slots, "[SimpleScoreUIController] No Dedicated_Panel_For_Slots panel assigned for the slot");
        Assert.IsNotNull(AllCollectableItems, "[SimpleScoreUIController] No items in allCollectableItems set");
        Assert.IsNotNull(InventorySlotSO_prefab_XS, "[SimpleScoreUIController] No InventorySlotSO_prefab_XS set");

        // we create slots based on the amount of levelcompleteGoals items to get, use Inventory.CreateSlot for this
        foreach (var item in AllCollectableItems)
        {
            // create the logic behind a 'slot' of the inventoryHolderSO
            var slotWithItem = Inventory.CreateSlotAndReturn(InventorySlotSO_prefab_XS);
            slotWithItem.item = item;
            var newSlotInView = Instantiate(InventorySlotSO_prefab_XS.SlotPrefab, Dedicated_Panel_For_Slots.transform);
            newSlotInView.GetComponent<SlotUIScript>().Bind(slotWithItem);
            Dedicated_Panel_For_Slots.AddToList(newSlotInView);
        }
    }

    void Update()
    {
        if (playerTrans != null)
            Text_Score.text = $"{playerTrans.position.z.ToString("0")}"; // display in who numbers

        var slots = Inventory.GetInventorySlots();

        for (int i = 0; i < slots.Count; i++)
        {
            Dedicated_Panel_For_Slots.UpdateSlotUI(slots[i], turnOffTextOnSlot);
        }
    }

    public void BindPossibleCollectablesInLevel(List<InventoryItemSO> allCollectableItems)
    {
        this.AllCollectableItems = allCollectableItems;
    }

    public void BindToPlayer(Transform playerTransform)
    {
        this.playerTrans = playerTransform;
    }

    public void Bind(InventoryHolderScriptableObject inventory)
    {
        this.Inventory = inventory;
    }
}