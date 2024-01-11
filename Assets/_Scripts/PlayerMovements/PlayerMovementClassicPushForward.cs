using UnityEngine;
using UnityEngine.Assertions;

public class PlayerMovementClassicPushForward : MonoBehaviour, ICameraOwner<GameObject>
{
    [SerializeField] GameManager2 myGameManager;

    public PlayerMovementInfoBase PlayerMovementInfoBase = null;
    public Rigidbody playerRigidBody;
    public GameObject playerCamera;
    public RaycastHit hitGround;

    private void Awake()
    {
        if (myGameManager == null)
            myGameManager = FindObjectOfType<GameManager2>();

        playerRigidBody = GetComponent<Rigidbody>();

        // set player camera
        playerCamera = gameObject.transform.GetChild(0).gameObject;

        Assert.IsNotNull(playerRigidBody);
        Assert.IsNotNull(playerCamera);
        Assert.IsTrue(playerCamera.tag == "MainCamera");
        Assert.IsNotNull(myGameManager);

    }
    public void BindToPlayer(PlayerMovementInfoBase playerMovementScript)
    {
        PlayerMovementInfoBase = playerMovementScript;
    }

    public void Update()
    {
        playerCamera.gameObject.transform.position = this.transform.position + PlayerMovementInfoBase.CameraOffset;
    }

    Vector3 force;

    public void FixedUpdate()
    {
        var sideForce = this.transform.rotation * new Vector3(0, PlayerMovementInfoBase.SidewayForce * Time.deltaTime, 0);

        if (Input.GetKey(KeyCode.D))
        {
            playerRigidBody.AddForce(PlayerMovementInfoBase.SidewayForce * Time.deltaTime, 0, 0, ForceMode.VelocityChange);
        }

        if (Input.GetKey(KeyCode.A))
        {
            playerRigidBody.AddForce(-PlayerMovementInfoBase.SidewayForce * Time.deltaTime, 0, 0, ForceMode.VelocityChange);
        }

        if (playerRigidBody.velocity.z >= PlayerMovementInfoBase.maxForwardSpeed_Z)
        {
            playerRigidBody.velocity = Vector3.ClampMagnitude(playerRigidBody.velocity, PlayerMovementInfoBase.maxForwardSpeed_Z);
        }
        else 
        {

            // We need to find it's relative force direction, including Quaternions 
            //force = this.transform.rotation * new Vector3(0, 0, forwardForceValue * Time.deltaTime);
            force = new Vector3(0,0, PlayerMovementInfoBase.ForwardForce * Time.deltaTime);
            
            playerRigidBody.AddForce(force);
        }

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.identity, PlayerMovementInfoBase.TurnSpeed);
    }

    private void StabalizeObjectDirection()
    {
        var currPos = transform.rotation;
        var stabalizedRotation = new Quaternion(0, transform.rotation.y, transform.rotation.z, transform.rotation.w);
        playerRigidBody.transform.rotation = Quaternion.Slerp(currPos, stabalizedRotation, PlayerMovementInfoBase.StabalizeDirectionStrenght);
    }

    public GameObject GetCamera() => playerCamera;
}