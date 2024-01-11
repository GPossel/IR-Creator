using System.Collections.Generic;
using UnityEngine;

public interface ICanvasControllerMain
{
    public void SetCanvasCameraAndRender(Camera playerCam, RenderMode renderMode = RenderMode.ScreenSpaceCamera);
    public void Bind(InventoryHolderScriptableObject inventory);
    public List<GameObject> GetAllPanels();
    public void BindCollectablesToView(List<InventoryItemSO> levelCompleteGoal);
    public void ShowEndPanel();
    public SimpleScoreUIController GetSimpleScoreUIController();
}