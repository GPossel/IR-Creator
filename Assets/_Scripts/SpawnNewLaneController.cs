using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;

public class SpawnNewLaneController : MonoBehaviour
{
    [SerializeField] SpawnWorldInformationScriptableObject spawnWorldInformation;
    [SerializeField] GameObject planeReferenceInWorld;
    [SerializeField] GameManager2 myGameManager;

    // we want to only do it once
    public bool isHit = false;

    private void Awake()
    {
        if (myGameManager == null)
            myGameManager = FindObjectOfType<GameManager2>();

        Assert.IsNotNull(myGameManager);
    }

    private void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.CompareTag("Player"))
        {
            if (!isHit)
            {
                SpawnNewRoad();
                myGameManager.ChangeState(GameManager2.GameState.SpawnPoints);
                isHit = true;
            }
        }
    }

    private void SpawnNewRoad()
    {
        // missing spawning of new items
        GameObject newObj = Instantiate(spawnWorldInformation.GetPrefabOfPlane(), new Vector3(0, 0, planeReferenceInWorld.transform.position.z + 5000), Quaternion.identity);
        newObj.name = $"Plane_Z_value_{transform.position.z}";

        // now trigger a respawn to instantiate all the objects
        myGameManager.ReSpawnObjectsOnWorld(newObj);

        // we need to set the prefab correctly
        StartCoroutine(DestroyOldPlane());
    }

    IEnumerator DestroyOldPlane()
    {
        Debug.Log("Destorying of plane started");
        yield return new WaitForSeconds(40);
        Destroy(planeReferenceInWorld);
    }
}