using UnityEngine;
using UnityEngine.Assertions;

public class PlanetGravity : MonoBehaviour
{
    public Planet attractorPlanet;
    public Transform playerTransform;

    private void Awake()
    {

    }

    void Start()
    {
        Assert.IsNotNull(attractorPlanet);
        Assert.IsNotNull(playerTransform);
        GetComponent<Rigidbody>().useGravity = false;
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;

        playerTransform = transform;
    }

    void FixedUpdate()
    {
        if (attractorPlanet)
        {
            attractorPlanet.Attract(playerTransform);
        }
    }
}