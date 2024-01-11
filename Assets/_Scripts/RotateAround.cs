using UnityEngine;

public class RotateAround : MonoBehaviour
{
    // ref: https://docs.unity3d.com/ScriptReference/Transform.RotateAround.html
    //Assign a GameObject in the Inspector to rotate around
    public GameObject target;
    public float speed = 200f;

    void Update()
    {
        // try get middle of target 
        var middle = (target.transform.right + target.transform.forward) / 2;

        // Spin the object around the target at 20 degrees/second.
        transform.RotateAround(target.transform.position, middle, speed * Time.deltaTime);
    }
}
