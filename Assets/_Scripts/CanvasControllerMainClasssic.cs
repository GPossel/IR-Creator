using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

// this script is made to be the first object of a UI prefab idea.
// this object should contain the Canvas components and a CanvasControllerMain component
// it will contain all panels that are in the game for the type of gameplay. Keep the first panel always for a developerPanel and the last one for the end of the game panel
public class CanvasControllerMainClasssic : MonoBehaviour, ICanvasControllerMain
{
    [SerializeField] public GameManager2 myGameManager;

    [SerializeField] public InventoryDeveloperUIController developerPanel;
    [SerializeField] public SimpleScoreUIController slotsUIController;
    [SerializeField] public EndGameUIController endGamePanel;

    List<GameObject> AllPanels = new List<GameObject>();

    private void Awake()
    {
        if (myGameManager == null)
            myGameManager = FindObjectOfType<GameManager2>();

        Assert.IsNotNull(myGameManager);
        Assert.IsNotNull(developerPanel, "[CanvasControllerMainClasssic] This prefab is missing its reference to InventoryDeveloperUIController");
        Assert.IsNotNull(slotsUIController, "[CanvasControllerMainClasssic] This prefab is missing its reference to SimpleScoreUIController");
        Assert.IsNotNull(endGamePanel, "[CanvasControllerMainClasssic] This prefab is missing its reference to EndGameUIController");

        AllPanels.Add(developerPanel.gameObject);
        AllPanels.Add(slotsUIController.gameObject);
        AllPanels.Add(endGamePanel.gameObject);

        myGameManager.OnGameStateToEndedEvent += ShowEndPanel;
    }


    private void Start()
    {
        developerPanel.enabled = false;
        slotsUIController.enabled = true;
        endGamePanel.enabled = false;
    }

    private void OnDestroy()
    {
        myGameManager.OnGameStateToEndedEvent -= ShowEndPanel;
    }

    // on each prefab the canvas should be on the first child component (!)
    public void SetCanvasCameraAndRender(Camera playerCam, RenderMode renderMode = RenderMode.ScreenSpaceOverlay)
    {
        if (this.transform.GetComponent<Canvas>() == null) throw new NotImplementedException("[Canvas Classic] CanvasControllerMainClasssic should be on tha same object that contains the canvas components");

        this.transform.GetComponent<Canvas>().worldCamera = playerCam;
        this.transform.GetComponent<Canvas>().renderMode = renderMode;
    }

    public void Bind(InventoryHolderScriptableObject inventory)
    {
        slotsUIController.Inventory = inventory;
        endGamePanel.Inventory = inventory;
    }

    public List<GameObject> GetAllPanels() => AllPanels;

    public void BindCollectablesToView(List<InventoryItemSO> allCollectableItems)
    {
        slotsUIController.AllCollectableItems = allCollectableItems;
    }

    public void ShowEndPanel()
    {
        endGamePanel.gameObject.SetActive(true);
        endGamePanel.GetComponent<EndGameUIController>().CreateSlotsOnTheView();
        endGamePanel.GameUpdateEndGameUI();
    }

    public SimpleScoreUIController GetSimpleScoreUIController() => slotsUIController;
}