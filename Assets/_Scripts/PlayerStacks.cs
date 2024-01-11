using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

public class PlayerStacks : MonoBehaviour, IStackObject<int>
{
    public int StackValue = 0;
    public List<ConfigOnScript> scriptJointConfigs = new List<ConfigOnScript>();
    public List<GameObject> childGameObjects = new List<GameObject>();

    [SerializeField] public GameObject cubePref;
    [SerializeField] public GameObject staticCubeStart;


    [SerializeField] public GameObject playerCamera;
    public Vector3 startpositionCameraZ;

    private void Awake()
    {
        // TODO: think about chaning this into a Camera SCRIPT<>, maybe do some fun with it? breaking after fall
        // set player camera
        playerCamera = this.gameObject.transform.GetChild(0).gameObject;
        playerCamera.transform.localPosition = new Vector3(0, 2, -5);

        // get stack static object
        staticCubeStart = this.gameObject.transform.GetChild(1).gameObject;


        Assert.IsTrue(playerCamera.tag == "MainCamera");
        Assert.IsNotNull(playerCamera);
        Assert.IsNotNull(staticCubeStart);
        startpositionCameraZ = playerCamera.transform.localPosition;
        Debug.Log(startpositionCameraZ);
    }


    // Start is called before the first frame update
    void Start()
    {
        scriptJointConfigs.Add(new ConfigOnScript(2500, 2500, float.PositiveInfinity, float.PositiveInfinity));
        scriptJointConfigs.Add(new ConfigOnScript(2000, 2000, float.PositiveInfinity, float.PositiveInfinity));
        scriptJointConfigs.Add(new ConfigOnScript(1800, 1800, 1500, 1500));
        scriptJointConfigs.Add(new ConfigOnScript(1400, 1400, 800, 800));
        scriptJointConfigs.Add(new ConfigOnScript(1000, 1000, 500, 500));
        scriptJointConfigs.Add(new ConfigOnScript(1000, 1000, 500, 500));
        scriptJointConfigs.Add(new ConfigOnScript(1000, 1000, 300, 300));
        scriptJointConfigs.Add(new ConfigOnScript(1000, 1000, 300, 300));
        scriptJointConfigs.Add(new ConfigOnScript(1000, 1000, 75, 75));
        scriptJointConfigs.Add(new ConfigOnScript(800, 800, 25, 25));
        scriptJointConfigs.Add(new ConfigOnScript(400, 400, 25, 25));
    }

    public class ConfigOnScript
    {
        public float DriveFactor = 0;
        public float AngularDriveFactor = 0;
        public float BreakForce = 0;
        public float BreakTorque = 0;

        public ConfigOnScript(float driveFactor, float angularDriveFactor, float breakForce, float breakTorque)
        {
            DriveFactor = driveFactor;
            AngularDriveFactor = angularDriveFactor;
            BreakForce = breakForce;
            BreakTorque = breakTorque;
        }

        public override string ToString()
        {
            return $"driveFac: {DriveFactor}, angularDrive: {AngularDriveFactor}, BreakFroce: {BreakForce}, BreakTorque: {BreakTorque}";
        }
    }

    // ref: https://www.youtube.com/watch?v=-6LRXBtbNHg
    // ref: https://www.youtube.com/watch?v=PMLzZCTylyo
    // ref: https://www.youtube.com/watch?v=MElbAwhMvTc

    // Update is called once per frame
    void Update()
    {
        // update camera
        Vector3 pos = playerCamera.transform.position;
        if (childGameObjects.Count > 3)
        {
            var newValue = startpositionCameraZ + new Vector3(0, 0, -2);
            playerCamera.gameObject.transform.localPosition = newValue;
        }
        else
        {
            playerCamera.gameObject.transform.localPosition = startpositionCameraZ;
        }
    }

    private void FixedUpdate()
    {
        if (StackValue > childGameObjects.Count)
        {
            // if it first one, we want to activate the KinematicOne
            if (childGameObjects.Count == 0)
            {
                staticCubeStart.gameObject.SetActive(true);
                childGameObjects.Add(staticCubeStart);
                Debug.Log("Added the first one!");
                return;
            }

            for (int i = 1; i < StackValue; i++)
            {
                Debug.Log("Is object already in list?");
                // check if object is not already existing
                var currObj = childGameObjects.ElementAtOrDefault(i);
                var getConfigsOnPosition = scriptJointConfigs.ElementAtOrDefault(i);

                if (getConfigsOnPosition == null)
                    getConfigsOnPosition = scriptJointConfigs[scriptJointConfigs.Count];

                if (currObj == null)
                {
                    // get previous
                    var previousObj = childGameObjects[i - 1];
                    if (previousObj == null) return;

                    // new position
                    Vector3 TransPosPrevObj = previousObj.transform.position;
                    var desiredOffsetPlayer = TransPosPrevObj + new Vector3(0, 0 + 1, 0);


                    if (i == 1)
                        // we want the cube to slightly spawn a bit up front
                        desiredOffsetPlayer = TransPosPrevObj + new Vector3(0, 0 + 1, 0 + 0.75f); // z = 0.75f

                    // create object
                    CubeStackPrefScript instantiateObj = Instantiate(cubePref).GetComponent<CubeStackPrefScript>(); // make parent
                                                                                                                    // newItemToAdd.transform.parent = transform;

                    instantiateObj.transform.rotation = Quaternion.identity;
                    instantiateObj.transform.position = desiredOffsetPlayer;

                    instantiateObj.Bind(getConfigsOnPosition, previousObj);
                    childGameObjects.Add(instantiateObj.gameObject);

                    Debug.Log($"New item created");
                }
            }
        }
    }

    public void RemoveItem(GameObject gameObject)
    {
        var indexToRemove = childGameObjects.IndexOf(gameObject);
        StackValue -= 1;
        if (indexToRemove == -1) return; // item already destoryed
        childGameObjects.RemoveAt(indexToRemove);
        childGameObjects.Select(x => x != null).ToList(); // rearrange list
    }

    public void Stack(int value)
    {
        // gives time for the FixedUpdate to spawn
        StackValue += value;
        if (StackValue < 0)
        {
            StackValue = 0;
            // TODO: End game!
        }
    }

    public void Bind(GameObject cubePref)
    {
        this.cubePref = cubePref;
    }

    public void StackObject(int amount, GameObject stackObject)
    {
        Stack(amount);
    }
}
