using UnityEngine;
using UnityEngine.Assertions;

public class GroundPositionSwitch : MonoBehaviour
{
    [SerializeField] public Transform myTarget;
    [SerializeField] public Quaternion rotationGoal;
    [SerializeField] public Vector3 relativePosition;

    private void Awake()
    {

    }

    void Start()
    {
        myTarget = GameObject.FindGameObjectWithTag("CenterAttractionPlanet").GetComponent<Transform>();
        Assert.IsNotNull(myTarget);
    }

    void Update()
    {
        SetDirectionOfObject();
    }

    public void SetDirectionOfObject()
    {
        relativePosition = (myTarget.localPosition - this.transform.localPosition);
        rotationGoal = Quaternion.LookRotation(relativePosition);

        var xpos = Mathf.Clamp(0.9f * myTarget.localPosition.x, myTarget.localPosition.x, 1.1f * myTarget.localPosition.x);
        var ypos = Mathf.Clamp(0.9f * myTarget.localPosition.y, myTarget.localPosition.y, 1.1f * myTarget.localPosition.y);
        var zpos = Mathf.Clamp(0.9f * myTarget.localPosition.z, myTarget.localPosition.z, 1.1f * myTarget.localPosition.z);

        var newPosition = new Vector3(-xpos, ypos, -zpos);

        if (this.transform.position.y < 0)
        {

            newPosition = new Vector3(xpos, ypos, zpos);

        }

        this.transform.rotation = Quaternion.FromToRotation(this.transform.localPosition, newPosition);


        // the second argument, upwards, defaults to Vector.up
        //this.transform.rotation = Quaternion.LookRotation(this.transform.position, rotationGoal);

        //if (this.transform.position.y > 0)
        //{
        //    //var xpos = Mathf.Clamp(0.9f * this.transform.localPosition.x, this.transform.localPosition.x, 1.1f * this.transform.localPosition.x);
        //    //var ypos = Mathf.Clamp(0.9f * this.transform.localPosition.y, this.transform.localPosition.y, 1.1f * this.transform.localPosition.y);
        //    //var zpos = Mathf.Clamp(0.9f * this.transform.localPosition.z, this.transform.localPosition.z, 1.1f * this.transform.localPosition.z);

        //    var newPosition = new Vector3(xpos, ypos, zpos);
        //    this.transform.rotation = Quaternion.FromToRotation(this.transform.localPosition, myTarget.localPosition);
        //}
        //else
        //{

        //    this.transform.rotation = Quaternion.FromToRotation(this.transform.localPosition, myTarget.localPosition);
        //}
    }

}