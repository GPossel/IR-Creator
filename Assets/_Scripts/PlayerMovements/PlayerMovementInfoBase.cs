using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(menuName = "ScriptableObjects/PlayerMovementInfoBase")]
public class PlayerMovementInfoBase : ScriptableObject
{
    // TAG For reference
    [SerializeField] public WorldType typeOfWorld;
    [SerializeField] public GameMode typeOfGameMode;

    // ROUND WORLD
    [SerializeField] public float WalkSpeed;
    [SerializeField] public float ForwardForce = 400f; // default
    [SerializeField] public float SidewayForce;
    [SerializeField] public float JumpForce;
    [SerializeField] public Vector3 MoveAmount;
    [SerializeField] public Vector3 Direction;
    [SerializeField] public bool isPushedForward;

    // LANE RUNNER
    [SerializeField] public float FallOffPlaneYvalue;
    [SerializeField] public bool IsGrounded;
    [SerializeField] public bool IsFalling;
    [SerializeField] public Ray Ray;
    [SerializeField] public RaycastHit HitGround;
    [SerializeField] public float MaxHeight;
    [SerializeField] public float MinHeight;
    [SerializeField] public float TurnSpeed;
    [SerializeField] public Vector3 CameraOffset;
    [SerializeField] public float StabalizeDirectionStrenght;
    [SerializeField] public bool canFallOffPlane;

    // CLASSIC RUN
    [SerializeField] public float maxForwardSpeed_Z;

    // VIEW
    [SerializeField] public float MouseSenX;
    [SerializeField] public float MouseSenY;
}