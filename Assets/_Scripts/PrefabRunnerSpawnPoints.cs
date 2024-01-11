using UnityEngine;

public class PrefabRunnerSpawnPoints : MonoBehaviour
{
    public Transform[] possibleSpawnPoints;

    // let the object rotate a little bit more random
    public bool AllowTilt;

    public float amplitude = 0.5f;
    public float frequency = 1f;

    public Transform ReturnRandomLocationForITemToSpawn()
    {
        if (possibleSpawnPoints.Length == 1) return possibleSpawnPoints[0];
        var randomIndex = UnityEngine.Random.Range(0, possibleSpawnPoints.Length);
        return possibleSpawnPoints[randomIndex];
    }

    public void RotateObjectSlightlyRandom()
    {
        Vector3 euler = transform.localEulerAngles;
        euler.z = Mathf.Lerp(euler.z, transform.rotation.z, 2.0f * Time.deltaTime); // Mathf.Lerp(euler.z, z, 2.0f * Time.deltaTime);
        transform.localEulerAngles = euler;
    }

    private void OnDrawGizmos()
    {
        foreach (var spot in possibleSpawnPoints)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawCube(spot.position, new Vector3(1, 1, 1));
        }
    }
}