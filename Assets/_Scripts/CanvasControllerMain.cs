using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class CanvasControllerMain : MonoBehaviour, ICanvasControllerMain
{
    [SerializeField] public GameManager2 myGameManager;

    [SerializeField] public InventoryDeveloperUIController developerPanel;
    [SerializeField] public TreasureHintHelpUIController treasureHintHelperPanel;
    [SerializeField] public SimpleScoreUIController slotsUIController;
    [SerializeField] public CountDownUIController countDownPanel;
    [SerializeField] public EndGameUIController endGamePanel;

    List<GameObject> AllPanels = new List<GameObject>();

    private void Awake()
    {
        if (myGameManager == null)
            myGameManager = FindObjectOfType<GameManager2>();

        Assert.IsNotNull(myGameManager);

        Assert.IsNotNull(developerPanel);
        Assert.IsNotNull(treasureHintHelperPanel);
        Assert.IsNotNull(slotsUIController);
        Assert.IsNotNull(countDownPanel);
        Assert.IsNotNull(endGamePanel);

        AllPanels.Add(developerPanel.gameObject);
        AllPanels.Add(treasureHintHelperPanel.gameObject);
        AllPanels.Add(slotsUIController.gameObject);
        AllPanels.Add(countDownPanel.gameObject);
        AllPanels.Add(endGamePanel.gameObject);

        myGameManager.OnGameStateToEndedEvent += ShowEndPanel;
    }

    private void Start()
    {
        developerPanel.enabled = false;
        endGamePanel.enabled = false;
    }

    private void OnDestroy()
    {
        myGameManager.OnGameStateToEndedEvent -= ShowEndPanel;
    }

    public void SetCanvasCameraAndRender(Camera playerCam, RenderMode renderMode = RenderMode.ScreenSpaceCamera)
    {
        if (this.transform.GetComponent<Canvas>() == null) throw new NotImplementedException("[Canvas TreasureHunt] CanvasControllerMain should be on tha same object that contains the canvas components");

        this.transform.GetComponent<Canvas>().renderMode = renderMode;
        this.transform.GetComponent<Canvas>().worldCamera = playerCam;
    }

    public void Bind(InventoryHolderScriptableObject inventory)
    {
        treasureHintHelperPanel.Inventory = inventory;
        slotsUIController.Inventory = inventory;
        endGamePanel.Inventory = inventory;
    }

    public List<GameObject> GetAllPanels() => AllPanels;

    public void BindCollectablesToView(List<InventoryItemSO> allCollectableItems)
    {
        slotsUIController.AllCollectableItems = allCollectableItems;
        treasureHintHelperPanel.AllCollectableItems = allCollectableItems;
    }

    public void ShowEndPanel()
    {
        endGamePanel.gameObject.SetActive(true);
        endGamePanel.GetComponent<EndGameUIController>().CreateSlotsOnTheView();
        endGamePanel.GameUpdateEndGameUI();
    }

    public SimpleScoreUIController GetSimpleScoreUIController() => slotsUIController;
}
