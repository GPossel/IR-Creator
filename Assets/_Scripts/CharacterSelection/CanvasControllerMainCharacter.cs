using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class CanvasControllerMainCharacter : MonoBehaviour
{
    [SerializeField] SceneOrchestration SceneOrchestration;
    [SerializeField] public InventoryDeveloperUIController developerUIController;
    [SerializeField] public SimpleCharacterUIController simpleCharacterUIController;
    [SerializeField] public CharacterSelectedUIView characterSelectedUIView;
    [SerializeField] public Button myReturnBtn;
    [SerializeField] public Button myStartRunBtn;

    void Start()
    {
        if (SceneOrchestration == null)
            SceneOrchestration = FindObjectOfType<SceneOrchestration>();

        Assert.IsNotNull(SceneOrchestration);

        Assert.IsNotNull(developerUIController, "[CanvasControllerMainCharacter] This prefab is missing its reference to developUIController");
        Assert.IsNotNull(simpleCharacterUIController, "[CanvasControllerMainCharacter] This prefab is missing its reference to simpleCharacterUIController");
        Assert.IsNotNull(characterSelectedUIView, "[CanvasControllerMainCharacter] This prefab is missing its reference to CharacterSelectedUIView");
        Assert.IsNotNull(myReturnBtn, "[CanvasControllerMainCharacter] return button is not linked!");
        Assert.IsNotNull(myStartRunBtn, "[CanvasControllerMainCharacter] startButton is not linked!");

    }

    public void SetCanvasCameraAndRender(Camera playerCam, RenderMode renderMode = RenderMode.ScreenSpaceOverlay)
    {
        if (this.transform.GetComponent<Canvas>() == null) throw new NotImplementedException("[CanvasControllerMainCharacter] CanvasControllerMainCharacter should be on tha same object that contains the canvas components");

        this.transform.GetComponent<Canvas>().worldCamera = playerCam;
        this.transform.GetComponent<Canvas>().renderMode = renderMode;
    }
    public void StartRun() => SceneOrchestration.StartNewRun();

    public void ReturnToMainMenu() => SceneOrchestration.LoadMenu();
}