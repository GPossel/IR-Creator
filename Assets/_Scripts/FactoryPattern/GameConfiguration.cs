using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class GameConfiguration
{
    private IWorldType _worldTypeSpecs;
    private IGamePlayMode _gamePlayTypeFactory;

    private GameObject Player;
    private GameObject PlayerCamera;
    private InventoryHolderScriptableObject InventoryHolderSO;

    public GameConfiguration()
    {

    }

    public GameConfiguration(IWorldType wordTypeSpec, IGamePlayMode worldTypeFactory)
    {
        _worldTypeSpecs = wordTypeSpec;
        _gamePlayTypeFactory = worldTypeFactory;
    }

    public void BindInfo(IWorldType wordTypeSpec, IGamePlayMode worldTypeFactory)
    {
        _worldTypeSpecs = wordTypeSpec;
        _gamePlayTypeFactory = worldTypeFactory;
    }
    public void BindWorldType(IWorldType wordTypeSpec) => _worldTypeSpecs = wordTypeSpec;
    public void BindGamePlay(IGamePlayMode worldTypeFactory) =>  _gamePlayTypeFactory = worldTypeFactory;
    private void BindingOfInventoryToScript(GameObject gameObject)
    {
        if (gameObject.GetComponent<IBindInventoryHolder<InventoryHolderScriptableObject>>() != null)
        {
            gameObject.GetComponent<IBindInventoryHolder<InventoryHolderScriptableObject>>().Bind(InventoryHolderSO);
        }
    }
    private void BindingOfPossibleCollectablesInLevel(GameObject gameObject, List<InventoryItemSO> allCollectableItems)
    {
        if (gameObject.GetComponent<IBindPossibleCollectablesInLevel<List<InventoryItemSO>>>() != null)
        {
            gameObject.GetComponent<IBindPossibleCollectablesInLevel<List<InventoryItemSO>>>().BindPossibleCollectablesInLevel(allCollectableItems);
        }
    }

    public void SetPlayerMovementForWorld(GameObject player, PlayerMovementInfoBase playerMovementConfig)
    {
        // set the settings for the movement
        _gamePlayTypeFactory.Bind(playerMovementConfig);
        _gamePlayTypeFactory.SetBaseMovement(player);

        //PlayerPrefabs playerPrefabs; think about using this.... 
        _worldTypeSpecs.SetPlayerMoveMentForWorld(player);

        // bind the movement matching the game mode or 'playstyle'
        _gamePlayTypeFactory.SetPlayerMovementForGameMode(player);

        PlayerCamera = _gamePlayTypeFactory.GetCamera();
        Player = player;

        Assert.IsNotNull(PlayerCamera);
        Assert.IsNotNull(Player);
    }

    public void SetPlayerStack(GameObject stackPrefab)
    {
        if (stackPrefab == null) Debug.Log("[WorldTypeFactroy] StackPrefab is null, did you forget to add it to the AssetreferenceManager?");
        if (Player.GetComponent<PlayerStacks>() != null)
        {
            if (stackPrefab != null)
            {
                Player.GetComponent<PlayerStacks>().Bind(stackPrefab);
                Player.GetComponent<PlayerStacks>().enabled = true;
            }
        }
    }

    public void SetPlayerInventoryForGameMode(InventoryChannel inventoryChannel)
    {
        // create the basic inventory
        InventoryHolderSO = ScriptableObject.CreateInstance<InventoryHolderScriptableObject>();
        InventoryHolderSO.MaxSlots = 3;

        var inventoryOnPlayer = Player.AddComponent<InventoryHolder>();
        // important for the treasure hunt to add these desired objects that will complete the level
        inventoryOnPlayer.InventoryChannel = inventoryChannel;
        inventoryOnPlayer.InventoryHolderSO = InventoryHolderSO;
        BindingOfInventoryToScript(inventoryOnPlayer.gameObject);
        _gamePlayTypeFactory.SetPlayerInventoryForGameMode(Player);
    }

    public void SetupUIForGame(ICanvasControllerMain canvasControllerMain, List<InventoryItemSO> allCollectableItems)
    {
        // debug is this camera is finding its way
        var camera = PlayerCamera.GetComponent<Camera>();

        // set instantiate to do all the bindings and stuff
        canvasControllerMain.SetCanvasCameraAndRender(camera, RenderMode.ScreenSpaceOverlay);
        canvasControllerMain.GetSimpleScoreUIController().BindToPlayer(Player.transform);
        canvasControllerMain.GetSimpleScoreUIController().BindPossibleCollectablesInLevel(allCollectableItems);
        foreach (var panel in canvasControllerMain.GetAllPanels())
        {
            BindingOfInventoryToScript(panel);
            BindingOfPossibleCollectablesInLevel(panel, allCollectableItems);
        }

        _gamePlayTypeFactory.SetUIForGameMode();
    }

    public void SetPlayerCollectablesInLevel(GameObject player, List<InventoryItemSO> allCollectableItems)
    {
        BindingOfPossibleCollectablesInLevel(player, allCollectableItems);
    }

    public void ReplaceSpawnedObjectsForGameMode()
    {
        _gamePlayTypeFactory.ReplaceSpawnedObjectsForGameMode();
    }

    public void SetWorldAsParent(GameObject childOfWorld)
    {
        _worldTypeSpecs.SetWorldAsParent(childOfWorld);
    }

    public void SetMaterialOnWorld(Material material)
    {
        _worldTypeSpecs.SetMaterialOnWorld(material);
    }

    public Transform[] GetSpawnSpotsOfOverlay()
    {
        return _worldTypeSpecs.GetOverlaySpotsOfTheWorld();
    }

    public List<Transform[]> GetAreasOfTheWorld()
    {
        return _worldTypeSpecs.GetAreasOfTheWorld();
    }

    public Transform GetPrefabSpawnPoints(GameObject chosenField)
    {
        return _gamePlayTypeFactory.ReturnSpawnPoints(chosenField);
    }

    public Transform[] ReturnRandomSpotForWorldType(List<Transform[]> spawnOnWorld)
    {
        return _worldTypeSpecs.ReturnAreaSpotForWorld(spawnOnWorld);
    }

    public Transform[] ReturnAllFillUpSpotsForWorld()
    {
        return _worldTypeSpecs.ReturnFillUpSpotsOfTheWorld();
    }
    public Transform[] GetSpawnSpotsOnGameMode(List<GameObject> allSpawnedAreas)
    {
        return _gamePlayTypeFactory.GetSpawnSpotsOnGameMode(allSpawnedAreas);
    }


    public IWorldType GetWorldType()
    {
        return _worldTypeSpecs;
    }

    public void UpdateItemsToGameMode(List<GameObject> items, InventoryChannel inventoryChannel)
    {
        _gamePlayTypeFactory.UpdateItemsToGameMode(items, inventoryChannel);
    }

    public void RefreshWorld(GameObject worldPrefab)
    {
        _worldTypeSpecs.RefreshWorld(worldPrefab);
    }

    public int[] GetFrequencyBalanceWorld()
    {
        return _worldTypeSpecs.GetFrequencyItemSpawnBalanceWorld();
    }

    public void SetObstacleController(GameObject newArea)
    {
        _gamePlayTypeFactory.SetObstacleControllerScripts(newArea);
    }

    internal void SetWorldMaterial(Material material)
    {
        _worldTypeSpecs.SetMaterialOnWorld(material);
    }
}

