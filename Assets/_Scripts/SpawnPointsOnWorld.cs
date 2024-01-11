using UnityEngine;
using UnityEngine.Assertions;

public class SpawnPointsOnWorld : MonoBehaviour
{
    public Transform[] SmallSpawnSpotsOnAreaOrDirection;
    public bool makeRed;
    public bool makePink;

    private void Start()
    {
        Assert.IsFalse(SmallSpawnSpotsOnAreaOrDirection.Length <= 0, "[SpawnPointsOnWorld] Your created world its area's is missing the SpawnPointsOnWorld information");
    }


    private void OnDrawGizmos()
    {
        foreach (var spot in SmallSpawnSpotsOnAreaOrDirection)
        {
            if (makeRed == true)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawCube(spot.transform.position, new Vector3(1, 1, 1));
            }
            else if (makePink == true)
            {
                Gizmos.color = Color.magenta;
                Gizmos.DrawCube(spot.transform.position, new Vector3(1, 1, 1));
            }
            else
            {
                Gizmos.color = Color.gray;
                Gizmos.DrawCube(spot.transform.position, new Vector3(1, 1, 1));
            }
        }
    }
}