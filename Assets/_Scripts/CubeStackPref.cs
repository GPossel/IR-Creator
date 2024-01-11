using UnityEngine;
using UnityEngine.Assertions;

public class CubeStackPref : MonoBehaviour
{
    public ConfigurableJoint myConnecitonJoint;

    private void Awake()
    {
        myConnecitonJoint = this.GetComponent<ConfigurableJoint>();
        Assert.IsNotNull(myConnecitonJoint);
    }

    private void Start()
    {

    }

    internal void Bind(PlayerStacks.ConfigOnScript getConfigsOnPosition, GameObject previousObj)
    {
        myConnecitonJoint.yMotion = ConfigurableJointMotion.Locked;
        myConnecitonJoint.connectedBody = previousObj.GetComponent<Rigidbody>(); // connect to previous
        // try configure the limit on the Z axis, prevents the big 'swing' when objects stack
        //configJoint.zMotion = ConfigurableJointMotion.Limited;
        //configJoint.linearLimitSpring = new SoftJointLimitSpring { spring = 20, damper = 10 };
        //// transfer the config info onto the new created object
        myConnecitonJoint.xDrive = new JointDrive { positionSpring = getConfigsOnPosition.DriveFactor, maximumForce = float.PositiveInfinity };
        myConnecitonJoint.yDrive = new JointDrive { positionSpring = getConfigsOnPosition.DriveFactor, maximumForce = float.PositiveInfinity };
        myConnecitonJoint.zDrive = new JointDrive { positionSpring = getConfigsOnPosition.DriveFactor, maximumForce = float.PositiveInfinity };
        myConnecitonJoint.angularXDrive = new JointDrive { positionSpring = getConfigsOnPosition.DriveFactor, maximumForce = float.PositiveInfinity };
        myConnecitonJoint.angularYZDrive = new JointDrive { positionSpring = getConfigsOnPosition.DriveFactor, maximumForce = float.PositiveInfinity };
        myConnecitonJoint.breakForce = getConfigsOnPosition.BreakForce;
        myConnecitonJoint.breakTorque = getConfigsOnPosition.BreakTorque;
    }
}