#region GamePlay Logic

public class GamePlayClassic : BasePlayMode
{
    public void SetPlayerMoveMentForWorld(GameObject player)
    {
        // base class handles this
    }
    public override void SetPlayerInventoryForGameMode(GameObject player)
    {
        player.AddComponent<PlayerStacks>();
    }

    public override GameObject SetPlayerMovementForGameMode(GameObject player)
    {
        player.AddComponent<PlayerMovementClassicPushForward>();
        player.GetComponent<PlayerMovementClassicPushForward>().BindToPlayer(base.myPlayerMovementInfo);
        return player;
    }
    public override void SetUIForGameMode()
    {
        // possible to add extra scripts on the ui for this gamemode
    }
    public override void ReplaceSpawnedObjectsForGameMode()
    {
        throw new System.NotImplementedException();
    }
    public override Transform ReturnSpawnPoints(GameObject chosenField)
    {
        var location = chosenField.GetComponent<PrefabRunnerSpawnPoints>().ReturnRandomLocationForITemToSpawn();
        return location;
    }
    public override Transform[] GetSpawnSpotsOnGameMode(List<GameObject> allSpawnedAreas)
    {
        List<Transform> transforms = new List<Transform>();
        Transform[] allAreaSpawnsTogether = new Transform[allSpawnedAreas.Count];

        for (int i = 0; i < allSpawnedAreas.Count; i++)
        {
            var area = allSpawnedAreas[i];
            var possibleSpawnPointsOnTheTreasurehuntArea = area.GetComponent<PrefabRunnerSpawnPoints>().possibleSpawnPoints;
            if (possibleSpawnPointsOnTheTreasurehuntArea == null)
                throw new System.NotImplementedException("[WorldTypeFactory] The spawned area of the world is missing the PrefabTreasureSpawnPoints script!");
            else
            {
                transforms.AddRange(possibleSpawnPointsOnTheTreasurehuntArea);
            }
        }

        allAreaSpawnsTogether = transforms.ToArray();
        return allAreaSpawnsTogether;
    }
    public override void UpdateItemsToGameMode(List<GameObject> items, InventoryChannel inventoryChannel)
    {
        // TODO: add stackobject script to this maybe.. ? 
    }
    public override void SetObstacleControllerScripts(GameObject newArea)
    {
        for (int i = 0; i < newArea.transform.childCount; i++)
        {
            var child = newArea.transform.GetChild(i);
            if (child.GetComponent<ObstacleController>() == null)
            {
                child.tag = "Obstacle";
                child.gameObject.AddComponent<ObstacleController>();
            }
        }
    }
}

