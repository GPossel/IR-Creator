using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

public class GameManager2 : SingletonDontDestroyOnLoad<GameManager2>
{
    private IGameManagerSharedSetupInfo gameManagerSharedSetupInfo;
    public ISpawnCourseFactory spawnCourseFactory;

    [SerializeField] public GameState State;

    [SerializeField] SceneOrchestration SceneOrchestration;
    [SerializeField] SpawnManager SpawnManager;
    [SerializeField] InventoryChannel InventoryChannel;

    [SerializeField] List<PlayerMovementInfoBase> playerMovementForWorlds;
    [SerializeField] public PlayerInventoryTotalSO playerInventoryTotal; 
    [SerializeField] SelectedCharacterScriptableObject Player;

    public delegate void UpdateGameState(GameState state);
    public event UpdateGameState OnGameStateChangedEvent;
    
    public delegate void UpdateGameStateToEnded();
    public event UpdateGameStateToEnded OnGameStateToEndedEvent;

    public InventoryItemSO[] GetAllItemsReferencesFromSpawnCollection()
    {
        var itemCollection = spawnCourseFactory.GetItemCollectionToSpawn();
        return itemCollection.Select(x => x.GetComponent<ItemController>().itemSO).ToArray();
    }

    void Start()
    {
        if (SceneOrchestration == null)
            SceneOrchestration = FindObjectOfType<SceneOrchestration>();

        Assert.IsNotNull(SceneOrchestration, "[GameManager] Missing essential SceneOrchestration");
        Assert.IsNotNull(playerInventoryTotal, "[GameManager] Should contain the complete inventory total for the player");
        Assert.IsNotNull(Player, "[GameManager] Assign the SelectedCharacterScriptableObject");
    }
    
    public void SetSharedInfo(GameManagerSharedSetupInfo sharedGameManagerSetupInfo)
    {
        gameManagerSharedSetupInfo = sharedGameManagerSetupInfo;
    }

    public void RaiseGameStateChanged(GameState state)
    {
        ChangeState(state);
    }

    public GameState GetState() { return State; }

    public void ChangeState(GameState state)
    {
        State = state;
        switch (State)
        {
            case GameState.Start:
                StartUpGame();
                break;
            case GameState.SpawnPoints:
                SpawnPoints();
                break;
            case GameState.SpawnItems:
                SpawnItems();
                break;
            case GameState.StartRun:
                StartRun();
                break;
            case GameState.InRun:
                // idle, just for statemanagement
                break;
            case GameState.EndRun:
                OnGameStateToEndedEvent?.Invoke();
                EndRun();
                break;
        }

        OnGameStateChangedEvent?.Invoke(state);
    }

    private GameConfiguration gameConfiguration;

    private GameObject World;
    private GameObject Canvas;

    private GameObject playerPrefabs;
    private Camera PlayerCamera;
    private GameObject stackPrefab;
    private PlayerMovementInfoBase myMovementConfig;

    private void StartUpGame()
    {
        if (playerPrefabs == null)
        {
            playerPrefabs = Instantiate(Player.myCharacterSO.PrefabOfCharacter);
        }

        stackPrefab = GetStackPrefab();

        myMovementConfig = playerMovementForWorlds.Where(x => x.typeOfWorld == gameManagerSharedSetupInfo.MyWorldType)
                                                  .Where(x => x.typeOfGameMode == gameManagerSharedSetupInfo.MyGameMode)
                                                  .FirstOrDefault();

        if(myMovementConfig == null)
            myMovementConfig = playerMovementForWorlds.Where(x => x.typeOfWorld == gameManagerSharedSetupInfo.MyWorldType)
                                                      .FirstOrDefault();

        gameConfiguration = new GameConfiguration();

        // create world type factory
        switch (gameManagerSharedSetupInfo.MyWorldType)
        {
            case WorldType.None:
                break;
            case WorldType.Round:
                World = CreateRoundWorld();
                gameConfiguration.BindWorldType(new RoundWorldGamePlay(World));
                break;
            case WorldType.Lane:
                World = CreateLane();
                gameConfiguration.BindWorldType(new LaneWorldGamePlay(World));
                break;
            default:
                break;
        }

        switch (gameManagerSharedSetupInfo.MyGameMode)
        {
            case GameMode.None:
                break;
            case GameMode.Classic:
                gameConfiguration.BindGamePlay(new GamePlayClassic());
                break;
            case GameMode.Mission:
                gameConfiguration.BindGamePlay(new GamePlayMission());
                break;
            case GameMode.Treasure:
                gameConfiguration.BindGamePlay(new GamePlayTreasueHunt());
                break;
        }

        // set player basic movement scripts
        // set player camera, gravity, if any
        gameConfiguration.SetPlayerMovementForWorld(playerPrefabs, myMovementConfig);
        gameConfiguration.SetPlayerInventoryForGameMode(InventoryChannel);

        // inventory stacks
        gameConfiguration.SetPlayerStack(stackPrefab);

        switch (gameManagerSharedSetupInfo.MyCollectionType)
        {
            case VisualStyleTypes.None:
                break;
            case VisualStyleTypes.Western:
                spawnCourseFactory = new WesternSpawner();
                break;
            case VisualStyleTypes.Space:
                spawnCourseFactory = new SpaceSpawner();
                break;
        }

        // UI
        switch (gameManagerSharedSetupInfo.MyGameMode)
        {
            case GameMode.None:
                break;
            case GameMode.Classic:
                Canvas = GetClassicGamePlayCanvas();
                break;
            case GameMode.Mission:
                throw new NotImplementedException("[GameManager2] does not have a mission canvas yet..");
            case GameMode.Treasure:
                Canvas = GetTreasureGamePlayCanvas();                
                break;
        }

        // Exception! it gives problems.. 
        // we need an extra force to control the player
        if (gameManagerSharedSetupInfo.MyWorldType == WorldType.Round && gameManagerSharedSetupInfo.MyGameMode == GameMode.Classic)
        {
            myMovementConfig.isPushedForward = true;
            var removeComponent = playerPrefabs.GetComponent<PlayerMovementFreeForward>();
            Destroy(removeComponent);
            playerPrefabs.AddComponent<PlayerMovementFreeForward>();
            playerPrefabs.GetComponent<PlayerMovementFreeForward>().BindToPlayer(myMovementConfig);
        }

        // retrieve collection of Items
        GameObject[] items = spawnCourseFactory.GetItemCollectionToSpawn();
        List<InventoryItemSO> allItemSOs = items.ToList().Select(x => x.GetComponent<ItemController>().itemSO).ToList();
        gameConfiguration.SetupUIForGame(Canvas.transform.GetChild(0).GetComponent<ICanvasControllerMain>(), allItemSOs);
        gameConfiguration.SetPlayerCollectablesInLevel(playerPrefabs, allItemSOs);

        ChangeState(GameState.SpawnPoints);
    }

