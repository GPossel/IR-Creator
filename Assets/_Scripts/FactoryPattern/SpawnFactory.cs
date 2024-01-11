using UnityEngine;
using System.Linq;

public class SpaceSpawner : SpawnCourse
{
    public SpaceSpawner()
    {
        this.Material = Resources.Load<Material>("Space-grey");
        this.LaneAreas = Resources.LoadAll<GameObject>("LaneWorldAreas_S/");
        this.RoundWorldAreas = Resources.LoadAll<GameObject>("RoundWorldAreas_S/");
        this.Obstacles = Resources.LoadAll<GameObject>("S_Obstacles/");
        this.Items = Resources.LoadAll<GameObject>("Items_S/");

        this.FillUps = Resources.LoadAll<GameObject>("FillUpsOfRoundWorld_S/");
        this.BackDropsLane = Resources.LoadAll<GameObject>("BackDrops_Lane_S/");
        this.BackDropsWorld = Resources.LoadAll<GameObject>("BackDrops_World_S/");

        this.SpawnRoadsOverLays = Resources.LoadAll<GameObject>("Ground_S/");
    }
}

public class WesternSpawner : SpawnCourse
{   public WesternSpawner()
    {
        this.Material = Resources.Load<Material>("Western-beige");
        this.LaneAreas = Resources.LoadAll<GameObject>("LaneWorldAreas_W/");
        this.RoundWorldAreas = Resources.LoadAll<GameObject>("RoundWorldAreas_W/");
        this.Obstacles = Resources.LoadAll<GameObject>("W_Obstacles/");
        this.Items = Resources.LoadAll<GameObject>("Items_W/");

        this.FillUps = Resources.LoadAll<GameObject>("FillUpsOfRoundWorld_W/");
        this.BackDropsLane = Resources.LoadAll<GameObject>("BackDrops_Lane_W/");
        this.BackDropsWorld = Resources.LoadAll<GameObject>("BackDrops_World_W/");

        this.SpawnRoadsOverLays = Resources.LoadAll<GameObject>("Ground_W/");
    }
}

public abstract class SpawnCourse : ISpawnCourseFactory
{
    public Material Material;
    public GameObject[] LaneAreas;
    public GameObject[] RoundWorldAreas;
    public GameObject[] Areas;
    public GameObject[] Obstacles;
    public GameObject[] Items;
    public GameObject[] FillUps;
    public GameObject[] BackDropsLane;
    public GameObject[] BackDropsWorld;
    public GameObject[] SpawnRoadsOverLays;

    public virtual GameObject MakeObstacle()
    {
        // just look for prefabs we want as obstacles... 
        var obstacle = Obstacles[0];
        GameObject gameObj = new GameObject();
        obstacle.AddComponent<ObstacleController>();
        return obstacle;
    }

    public virtual GameObject MakeSpecificObstacle(string name)
    {
        GameObject obstaclePrefab = Obstacles.Where(x => x.name == name).FirstOrDefault();
        if (obstaclePrefab == null)
        {
            obstaclePrefab = Obstacles[0];
            Debug.LogError($"[SpawnFactory] item with name {name}, NOT FOUND! Use default");
        }
        GameObject obstacle = new GameObject();
        obstacle.AddComponent<ObstacleController>();
        return obstacle;
    }

    public virtual GameObject SelectRandomItemPrefab() {
        int randomIndex = Random.Range(0, Items.Length);
        GameObject itemPefab = Items[randomIndex];

        itemPefab.AddComponent<ItemController>();
        return itemPefab;
    }

    public virtual GameObject SelectRandomFillUpPrefab()
    {
        int randomIndex = Random.Range(0, FillUps.Length);
        var fillUpPrefab = FillUps[randomIndex];
        return fillUpPrefab;
    }

    public Material GetMaterial()
    {
        var color = Material;
        return color;
    }
    public GameObject[] GetBackDropsLane() => BackDropsLane;
    public GameObject[] GetBackDropsWorld() => BackDropsWorld;
    public GameObject GetRandomAreaPrefab(IWorldType worldType)
    {
        if (worldType.GetType() == typeof(RoundWorldGamePlay))
        {
            return SpawnHelper.ReturnRandomPrefab(RoundWorldAreas);
        }
        else if (worldType.GetType() == typeof(LaneWorldGamePlay))
        {
            return SpawnHelper.ReturnRandomPrefab(LaneAreas);
        }

        Debug.Log("[SpawnFactory] GetRandomArea of IWorldType not found, please check if new world is added correctly");
        return null;
    }
    public GameObject[] GetItemCollectionToSpawn() => Items;    
    public GameObject[] GetFillUpCollectionToSpawn() => FillUps;
    public GameObject[] GetGroundLayouts() => SpawnRoadsOverLays;
}

public interface ISpawnCourseFactory
{
    public abstract Material GetMaterial();
    public abstract GameObject MakeSpecificObstacle(string name);
    public abstract GameObject MakeObstacle();
    public abstract GameObject SelectRandomItemPrefab();
    public abstract GameObject SelectRandomFillUpPrefab();
    public abstract GameObject GetRandomAreaPrefab(IWorldType worldType);
    public abstract GameObject[] GetItemCollectionToSpawn();
    public abstract GameObject[] GetFillUpCollectionToSpawn();
    public abstract GameObject[] GetBackDropsLane();
    public abstract GameObject[] GetBackDropsWorld();
    public abstract GameObject[] GetGroundLayouts();
}