public class GamePlayTreasueHunt : BasePlayMode
{
    public void SetPlayerMoveMentForWorld(GameObject player)
    {
        // base class handles this
    }
    public override void SetPlayerInventoryForGameMode(GameObject player)
    {
        player.AddComponent<TreasureHuntInstigator>();
        player.AddComponent<TreasureHuntInventoryCheck>();
    }

    public override GameObject SetPlayerMovementForGameMode(GameObject player)
    {
        player.AddComponent<PlayerMovementFreeForward>();
        base.myPlayerMovementInfo.WalkSpeed = 25f;
        player.GetComponent<PlayerMovementFreeForward>().BindToPlayer(base.myPlayerMovementInfo);
        return player;
    }
    public override void SetUIForGameMode()
    {
        // possible to add extra scripts on the ui for this gamemode
    }
    public override void ReplaceSpawnedObjectsForGameMode()
    {
        throw new System.NotImplementedException();
    }
    public override Transform ReturnSpawnPoints(GameObject chosenField)
    {
        var location = chosenField.GetComponent<PrefabTreasureSpawnPoints>().ReturnRandomLocationForITemToSpawn();
        return location;
    }
    public override Transform[] GetSpawnSpotsOnGameMode(List<GameObject> allSpawnedAreas)
    {
        List<Transform> transforms = new List<Transform>();
        Transform[] allAreaSpawnsTogether = new Transform[allSpawnedAreas.Count];

        for (int i = 0; i < allSpawnedAreas.Count; i++)
        {
            var area = allSpawnedAreas[i];

            if (area == null)
            {
                Debug.Log("haha");
            }

            var possibleSpawnPointsOnTheTreasurehuntArea = area.GetComponent<PrefabTreasureSpawnPoints>().possibleSpawnPoints;
            if (possibleSpawnPointsOnTheTreasurehuntArea == null)
                throw new System.NotImplementedException("[WorldTypeFactory] The spawned area of the world is missing the PrefabTreasureSpawnPoints script!");
            else
            {
                transforms.AddRange(possibleSpawnPointsOnTheTreasurehuntArea);
            }
        }

        allAreaSpawnsTogether = transforms.ToArray();
        return allAreaSpawnsTogether;
    }
    public override void UpdateItemsToGameMode(List<GameObject> items, InventoryChannel inventoryChannel)
    {
        // we add the prefab for the HuntHelperAlarm on all the items that are spawn
        for (int i = 0; i < items.Count; i++)
        {
            GameObject treasureHuntHelper = new GameObject();
            treasureHuntHelper.AddComponent<SphereCollider>();
            treasureHuntHelper.AddComponent<TreasureHuntItemAlarmHelper>();
            treasureHuntHelper.GetComponent<TreasureHuntItemAlarmHelper>().InventoryChannel = inventoryChannel;
            treasureHuntHelper.GetComponent<TreasureHuntItemAlarmHelper>().CreateColliders();
            treasureHuntHelper.name = "TreasureHuntAlarmHelper";
            treasureHuntHelper.tag = "TreasureHuntAlarm";
            treasureHuntHelper.transform.position = items[i].transform.position;
            treasureHuntHelper.transform.parent = items[i].transform;
            treasureHuntHelper.transform.SetParent(items[i].transform);
        }
    }
    public override void SetObstacleControllerScripts(GameObject newArea)
    {
        // not adding the obstaleController script because we want this gamemode to be more forgiving
    }
}

