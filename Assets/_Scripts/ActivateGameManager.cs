using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

public class ActivateGameManager : MonoBehaviour
{
    [SerializeField] GameManager2 myGameManager;

    public bool activateOnce = false;

    private void Awake()
    {
        if (myGameManager == null)
            myGameManager = FindObjectOfType<GameManager2>();

        Assert.IsNotNull(myGameManager);
    }

    private void Start()
    {
        SceneManager.sceneUnloaded += OnSceneUnLoaded;
    }

    private void OnSceneUnLoaded(Scene arg0)
    {
        activateOnce = false;
    }

    private void Update()
    {
        //Debug.Log($"currScene: {SceneManager.GetActiveScene().name}");
        if (!activateOnce)
        {
            myGameManager.RaiseGameStateChanged(GameManager2.GameState.Start);
            activateOnce = true;
        }
    }
}