    public void SpawnPoints()
    {
        SpawnAreasOfTheWorld();
        SpawnOverLayOnWorld();
        ChangeState(GameState.SpawnItems);
    }

    public void SpawnItems()
    {
        SpawnItemsOnTheAreas();
        SpawnFillUpsOnWorld();

        ChangeState(GameState.StartRun);
    }

    public void StartRun()
    {
        // These scripts before already create a new instance of the inventory,
        // by using the scriptable object, so it won't be needed to empty out
        // the old one... Probably.

        // throw new NotImplementedException("[GameManager2] StartRun state not existing,
        // please make sure to use a run counter for this run, reset scriptable object items, ");

        ChangeState(GameState.InRun);
    }

    private void EndRun()
    {
        // we find the player with the correct script
        var player = GameObject.FindGameObjectWithTag("Player");

        // add sound effect
        player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;

        var inventoryCollected = player.GetComponent<InventoryHolder>().InventoryHolderSO;

        // we have to write off the information into a different scriptable object,
        // then we destory the SO, because the gamaConfiguration shall create a new one...
        if (player.GetComponent<InventoryHolder>().InventoryHolderSO != null)
        {
            playerInventoryTotal.AddNewItems(inventoryCollected);
            Destroy(player.GetComponent<InventoryHolder>().InventoryHolderSO);
        }
        // Empty out the spawnManager, because it cannot keep empty references to previously created prefabs and spawns
        SpawnManager.Instance.EmptyAll();
        myMovementConfig.isPushedForward = false;
    }

    public void ReSpawnObjectsOnWorld(GameObject worldPrefab)
    {
        ChangeState(GameState.Respawning);

        // we update the world into the new world
        gameConfiguration.RefreshWorld(worldPrefab);
        ChangeState(GameState.InRun);
    }

    public void SpawnAreasOfTheWorld()
    {
        // we need to retrieve the area's on this created world, each world contains an SpawnSplitOnWorldOrLane
        // In a plane this will be straightforward, area 1 area 2, area 3, area 4.
        // In a round world this will be like: up, down, left, right, forward, backward
        var allAreasToSpawnOn = gameConfiguration.GetAreasOfTheWorld();
        var result = gameConfiguration.ReturnRandomSpotForWorldType(allAreasToSpawnOn);

        for (int i = 0; i < result.Length; i++)
        {
            // now we want to get some prefabs of the area's
            var randomAreaPrefab = spawnCourseFactory.GetRandomAreaPrefab(gameConfiguration.GetWorldType());
            var newArea = SpawnManager.Instance.InstantiateAreaPrefabInTheWorld(randomAreaPrefab, result[i].position, result[i].rotation);
            gameConfiguration.SetWorldAsParent(newArea);
            gameConfiguration.SetObstacleController(newArea);
        }
    }

    public void SpawnOverLayOnWorld()
    {
        // set world overlay
        Transform[] overlaySpots = gameConfiguration.GetSpawnSpotsOfOverlay();
        if (overlaySpots == null || !(overlaySpots.Length > 0))
        {
            Debug.Log($"[GameManager] {World.name} does not contain AllSPawnRoadOverlays Script or spawnPositionsForRoad is empty");
            return;
        }

        var spawnRoadPrefabs = spawnCourseFactory.GetGroundLayouts();

        foreach (var spot in overlaySpots)
        {
            var randomIndex = UnityEngine.Random.Range(0, spawnRoadPrefabs.Length);
            var pickRandomPrefab = spawnRoadPrefabs[randomIndex];
            var overlay = SpawnManager.Instance.InstantiateOverlayPrefabInTheWorld(pickRandomPrefab, spot.position, spot.rotation);
            gameConfiguration.SetWorldAsParent(overlay);
        }

            var materialColor = spawnCourseFactory.GetMaterial();
            if (materialColor != null)
                gameConfiguration.SetWorldMaterial(materialColor);
    }


