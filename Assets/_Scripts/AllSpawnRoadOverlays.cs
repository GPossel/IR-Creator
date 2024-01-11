using UnityEngine;
using UnityEngine.Assertions;

public class AllSpawnRoadOverlays : MonoBehaviour
{
    [SerializeField] public Transform[] spawnPositionsForRoad;

    void Start()
    {
        Assert.IsTrue(spawnPositionsForRoad.Length > 0);
    }

}