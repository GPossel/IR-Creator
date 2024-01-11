using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

public class SlotsUIController : MonoBehaviour, IBindInventoryHolder<InventoryHolderScriptableObject>
{
    [SerializeField] public GameObject PanelView;
    [SerializeField] public InventoryHolderScriptableObject Inventory;
    [SerializeField] public List<GameObject> allMySlotChildren = new List<GameObject>();

    private void Awake()
    {
        Assert.IsNotNull(PanelView);
    }

    public void UpdateSlotUI(InventorySlotSO inventorySlotSO, bool turnOffTextOnSlot)
    {
        var myUIView = GetSlotsViewFromUI(inventorySlotSO);
        if (myUIView == null) Debug.Log("[SlotsUIController] Slot not added to the allMySlotChildren component");

        var slotUIscript = myUIView.GetComponent<SlotUIScript>();
        slotUIscript.UpdateView(inventorySlotSO, turnOffTextOnSlot);
    }

    public void Bind(InventoryHolderScriptableObject inventory)
    {
        this.Inventory = inventory;
    }

    public void AddToList(GameObject newSlotInView) => allMySlotChildren.Add(newSlotInView);

    public void DeActivateInList(InventorySlotSO inventorySlotSO) => GetSlotsViewFromUI(inventorySlotSO).SetActive(false);
  
    public void ActivateInList(InventorySlotSO inventorySlotSO) => GetSlotsViewFromUI(inventorySlotSO).SetActive(true);

    public GameObject GetSlotsViewFromUI(InventorySlotSO inventorySlotSO) => allMySlotChildren.Where(x => x.GetComponent<SlotUIScript>() != null &&
                                                                                                          x.GetComponent<SlotUIScript>().mySlot == inventorySlotSO)
                                                                                                           .FirstOrDefault();
}