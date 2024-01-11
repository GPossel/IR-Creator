using UnityEngine;
using UnityEngine.Assertions;

public class PlayerDoubleJump : MonoBehaviour
{
    [SerializeField] public PlayerMovementInfoBase PlayerMovementInfoBase = null;
    public bool isJumpAllowed = true;
    public Rigidbody playerRigidBody;
    public AnimationStateController AnimationStateController;
    public void BindToPlayer(PlayerMovementInfoBase playerMovementScript)
    {
        PlayerMovementInfoBase = playerMovementScript;
    }

    private void Start()
    {
        playerRigidBody = GetComponent<Rigidbody>();
        Assert.IsNotNull(playerRigidBody);
        Assert.IsNotNull(PlayerMovementInfoBase);
        AnimationStateController = this.GetComponent<AnimationStateController>();
    }

    void Update()
    {
        PlayerMovementInfoBase.IsFalling = playerRigidBody.velocity.y < 0 ? true : false;

        if (PlayerMovementInfoBase.IsFalling)
        {
            if (PlayerMovementInfoBase.Ray.origin.y > PlayerMovementInfoBase.MaxHeight)
                PlayerMovementInfoBase.MaxHeight = PlayerMovementInfoBase.Ray.origin.y;
        }

        PlayerMovementInfoBase.Ray = new Ray(this.transform.position, this.transform.rotation * new Vector3(0, -0.5f, 0));

        CheckGrounded();
        if (Input.GetButtonDown("Jump") && PlayerMovementInfoBase.IsGrounded)
        {
            playerRigidBody.AddForce(transform.up * PlayerMovementInfoBase.JumpForce);
        }

        if (Input.GetButtonDown("Jump") && isJumpAllowed && !PlayerMovementInfoBase.IsGrounded)
        {
            playerRigidBody.AddForce(transform.up * PlayerMovementInfoBase.JumpForce * 0.5f);
            isJumpAllowed = false;
        }

        if (!Input.GetButtonDown("Jump") && PlayerMovementInfoBase.IsGrounded)
        {
            isJumpAllowed = true;
        }

        if (playerRigidBody.position != playerRigidBody.position + transform.TransformDirection(PlayerMovementInfoBase.MoveAmount) * Time.unscaledDeltaTime)
        {
            isMovingForward = true;
            AnimationStateController.m_currentV = 1;
        }
        else
        {
            isMovingForward = false;
            AnimationStateController.m_currentV = 0;
        }
    }

    public RaycastHit hitGroundFar;
    public RaycastHit hitGroundClose;
    public bool isMovingForward;

    private void CheckGrounded()
    {
        bool farRayHitsGround = Physics.Raycast(PlayerMovementInfoBase.Ray, out hitGroundFar, maxDistance: 1f); // high
        Debug.DrawRay(PlayerMovementInfoBase.Ray.origin, PlayerMovementInfoBase.Ray.direction * 2f, Color.blue);

        bool closeRayHitsGround = Physics.Raycast(PlayerMovementInfoBase.Ray, out hitGroundClose, maxDistance: 0.5f); // lower
        Debug.DrawRay(PlayerMovementInfoBase.Ray.origin, PlayerMovementInfoBase.Ray.direction * 0.5f, Color.red);

        bool isAlreadyGrounded = AnimationStateController.m_isGrounded; // on ground

        if (farRayHitsGround && closeRayHitsGround) // we recieve the giving information if the ray was hit
        {
            PlayerMovementInfoBase.IsGrounded = true;
            AnimationStateController.m_isGrounded = true;
            AnimationStateController.m_wasGrounded = true;

            return;
        }

        if (farRayHitsGround && !closeRayHitsGround)
        {
            // we try to trigger the "Land" animation, we are now close to the ground
            Debug.Log("[PlayerMovementFreeForward] Landing on ground!");
            PlayerMovementInfoBase.IsGrounded = false;
            AnimationStateController.m_isGrounded = false;
            AnimationStateController.m_wasGrounded = false;
            return;
        }

        if (!farRayHitsGround && !closeRayHitsGround && PlayerMovementInfoBase.IsGrounded)
        {
            PlayerMovementInfoBase.IsGrounded = false;
            AnimationStateController.m_isGrounded = false;

            Debug.Log("[PlayerMovementFreeForward] Jumping!");
            return;

        }
    }
}