using UnityEngine;
using UnityEngine.Assertions;

public class StartPlayOptionsQuitUIController : MonoBehaviour
{
    [SerializeField] SceneOrchestration SceneOrchestration;

    // this class will handle the button request back to the seceneorchestration
    private void Awake()
    {
        if (SceneOrchestration == null)
            SceneOrchestration = FindObjectOfType<SceneOrchestration>();

        Assert.IsNotNull(SceneOrchestration);
    }

    public void OnBtnPlayGame() => SceneOrchestration.StartNewRun();

    public void OnBtnQuitGame() => SceneOrchestration.ExitGame();
}