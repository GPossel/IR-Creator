using UnityEngine;
using UnityEngine.Assertions;

public class LoadBackdrop : MonoBehaviour
{
    [SerializeField] public GameManager2 myGameManager;
    [SerializeField] public GameObject backDrop;

    private void Awake()
    {
        if (myGameManager == null)
            myGameManager = FindObjectOfType<GameManager2>();

        Assert.IsNotNull(myGameManager);
    }

    void Update()
    {
        // problems with the async eventsystem loading of new scene in the SceneOrchestrations
        if (backDrop == null)
        {
            var getBackDropPrefabs = myGameManager.GetBackdrops();
            var indexOfWorld = myGameManager.GetGamePlayWorldType();
            backDrop = new GameObject("PlaceHolderBackDrop");

            // reset the Z index of the spawn object so that you prevent instantiating objects on same spot and that fucks up the position 
            var posZ = getBackDropPrefabs[0].transform.position.z + 10;

            backDrop = Instantiate(getBackDropPrefabs[0], new Vector3(getBackDropPrefabs[0].transform.position.x, getBackDropPrefabs[0].transform.position.y, posZ), getBackDropPrefabs[0].transform.rotation);
        }
    }
}