public class GamePlayMission : BasePlayMode
{
    public override void SetPlayerInventoryForGameMode(GameObject player)
    {
        // base class handles this
    }
    public override void SetUIForGameMode()
    {
        // possible to add extra scripts on the ui for this gamemode
    }
    public override void ReplaceSpawnedObjectsForGameMode()
    {
        throw new System.NotImplementedException();
    }
    public void SetPlayerMoveMentForWorld(GameObject player)
    {
        throw new System.NotImplementedException();
    }
    public override Transform ReturnSpawnPoints(GameObject chosenField)
    {
        return null;
    }
    public override Transform[] GetSpawnSpotsOnGameMode(List<GameObject> allSpawnedAreas)
    {
        return null;
    }
    public override void UpdateItemsToGameMode(List<GameObject> items, InventoryChannel inventoryChannel)
    { }
    public override void SetObstacleControllerScripts(GameObject newArea)
    { }
}

public partial class BasePlayMode : IGamePlayMode
{
    public PlayerMovementInfoBase myPlayerMovementInfo;
    public GameObject _player;

    public GameObject GetCamera()
    {
        GameObject myCam;
        if (_player.GetComponent<ICameraOwner<GameObject>>() == null)
            throw new System.NotImplementedException();

        myCam = _player.GetComponent<ICameraOwner<GameObject>>().GetCamera();

        Assert.IsNotNull(myCam, "[WorldTypeFactory] Make sure the player movement script contains a camera and a ICameraOwner<GameObject> script to return the cerma");
        return myCam;
    }

    public void Bind(PlayerMovementInfoBase playeyMovementInf)
    {
        myPlayerMovementInfo = playeyMovementInf;
    }

    public void SetBaseMovement(GameObject player)
    {
        _player = player;
        player.AddComponent<PlayerMovementGeneral>();
        player.GetComponent<PlayerMovementGeneral>().BindToPlayer(myPlayerMovementInfo);
        player.AddComponent<PlayerDoubleJump>();
        player.GetComponent<PlayerDoubleJump>().BindToPlayer(myPlayerMovementInfo);
    }

    public virtual void ReplaceSpawnedObjectsForGameMode()
    {
    }

    public virtual void SetPlayerInventoryForGameMode(GameObject player)
    {
        // possible to add extra scripts on the ui for this gamemode
    }

