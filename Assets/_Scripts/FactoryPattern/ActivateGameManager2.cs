using UnityEngine;
using UnityEngine.Assertions;

public class ActivateGameManager2 : MonoBehaviour
{
    [SerializeField] GameManager2 myGameManager;

    public bool activateOnce = false;

    private void Awake()
    {
        if (myGameManager == null)
            myGameManager = FindObjectOfType<GameManager2>();

        Assert.IsNotNull(myGameManager);
    }

    private void Update()
    {
        if (!activateOnce)
        {
            myGameManager.ChangeState(GameManager2.GameState.Start);
            //myGameManager.RaiseGameStateChanged(GameManager2.GameState.Start);
            activateOnce = true;
        }
    }
}
