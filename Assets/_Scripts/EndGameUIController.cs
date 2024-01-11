using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class EndGameUIController : MonoBehaviour,
                                                 IBindInventoryHolder<InventoryHolderScriptableObject>,
                                                 IBindPossibleCollectablesInLevel<List<InventoryItemSO>>
{
    [SerializeField] SceneOrchestration SceneOrchestration;

    [SerializeField] public InventoryHolderScriptableObject Inventory;
    [SerializeField] public SlotsUIController Dedicated_Panel_For_Slots;
    [SerializeField] public List<InventoryItemSO> AllCollectableItems;
    [SerializeField] public InventorySlotSO InventorySlotSO_prefab_XL;

    [SerializeField] public GameObject InventorySlotView;
    [SerializeField] public Text LevelName_txt;

    [SerializeField] public GameObject MissionStateMentView;
    [SerializeField] public Text NewHighScore_txt;
    [SerializeField] public bool turnOffTextOnSlot;

    [SerializeField] public Button myReturnBtn;
    [SerializeField] public Button myStartRunBtn;

    private void Awake()
    {
        if (SceneOrchestration == null)
            SceneOrchestration = FindObjectOfType<SceneOrchestration>();

        Assert.IsNotNull(SceneOrchestration);

        Assert.IsNotNull(Dedicated_Panel_For_Slots, "[EndGameUIController] No Dedicated_Panel_For_Slots panel assigned for the slot");
        Assert.IsNotNull(AllCollectableItems, "[EndGameUIController] No AllCollectableItems set");
        Assert.IsNotNull(InventorySlotSO_prefab_XL, "[EndGameUIController] No InventorySlotSO_prefab_XL set");

        Assert.IsNotNull(InventorySlotView);
        Assert.IsNotNull(LevelName_txt);

        Assert.IsNotNull(MissionStateMentView);
        Assert.IsNotNull(NewHighScore_txt);

        Assert.IsNotNull(myReturnBtn);
        Assert.IsNotNull(myStartRunBtn);
    }

    public void GameUpdateEndGameUI()
    {
        // todo: make sounds

        // player highscore
        var highScore = PlayerSettings.GetHighScore();
        var newScore = 12; // improve
        // add sounds

        if (newScore > highScore)
        {
            NewHighScore_txt.text = $"NEW HIGHSCORE: {newScore}";
            PlayerSettings.SetHighScore(highScore);
        }
        else
        {
            NewHighScore_txt.text = "";
        }

        InventorySlotView.SetActive(true);
    }

    public void CreateSlotsOnTheView()
    {
        var slotWithItems = Inventory.GetInventorySlots();
        // now we pick all the slots that are in the inventory
        for (int i = 0; i < slotWithItems.Count; i++)
        {
            var slot = slotWithItems[i];
            // similiar to the creation of slots based on Inventory, (like in SimpleScoreUIController)
            // but change is, the setting and reusing of the prefab
            var newSlotInView = Instantiate(InventorySlotSO_prefab_XL.SlotPrefab, Dedicated_Panel_For_Slots.transform);
            newSlotInView.GetComponent<SlotUIScript>().Bind(slot);
            Dedicated_Panel_For_Slots.AddToList(newSlotInView);
        }

        var slotsCreatedForView = Dedicated_Panel_For_Slots.allMySlotChildren;

        for (int i = 0; i < slotsCreatedForView.Count; i++)
        {
            Dedicated_Panel_For_Slots.UpdateSlotUI(slotsCreatedForView[i].GetComponent<SlotUIScript>().mySlot, turnOffTextOnSlot);
        }
    }

    public void BindPossibleCollectablesInLevel(List<InventoryItemSO> myCollectables)
    {
        this.AllCollectableItems = myCollectables;
    }

    public void Bind(InventoryHolderScriptableObject inventory)
    {
        this.Inventory = inventory;
    }

    public void StartRun() => SceneOrchestration.StartNewRun();

    public void ReturnToMainMenu() => SceneOrchestration.LoadMenu();
}