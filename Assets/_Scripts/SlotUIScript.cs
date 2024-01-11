using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class SlotUIScript : MonoBehaviour
{
    [SerializeField] public InventorySlotSO mySlot;

    [SerializeField] public Text slot_title_txt;
    [SerializeField] public Text slot_quantity_txt;
    [SerializeField] public Image slot_image;

    [SerializeField] public bool CheckOnItemsStillInWorld = false;

    private void Start()
    {
        Assert.IsNotNull(mySlot, "[SlotUIScript] mySlot is not assigned!");
        Assert.IsNotNull(slot_title_txt, "[SlotUIScript] slot_title_txt is not assigned!");
        Assert.IsNotNull(slot_quantity_txt, "[SlotUIScript] slot_quantity_txt is not assigned!");
        Assert.IsNotNull(slot_image, "[SlotUIScript] slot_image is not assigned!");
    }

    public void UpdateView(InventorySlotSO slotInventory, bool turnOffTextOnSlot)
    {
        if (turnOffTextOnSlot)
        {
            UpdateNoTitleView(slotInventory);
            return;
        }

        // FALLBACK TO MISSING ITEM ICON
        if (slotInventory.item != null)
        {
            if (slotInventory.item.Name != "")
                slot_title_txt.gameObject.SetActive(true);
            slot_title_txt.text = slotInventory.item.Name;


            slot_image.sprite = slotInventory.item.SpriteSource;
        }

        if (CheckOnItemsStillInWorld)
        {
            AddNumbOfTotalObjectInWorld(slotInventory);
        }
        else {
            CreateEndPanelView(slotInventory);
        }
    }

    public void AddNumbOfTotalObjectInWorld(InventorySlotSO slotInventory)
    {
        var numbOfItemsNotPickedUp = SpawnManager.Instance.CountOfItemStillInWorld(slotInventory.item.itemType);

        if (numbOfItemsNotPickedUp > 0)
            slot_quantity_txt.GetComponent<Text>().text = numbOfItemsNotPickedUp.ToString();
        else
            slot_quantity_txt.GetComponent<Text>().text = "0";
    }

    public void CreateEndPanelView(InventorySlotSO slotInventory)
    {
        slot_quantity_txt.text = slotInventory.Quanity.ToString();
    }

    public void UpdateNoTitleView(InventorySlotSO slotInventory)
    {
        if (slotInventory.item != null)
        {
            if (slotInventory.Quanity > 1)
                slot_quantity_txt.gameObject.SetActive(true);
            slot_quantity_txt.text = slotInventory.Quanity.ToString();

            slot_image.sprite = slotInventory.item.SpriteSource;
        }
    }

    public void Bind(InventorySlotSO mySlot)
    {
        this.mySlot = mySlot;
    }
}