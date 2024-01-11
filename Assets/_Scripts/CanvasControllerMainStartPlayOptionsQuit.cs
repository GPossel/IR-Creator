using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

public class CanvasControllerMainStartPlayOptionsQuit : MonoBehaviour
{
    [SerializeField] SceneOrchestration SceneOrchestration;
    [SerializeField] public InventoryDeveloperUIController developerPanel;
    [SerializeField] public StartPlayOptionsQuitUIController startPlayOptionsQuitUIcontroller;
    [SerializeField] public GameManager2 myGameManager;


    List<GameObject> AllPanels = new List<GameObject>();

    private void Awake()
    {
        Assert.IsNotNull(developerPanel);
        Assert.IsNotNull(startPlayOptionsQuitUIcontroller);

        if (myGameManager == null)
            myGameManager = FindObjectOfType<GameManager2>();

        Assert.IsNotNull(myGameManager);

        AllPanels.Add(developerPanel.gameObject);
        AllPanels.Add(startPlayOptionsQuitUIcontroller.gameObject);
    }

    private void Start()
    {
        if (SceneOrchestration == null)
            SceneOrchestration = FindObjectOfType<SceneOrchestration>();

        Assert.IsNotNull(SceneOrchestration);

        developerPanel.enabled = false;
    }

    private void Update()
    {

    }

    public void SetCanvasCameraAndRender(Camera playerCam, RenderMode renderMode = RenderMode.ScreenSpaceCamera)
    {
        if (this.transform.GetComponent<Canvas>() == null) throw new NotImplementedException("[Canvas TreasureHunt] CanvasControllerMain should be on tha same object that contains the canvas components");

        this.transform.GetComponent<Canvas>().renderMode = renderMode;
        this.transform.GetComponent<Canvas>().worldCamera = playerCam;
    }

    public List<GameObject> GetAllPanels() => AllPanels;

    public void OpenCharacterSelectionWindow() => SceneOrchestration.LoadCharacterSelectionView();
}
