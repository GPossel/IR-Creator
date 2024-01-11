using System.Collections;
using UnityEngine;

public class FakePlayerMovementIntro : MonoBehaviour
{
    public Rigidbody rb;
    public Vector3 direction;

    public float verticalLookRotation;

    public float walkSpeed = 8;
    public float forwardForce = 10f;
    public float jumpForce = 100f;
    public Vector3 moveAmount;
    public Vector3 smoothMoveVelocity;

    private void Start()
    {
    }

    void Update() // will be called when Time.timeScale = 0f! :D  ref: https://quick-advisors.com/is-fixedupdate-affected-by-timescale/
    {
        transform.Rotate(Vector3.up * Input.GetAxis("Mouse X") * Time.unscaledDeltaTime);
        verticalLookRotation += Input.GetAxis("Mouse Y") * Time.unscaledDeltaTime;

        // move <- -> up and down
        Vector3 moveDir = new Vector3(1, 0, 0.5f).normalized;
        Vector3 targetMoveAmount = moveDir * walkSpeed;
        moveAmount = Vector3.SmoothDamp(targetMoveAmount, targetMoveAmount, ref smoothMoveVelocity, .15f);


        StartCoroutine(JumpSometimes());
    }

    IEnumerator JumpSometimes()
    {
        yield return new WaitForSeconds(30);
        rb.AddForce(transform.up * jumpForce);
    }

    private void FixedUpdate() // won't be called when Time.timeScale = 0f;
    {
        //    direction = new Vector3(xMove, 0, zMove).normalized; // we can only normalize vectors
        //   GetComponent<Rigidbody>().MovePosition(GetComponent<Rigidbody>().position + transform.TransformDirection(direction));


        // difference in local space and world space. By applying the transform direction with the move amount
        // we make sure the player rotates, calcs new pos, and then moves facing that new position. Whereas world space,
        // would let the player move on a x into the distance, into eternity
        rb.MovePosition(rb.position + transform.TransformDirection(moveAmount) * Time.unscaledDeltaTime);

    }
}