    public void SpawnItemsOnTheAreas()
    {
        // we receive the collection with the items we want to spawn
        GameObject[] items = spawnCourseFactory.GetItemCollectionToSpawn();
        // receive all spots for world, thanks to the gamemode
        var allAreas = SpawnManager.Instance.GetAllAreasSpawned();
        var allPossibleSpawnSpotsForItems = gameConfiguration.GetSpawnSpotsOnGameMode(allAreas);
        var spawnItemFrequencyForWorldType = gameConfiguration.GetFrequencyBalanceWorld();
        // spawnmanager instantiates the 'real' in game object,
        // select random position out of all these things... 
        var allItemsCreated = SpawnManager.Instance.InstantiateRandomItemsInTheWorld(items, allPossibleSpawnSpotsForItems, spawnItemFrequencyForWorldType);

        gameConfiguration.UpdateItemsToGameMode(allItemsCreated, InventoryChannel);

        // make sure we add all the created points as a child of the world
        // we need to do so, so when we destroy the world we remove all its children too
        allItemsCreated.ForEach(x => gameConfiguration.SetWorldAsParent(x));
    }

    public void SpawnFillUpsOnWorld()
    {
        var fillUpsSpots = gameConfiguration.ReturnAllFillUpSpotsForWorld();

        for (int i = 0; i < fillUpsSpots.Length; i++)
        {
            var randomFillUpoPrefab = spawnCourseFactory.SelectRandomFillUpPrefab();
            // now we want to retrieve a rondom prefab from the fillUp collection
            var newFillUp = SpawnManager.Instance.InstantiateSpawnFillUpsInTheWorld(randomFillUpoPrefab, fillUpsSpots[i].position, fillUpsSpots[i].rotation);
            gameConfiguration.SetWorldAsParent(newFillUp);
        }
    
    }

    public GamePlayWorldType GetGamePlayWorldType()
    { 
        return (GamePlayWorldType)gameConfiguration.GetWorldType();
    }

    public List<InventoryItemSO> GetInventoryItemsFromCollection()
    {
        var list = spawnCourseFactory.GetItemCollectionToSpawn().ToList();
        var newList = list.Select(x => x.GetComponent<ItemController>().itemSO ).ToList();
        return newList;
    }

    public Sprite GetInventoryIconFromCollection(ItemTypeEnum currency)
    {
        var list = spawnCourseFactory.GetItemCollectionToSpawn().ToList();
        var InventorySprite = list.Where(x => x.GetComponent<ItemController>().itemSO.itemType == currency)
                                  .Select(x => x.GetComponent<ItemController>().itemSO.SpriteSource)
                                  .FirstOrDefault();
        
        if(InventorySprite != null)
            return InventorySprite;

        return null;
    }

    public GameObject[] GetBackdrops()
    {
        IWorldType worldType = gameConfiguration.GetWorldType();

       if (worldType.GetType() == typeof(RoundWorldGamePlay))
       {
         return spawnCourseFactory.GetBackDropsWorld();
       }
       else if(worldType.GetType() == typeof(LaneWorldGamePlay))
       {
         return spawnCourseFactory.GetBackDropsLane();
       }

        Debug.Log("[GameManager2] GetBackdrops for worldType not found!");
        return null;
    }

    private GameObject CreateLane() => Instantiate(AssetReferenceManager.Instance.MyWorldsPrefabs[0], Vector3.zero, Quaternion.identity);
    private GameObject CreateRoundWorld() => Instantiate(AssetReferenceManager.Instance.MyWorldsPrefabs[1], Vector3.zero, Quaternion.identity);
    private GameObject GetStackPrefab() => AssetReferenceManager.Instance.MyStackObjectPrefabs;
    private GameObject GetClassicGamePlayCanvas() => Instantiate(AssetReferenceManager.Instance.MyUIgamePlayCanvases[0], Vector3.zero, Quaternion.identity);
    private GameObject GetTreasureGamePlayCanvas() => Instantiate(AssetReferenceManager.Instance.MyUIgamePlayCanvases[1], Vector3.zero, Quaternion.identity);
    private GameObject GetMissionGamePlayCanvas() => Instantiate(AssetReferenceManager.Instance.MyUIgamePlayCanvases[2], Vector3.zero, Quaternion.identity);

    public enum GameState
    {
        Start,
        SpawnUI,
        SpawnUsers,
        SpawnPoints,
        SpawnItems,
        SpawnFillUps,
        Spawn1By1s,
        SpawnEnemies,
        StartRun,
        InRun,
        EndRun,
        Stop,
        Idle,
        Respawning,
        End
    }
}