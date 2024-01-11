using UnityEngine;

public class Waypoints : MonoBehaviour
{
    // ref: https://www.youtube.com/watch?v=TJCOC0gcU4k
    public GameObject[] waypoints;
    int current = 0;
    float rotSpeed;
    public float speed = 0;
    float WRadius = 1;

    private void Update()
    {
        if (Vector3.Distance(waypoints[current].transform.position, transform.position) < WRadius)
        {
            current++;
            if (current >= waypoints.Length)
                current = 0;
        }

        transform.position = Vector3.MoveTowards(transform.position, waypoints[current].transform.position, Time.deltaTime * speed);
    }
}