    public virtual GameObject SetPlayerMovementForGameMode(GameObject player)
    {
        return null;
    }

    public virtual void SetUIForGameMode()
    {    }

    public virtual Transform ReturnSpawnPoints(GameObject chosenField)
    {
        throw new NotImplementedException("[BasePlayMode].ReturnSpawnPoints Please use a derrived object form the BasePlayMode, like GamePlayClassic, GamePlayTreasurehunt or GamePlayMission");
    }
    public virtual Transform[] GetSpawnSpotsOnGameMode(List<GameObject> allSpawnedAreas)
    {
        return null;
    }

    public virtual void UpdateItemsToGameMode(List<GameObject> items, InventoryChannel inventoryChannel)
    {
        throw new NotImplementedException();
    }

    public virtual void SetObstacleControllerScripts(GameObject newArea)
    {
        throw new NotImplementedException();
    }
}


public interface IGamePlayMode
{
    public void Bind(PlayerMovementInfoBase myPlayerMovementInfo);
    public void SetBaseMovement(GameObject player);
    public GameObject SetPlayerMovementForGameMode(GameObject player);
    public void SetPlayerInventoryForGameMode(GameObject player);
    public void SetUIForGameMode();
    public void ReplaceSpawnedObjectsForGameMode();
    public GameObject GetCamera();
    public Transform ReturnSpawnPoints(GameObject chosenField);
    public Transform[] GetSpawnSpotsOnGameMode(List<GameObject> allSpawnedAreas);
    public void UpdateItemsToGameMode(List<GameObject> items, InventoryChannel inventoryChannel);
    public void SetObstacleControllerScripts(GameObject newArea);
}

#endregion

