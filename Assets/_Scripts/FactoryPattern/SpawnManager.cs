using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnManager : SingletonDontDestroyOnLoad<SpawnManager>
{
    public List<GameObject> allAreasSpawned;
    public List<GameObject> allItemsSpawned;
    public List<GameObject> allObstaclesSpawned;
    public List<GameObject> allFillUpsSpawned;
    public List<GameObject> allPrefabScenesCreated;


    private void Start()
    { }

    private void Update()
    {
        CheckEmptyObjects();
    }

    public GameObject InstantiateAreaPrefabInTheWorld(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        var area = Instantiate(prefab, position, rotation);
        AddToAllAreasSpawnedList(area);
        return area;
    }

    public GameObject InstantiateSpawnFillUpsInTheWorld(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        var fillUp = Instantiate(prefab, position, rotation);
        AddToAllFillUpsSpawnedList(fillUp);
        return fillUp;
    }

    public List<GameObject> InstantiateRandomItemsInTheWorld(GameObject[] items, Transform[] allSpawnSpotsForItems, int[] InOrderSpawnFrequencyWorld)
    {
        var listWithItems = new List<GameObject>();
        var randomCoinIndex = UnityEngine.Random.Range(0, items.Length);
        // wait a second to work with the other objects
        // maybe think about the weight of each object to get spawned
        var coin = items[0];
        var specialItem1 = items[1];
        var specialItem2 = items[2];
        var specialItem3 = items[3];

        // just set the collection to spawn (around) 100 items per round, obviously if the spot is illegal it won't do it
        for (int i = 0; i < InOrderSpawnFrequencyWorld[0]; i++) // frequency coins
        {
            var randomSpawnIndex = UnityEngine.Random.Range(0, allSpawnSpotsForItems.Length);

            // we don't want our random to 
            var selectSpot = allSpawnSpotsForItems[randomSpawnIndex];

            /// do check
            if (SpawnHelper.IsSpawnPosLegalSphere(selectSpot.position, 0.5f))
            {
                var item = Instantiate(coin, selectSpot.position, selectSpot.rotation);
                listWithItems.Add(item);
                AddToAllItemsSpawnedList(item);
            }
        }

        // Let's spawn 15 special items, we switch a bit between number 1 and 2
        for (int i = 0; i < InOrderSpawnFrequencyWorld[1]; i++) // frequency special 1 & 2
        {
            var randomSpawnIndex = UnityEngine.Random.Range(0, allSpawnSpotsForItems.Length);

            // we don't want our random to 
            var selectSpot = allSpawnSpotsForItems[randomSpawnIndex];


            var zeroOrOne = UnityEngine.Random.Range(0, 1);
            // pick one of the special items
            var pickSpecial = (zeroOrOne == 0) ? specialItem1 
                                               : specialItem2;

            /// do check
            if (SpawnHelper.IsSpawnPosLegalSphere(selectSpot.position, 0.5f))
            {
                var item = Instantiate(pickSpecial, selectSpot.position, selectSpot.rotation);
                listWithItems.Add(item);
                AddToAllItemsSpawnedList(item);
            }
        }

        // Let's spawn 1 extreme special items. Also if spawn spots are taken, you are just unlukcy
        for (int i = 0; i < InOrderSpawnFrequencyWorld[2]; i++) // frequency rare item
        {
            var randomSpawnIndex = UnityEngine.Random.Range(0, allSpawnSpotsForItems.Length);

            // we don't want our random to 
            var selectSpot = allSpawnSpotsForItems[randomSpawnIndex];

            /// do check
            if (SpawnHelper.IsSpawnPosLegalSphere(selectSpot.position, 0.5f))
            {
                var item = Instantiate(specialItem3, selectSpot.position, selectSpot.rotation);
                listWithItems.Add(item);
                AddToAllItemsSpawnedList(item);
            }
        }

        return listWithItems;
    }

    public GameObject InstantiateOverlayPrefabInTheWorld(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        return Instantiate(prefab, position, rotation);
    }

    private void AddToAllAreasSpawnedList(GameObject area)
    {
        if (allAreasSpawned == null)
            allAreasSpawned = new List<GameObject>();
        allAreasSpawned.Add(area);
        AddToAllPrefabScenesCreatedList(area);
    }

    private void AddToAllFillUpsSpawnedList(GameObject fillUp)
    {
        if (allFillUpsSpawned == null)
            allFillUpsSpawned = new List<GameObject>();
        allFillUpsSpawned.Add(fillUp);
        AddToAllPrefabScenesCreatedList(fillUp);
    }

    private void AddToAllItemsSpawnedList(GameObject item)
    {
        if (allItemsSpawned == null)
            allItemsSpawned = new List<GameObject>();
        allItemsSpawned.Add(item);
        AddToAllPrefabScenesCreatedList(item);
    }

    private void AddToAllObstaclesSpawnedList(GameObject obstacles)
    {
        if (allObstaclesSpawned == null)
            allObstaclesSpawned = new List<GameObject>();
        allAreasSpawned.Add(obstacles);
        AddToAllPrefabScenesCreatedList(obstacles);
    }

    private void AddToAllPrefabScenesCreatedList(GameObject prefabObj)
    {
        if (allPrefabScenesCreated == null)
            allPrefabScenesCreated = new List<GameObject>();
        allPrefabScenesCreated.Add(prefabObj);
    }

    public List<GameObject> GetAllAreasSpawned()
    {
        return allAreasSpawned.Where(x => x != null)
                              .ToList();
    }

    public List<GameObject> GetAllItemsSpawned()
    {
        return allItemsSpawned.Where(x => x != null)
                              .ToList();
    }

    public List<GameObject> GetObstaclesSpawned()
    {
        return allObstaclesSpawned.Where(x => x != null)
                                  .ToList();
    }

    public List<GameObject> GetAllPrefabsCreated()
    {
        return allPrefabScenesCreated.Where(x => x != null)
                                     .ToList();
    }

    public int CountOfItemStillInWorld(ItemTypeEnum itemType) => allItemsSpawned.Where(x => x.GetComponent<ItemController>() != null)
                                                                                .Where(x => x.GetComponent<ItemController>().itemSO.itemType == itemType)
                                                                                .Count();
                                                                       

    public int CountOfItemsStillInWorld() => allItemsSpawned.Count;

    private void CheckEmptyObjects()
    {
        allAreasSpawned.Where(x => x == null)
            .ToList()
            .ForEach(x => allAreasSpawned.Remove(x));

        allItemsSpawned.Where(x => x == null)
            .ToList()
            .ForEach(x => allItemsSpawned.Remove(x));

        allObstaclesSpawned.Where(x => x == null)
            .ToList()
            .ForEach(x => allObstaclesSpawned.Remove(x));

        allPrefabScenesCreated.Where(x => x == null)
            .ToList()
            .ForEach(x => allPrefabScenesCreated.Remove(x));
    }

    public void EmptyAll()
    {
        this.allAreasSpawned = new List<GameObject>();
        this.allItemsSpawned = new List<GameObject>();
        this.allObstaclesSpawned = new List<GameObject>();
        this.allPrefabScenesCreated = new List<GameObject>();
    }
}