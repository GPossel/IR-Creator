using UnityEngine;
using UnityEngine.Assertions;

public class PlayerMovementFreeForward : MonoBehaviour, ICameraOwner<GameObject>
{
    [SerializeField] public PlayerMovementInfoBase PlayerMovementInfoBase = null;
    public Vector3 smoothMoveVelocity;
    public Rigidbody playerRigidBody;
    public GameObject playerCamera;

    private void Awake()
    {
        playerRigidBody = GetComponent<Rigidbody>();

        // set player camera
        playerCamera = gameObject.transform.GetChild(0).gameObject;
        playerCamera.transform.localPosition = new Vector3(0, 3, -4);
        playerCamera.transform.localEulerAngles = new Vector3(25, 0, 0);

        Assert.IsNotNull(playerRigidBody);
        Assert.IsNotNull(playerCamera);
        Assert.IsTrue(playerCamera.tag == "MainCamera");
    }

    public void BindToPlayer(PlayerMovementInfoBase playerMovementScript)
    {
        PlayerMovementInfoBase = playerMovementScript;
    }

    private void Start()
    {
        Assert.IsTrue(PlayerMovementInfoBase.WalkSpeed > 0, "Set walkspeed higher than 0!");
    }

    public void Update() 
    {
        transform.Rotate(Vector3.up * Input.GetAxis("Mouse X") * Time.unscaledDeltaTime * PlayerMovementInfoBase.MouseSenX);

        if (!PlayerMovementInfoBase.isPushedForward)
        {
            // move <- -> up and down
            Vector3 moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
            Vector3 targetMoveAmount = moveDir * PlayerMovementInfoBase.WalkSpeed;
            PlayerMovementInfoBase.MoveAmount = Vector3.SmoothDamp(targetMoveAmount, targetMoveAmount, ref smoothMoveVelocity, .15f);
        }
    }

    float minDragValue = 10;
    float maxDragValue = 6; // remember, the higher the drag, the slower you go
    float t = 0.0f;

    public void FixedUpdate()
    {
        if (PlayerMovementInfoBase.isPushedForward)
        {
            // move from drag of 10 lerp into a drag of 3, interval slowly 
            t += 0.01f * Time.deltaTime;
            playerRigidBody.drag = Mathf.Lerp(minDragValue, maxDragValue, t);
            var addForce = transform.forward * 100;
            playerRigidBody.AddForce(addForce);
            playerRigidBody.MovePosition(playerRigidBody.position + transform.TransformDirection(PlayerMovementInfoBase.MoveAmount) * Time.unscaledDeltaTime);
        }
        else {
            playerRigidBody.MovePosition(playerRigidBody.position + transform.TransformDirection(PlayerMovementInfoBase.MoveAmount) * Time.unscaledDeltaTime);
        }
    }

    public GameObject GetCamera() => playerCamera;
}