#region WorldType Logic Movement
public class RoundWorldGamePlay : GamePlayWorldType
{
    public int[] SpawnFrequencyBalance; // this int is responsible for setting the X amount of items to spawn
    public RoundWorldGamePlay(GameObject world)
    {
        World = world;
        SpawnFrequencyBalance = new int[3]; // we only make a difference in: coin, special, rare
        SpawnFrequencyBalance[0] = 25; // coins
        SpawnFrequencyBalance[1] = 6; // everything in between
        SpawnFrequencyBalance[2] = 1; // most rare
    }
    public override void SetPlayerMoveMentForWorld(GameObject player)
    {
        var attractionToPlanet = player.gameObject.AddComponent<PlanetGravity>();
        attractionToPlanet.playerTransform = player.transform;
        attractionToPlanet.attractorPlanet = World.GetComponent<Planet>();

        //// start position
        player.transform.position = new Vector3(0, 18, -24);
        player.transform.rotation = new Quaternion(-43, 0, 0, 0);

        // for animation purposes
        var rb = player.GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    public override Transform[] GetOverlaySpotsOfTheWorld()
    { 
        if(World.GetComponent<AllSpawnRoadOverlays>() != null)
            return World.GetComponent<AllSpawnRoadOverlays>().spawnPositionsForRoad;

        return null;
    }

    public override List<Transform[]> GetAreasOfTheWorld()
    {
        var allAreasToSpawnOn = World.GetComponent<SpawnSplitOnWorldOrLane>().SpawnDirectionOrAreas;
        if (allAreasToSpawnOn == null) throw new NotImplementedException("[WorldTypeFactory] The area is missing SpawnSplitOnWorldOrLaneScript.SpawnDirectionOrAreas");

        List<Transform[]> result = new List<Transform[]>();
        Transform[] forOnlyOneArea = new Transform[allAreasToSpawnOn.Length];
        for (int i = 0; i < allAreasToSpawnOn.Length; i++)
        {
            var area = allAreasToSpawnOn[i];
            var spawnOnWorld = area.gameObject.GetComponent<SpawnPointsOnWorld>().SmallSpawnSpotsOnAreaOrDirection;

            var randomIndex = UnityEngine.Random.Range(0, spawnOnWorld.Length);
            var areaPickOne = spawnOnWorld[randomIndex];

            if (areaPickOne != null)
            {
                forOnlyOneArea[i] = areaPickOne;
            }
        }

        result.Add(forOnlyOneArea);
        return result;
    }
    public override Transform[] ReturnAreaSpotForWorld(List<Transform[]> spawnOnWorld)
    {
        Transform[] worldSpotsForSpawning = new Transform[spawnOnWorld.Count];
        var randomIndexArea = UnityEngine.Random.Range(0, worldSpotsForSpawning.Length);
        var selectOne = spawnOnWorld[randomIndexArea];
        return selectOne;
    }
    public override Transform[] ReturnFillUpSpotsOfTheWorld() => base.ReturnFillUpSpotsOfTheWorld();
    public override void RefreshWorld(GameObject worldPrefab)
    {
        this.World = worldPrefab;
    }
    public override void SetWorldAsParent(GameObject childOfWorld)
    {
        var getThePositionOftheChild = childOfWorld.transform;
        childOfWorld.transform.parent = World.transform;
        childOfWorld.transform.SetParent(World.transform);
        childOfWorld.transform.position = getThePositionOftheChild.position;
    }
    public override int[] GetFrequencyItemSpawnBalanceWorld()
    {
        return SpawnFrequencyBalance;
    }
    public override void SetMaterialOnWorld(Material material)
    {
        this.World.transform.GetComponent<MeshRenderer>().material = material;
    }
}

public class LaneWorldGamePlay : GamePlayWorldType
{
    public int[] SpawnFrequencyBalance; // this int is responsible for setting the X amount of items to spawn
    public LaneWorldGamePlay(GameObject world)
    {
        World = world;
        SpawnFrequencyBalance = new int[3]; // we only make a difference in: coin, special, rare
        SpawnFrequencyBalance[0] = 75; // coins
        SpawnFrequencyBalance[1] = 15; // everything in between
        SpawnFrequencyBalance[2] = 1; // most rare
    }
    public override void SetPlayerMoveMentForWorld(GameObject player)
    {
        // start position
        player.transform.position = new Vector3(0, 0, 0);
        player.transform.rotation = new Quaternion(0, 0, 0, 0);

        // for animation purposes
        var rb = player.GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }
    public override Transform[] GetOverlaySpotsOfTheWorld()
    {
        var spawnPositionsForRoad = World.GetComponentInChildren<AllSpawnRoadOverlays>().spawnPositionsForRoad;
        return spawnPositionsForRoad;
    }
    public override List<Transform[]> GetAreasOfTheWorld()
    {
        List<Transform> holder = new List<Transform>();
        List<Transform[]> result = new List<Transform[]>();

        var allAreasToSpawnOn = World.GetComponent<SpawnSplitOnWorldOrLane>().SpawnDirectionOrAreas;
        if (allAreasToSpawnOn == null) throw new NotImplementedException("[WorldTypeFactory] The area is missing SpawnSplitOnWorldOrLaneScript.SpawnDirectionOrAreas");

        for (int i = 0; i < allAreasToSpawnOn.Length; i++)
        {
            var area = allAreasToSpawnOn[i];
            var spawnSurface = area.gameObject.GetComponent<SpawnPointsOnWorld>().SmallSpawnSpotsOnAreaOrDirection;

            for (int j = 0; j < spawnSurface.Length; j++)
            {
               // difference with round world, we want to keep all these rows,
               // where the round world the spawnpoints mean less, we only want one of the spots on one of the sides.
               // here we want all the spots of the side
               holder.Add(spawnSurface[j]);
            }
        }

        // dump all object to the transform list
        Transform[] transform = new Transform[holder.Count];
        int count = 0;

        foreach (var row in holder)
        {
            transform[count] = row;
            result.Add(transform);
            count++;
        }

        return result;
    }
    public override Transform[] ReturnAreaSpotForWorld(List<Transform[]> spawnOnWorld)
    {
        Transform[] worldSpotsForSpawning = new Transform[spawnOnWorld.Count];
        var randomIndexArea = UnityEngine.Random.Range(0, worldSpotsForSpawning.Length);
        var selectOne = spawnOnWorld[randomIndexArea];
        return selectOne;
    }
    public override Transform[] ReturnFillUpSpotsOfTheWorld() => base.ReturnFillUpSpotsOfTheWorld();
    public override void RefreshWorld(GameObject worldPrefab)
    {
        this.World = worldPrefab;
    }
    public override void SetWorldAsParent(GameObject childOfWorld)
    {
        var getThePositionOftheChild = childOfWorld.transform;
        childOfWorld.transform.parent = World.transform;
        childOfWorld.transform.SetParent(World.transform);
        childOfWorld.transform.position = getThePositionOftheChild.position;
    }
    public override int[] GetFrequencyItemSpawnBalanceWorld()
    {
        return SpawnFrequencyBalance;
    }
    public override void SetMaterialOnWorld(Material material)
    {
        // restrict: keep the plane render as the first object
        this.World.transform.GetChild(0).GetComponent<MeshRenderer>().material = material;
    }
}


public partial class GamePlayWorldType : IWorldType
{
    public GameObject World;

