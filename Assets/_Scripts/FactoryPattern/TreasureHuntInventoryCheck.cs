using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class TreasureHuntInventoryCheck : MonoBehaviour, IBindPossibleCollectablesInLevel<List<InventoryItemSO>>
{
    [SerializeField] public InventoryChannel InventoryChannel;
    [SerializeField] public InventoryHolderScriptableObject InventoryHolderSO;
    [SerializeField] public GameManager2 myGameManager;
    [SerializeField] public List<InventoryItemSO> AllCollectableItems;

    private void Awake()
    {
        // we receive the 'base' information from it's basic inventoryholder script that is on the player
        var inventoryHolderBasicScript = this.GetComponent<InventoryHolder>();
        Assert.IsNotNull(inventoryHolderBasicScript);
        InventoryChannel = inventoryHolderBasicScript.InventoryChannel;
        InventoryHolderSO = inventoryHolderBasicScript.InventoryHolderSO;
        myGameManager = inventoryHolderBasicScript.myGameManager;

        if (myGameManager == null)
            myGameManager = FindObjectOfType<GameManager2>();

        Assert.IsNotNull(myGameManager);
    }

    void Update()
    {
        if (AllCollectableItems != null)
        {
            var countItemsInWorld = SpawnManager.Instance.CountOfItemsStillInWorld();
            if (countItemsInWorld <= 0)
            {
                var myWorld = myGameManager.GetGamePlayWorldType();
                // now trigger a respawn to instantiate all the items to collect
                myGameManager.ReSpawnObjectsOnWorld(myWorld.World);
                myGameManager.ChangeState(GameManager2.GameState.SpawnItems);
                // DO something nice! Like refresh the world animation
                // play succesfukk sound *SPARKS*
            }
        }
    }

    public void BindPossibleCollectablesInLevel(List<InventoryItemSO> allCollectables)
    {
        AllCollectableItems = allCollectables;
    }
}