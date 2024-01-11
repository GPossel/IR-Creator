using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
// TODO: improve after scriptable object and spawning or remove
public class LoadInUsedStylingFromGamemanager : MonoBehaviour
{
    [SerializeField] public Transform[] spawnPoints;
    [SerializeField] public List<PrefabCollectableItemBasic> PrefabItemsToSpawnOnWorld;
    [SerializeField] public List<GameObject> MySpawns = new List<GameObject>();
    [SerializeField] public GameObject prefab;

    private void Awake()
    {
        Assert.IsTrue(spawnPoints.Length > 0);
    }

    private void FixedUpdate()
    {
        SpawnFakeEnviroment();
    }


    public void SpawnFakeEnviroment()
    {
        while (MySpawns.Count < spawnPoints.Length)
        {
            foreach (var spot in spawnPoints)
            {
                var randomIndex = Random.Range(0, PrefabItemsToSpawnOnWorld.Count);
                var prefabObj = PrefabItemsToSpawnOnWorld[randomIndex];

                var newGameObject = Instantiate(prefabObj.Object, spot.position, Quaternion.identity);
                // make sure the object has not logic
                //if (newGameObject.GetComponent<ItemController>() != null)
                //{
                //    var newthing = newGameObject.GetComponent<ItemController>().CleanMeUp();
                //    newGameObject.GetComponent<ItemController>().itemSO = newthing;
                //    newGameObject.GetComponent<ItemController>().enabled = false;
                //    //                     newGameObject.GetComponent<ItemController>().enabled = false;

                //}

                MySpawns.Add(newGameObject);

                //MySpawns.Add(Instantiate(randomObj.Object as GameObject, spot.position, Quaternion.identity));
            }
        }

        Debug.Log("Spawning once");
        Debug.Log($"{SceneManager.GetActiveScene().name}");
    }

    public void SetObjectsToSpawn(List<PrefabCollectableItemBasic> itemsToSpawnOnWorld)
    {
        this.PrefabItemsToSpawnOnWorld = itemsToSpawnOnWorld;
    }
}