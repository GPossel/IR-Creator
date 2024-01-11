using UnityEngine;
using UnityEngine.Assertions;

public class SpawnSplitOnWorldOrLane : MonoBehaviour
{

    [SerializeField] public Transform[] SpawnDirectionOrAreas;
    [SerializeField] public Transform[] SpawnFillUps;
    [SerializeField] public Transform[] SpawnClouds;

    void Start()
    {
        Assert.IsTrue(SpawnDirectionOrAreas.Length > 0);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawCube(this.transform.position, new Vector3(10,10,10));
    }
}