    public virtual void SetPlayerMoveMentForWorld(GameObject player)
    {
    }
    public virtual Transform[] GetOverlaySpotsOfTheWorld()
    {
        var spawnPositionsForRoad = World.GetComponent<AllSpawnRoadOverlays>().spawnPositionsForRoad;
        return spawnPositionsForRoad;
    }
    public virtual List<Transform[]> GetAreasOfTheWorld()
    {
        return null;
    }
    public Transform[] GetSpawnPointsOnTheWorld(GameObject area)
    {
        var spawnPoints = area.GetComponent<SpawnPointsOnWorld>().SmallSpawnSpotsOnAreaOrDirection;
        if (spawnPoints.Length == 0) throw new NotImplementedException("[WorldTypeFactory] The area is missing SpawnPointsOnWorld.SmallSpawnSpotsOnAreaOrDirection");
        return spawnPoints;
    }
    public virtual Transform[] ReturnAreaSpotForWorld(List<Transform[]> spawnOnWorld)
    {
        throw new NotImplementedException("[WorldTypeFactory] virtual Transform ReturnSpotForWorld not overridden!");
    }
    public virtual Transform[] ReturnFillUpSpotsOfTheWorld()
    {
        var allFillUpSpotsToSpawnOn = World.GetComponent<SpawnSplitOnWorldOrLane>().SpawnFillUps;
        if (allFillUpSpotsToSpawnOn == null) return null;
        return allFillUpSpotsToSpawnOn;
    }
    public virtual void RefreshWorld(GameObject worldPrefab)
    {
        throw new NotImplementedException("[WorldTypeFactory] virtual not overridden!");
    }
    public virtual void SetWorldAsParent(GameObject childOfWorld)
    {
        throw new NotImplementedException("[WorldTypeFactory] virtual not overridden!");
    }
    public virtual int[] GetFrequencyItemSpawnBalanceWorld()
    {
        throw new NotImplementedException("[WorldTypeFactory] virtual not overridden!");
    }
    public virtual void SetMaterialOnWorld(Material material)
    {
        throw new NotImplementedException("[WorldTypeFactory] virtual not overridden!");
    }

}


public interface IWorldType
{
    public void SetPlayerMoveMentForWorld(GameObject player);
    public Transform[] GetOverlaySpotsOfTheWorld();
    public List<Transform[]> GetAreasOfTheWorld();
    public Transform[] ReturnAreaSpotForWorld(List<Transform[]> spawnOnWorld);
    public Transform[] ReturnFillUpSpotsOfTheWorld();
    public void RefreshWorld(GameObject worldPrefab);
    public void SetWorldAsParent(GameObject childOfWorld);
    public int[] GetFrequencyItemSpawnBalanceWorld();
    public void SetMaterialOnWorld(Material material);
}

#endregion