using UnityEngine;
using UnityEngine.Playables;
using static PlayerMovement;

public class NewMonoBehaviourScript : MonoBehaviour
{
    Rigidbody rb;

    public float MoveSpeed;
    public float JumpForce;
    public float SprintSpeedAddition;
    public float RollSpeed;
    public float RollAnimationSpeed;
    
    float CurrSpeed;
    bool StartRollVelocity = false;
    bool RollDirSet = false;
    bool WantsToRoll = false;
    bool IsGroundedThisFrame;

    Vector2 Move;
    Vector3 MoveForce;
    Vector3 MoveDir;
    private Vector3 RollDirection;
    private Vector3 PreLandDirection;
    
    Animator PlayerAnimator;
    
    private Camera MainCamera;
    
    public enum RollType { Light, Medium, Heavy, Over };
    public RollType CurrentRollType;
   
    enum PlayerState { Idling, Walking, Sprinting, Jumping, Rolling, Falling, LandingRoll };
    PlayerState CurrentState = PlayerState.Idling;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void RollParams()
    {
        if (CurrentState != PlayerState.LandingRoll)
        {
            switch (CurrentRollType)
            {
                case RollType.Light:
                    {
                        RollSpeed = 10f;
                        RollAnimationSpeed = 1.3f; break;
                    }
                case RollType.Medium:
                    {
                        RollSpeed = 8f;
                        RollAnimationSpeed = 1f; break;
                    }
                case RollType.Heavy:
                    {
                        RollSpeed = 6f;
                        RollAnimationSpeed = 0.7f; break;
                    }
                case RollType.Over:
                    {
                        RollSpeed = 100f;
                        RollAnimationSpeed = 1f; break;
                    }
            }
        }
        else RollSpeed = 4f;
        PlayerAnimator.SetFloat("RollAnimationSpeed", RollAnimationSpeed);
    }
    public void MidRollSpeedVariance()
    {
        /*
        switch (CurrentRollType)
        {
            case RollType.Light:
                {
                    RollSpeed = 1f;
                     break;
                }
            case RollType.Medium:
                {
                    RollSpeed = 1f;
                    break;
                }
            case RollType.Heavy:
                {
                    RollSpeed = 1f;
                    break;
                }
            case RollType.Over:
                {
                    RollSpeed = 1f;
                    break;
                }
        }
        */
        RollSpeed = 1f;
    }
    bool GroundedCheck()
    {
        bool IsGrounded;
        Vector3 StartPosition = transform.position + Vector3.up * 0.1f;
        float RayCastLength = 0.45f;
        IsGrounded = Physics.Raycast(StartPosition, Vector3.down, RayCastLength);
        return IsGrounded;
    }
    void SprintAndWalkSpeedSwitcher()
    {
        if (CurrentState == PlayerState.Sprinting)
            CurrSpeed = MoveSpeed + SprintSpeedAddition;
        else
            CurrSpeed = MoveSpeed;
    }
    void SpeedKiller()
    {
        //maintain y velocity to not mess with gravity
        rb.linearVelocity = new Vector3(0f, rb.linearVelocity.y, 0f);
    }
    void SpeedKillerOnNoInput()
    {
        // Snap player to stop if no input is pressed and not rolling
        if (Move.sqrMagnitude < 0.01f && IsGroundedThisFrame && CurrentState != PlayerState.Rolling && CurrentState != PlayerState.LandingRoll)
        {

            SpeedKiller();
        }
    }
    void CameraHandler()
    {
        Vector3 CameraForward = MainCamera.transform.forward;
        Vector3 CameraRight = MainCamera.transform.right;
        CameraForward.y = 0;
        CameraRight.y = 0;
        CameraForward.Normalize();
        CameraRight.Normalize();
        MoveDir = (CameraForward * Move.y + CameraRight * Move.x).normalized;
    }
    void RollDirectionHandler()
    {
        if (!IsGroundedThisFrame)
        {
            PreLandDirection = MoveDir.sqrMagnitude > 0.1f
                ? MoveDir.normalized : transform.forward;
        }
    }
    void RollHandler()
    {
        if (CurrentState == PlayerState.Rolling || CurrentState == PlayerState.LandingRoll)
        {

            if (!RollDirSet)
            {

                RollDirSet = true;
            }
            if (StartRollVelocity)
            {
                rb.linearVelocity = new Vector3(RollDirection.x * RollSpeed, rb.linearVelocity.y, RollDirection.z * RollSpeed);
                StartRollVelocity = false;
            }

            return;
        }
    }
    void MovementHandler()
    {
        if (CurrentState == PlayerState.Rolling || CurrentState == PlayerState.LandingRoll) return;
        if (MoveDir.sqrMagnitude > 0.00001f && IsGroundedThisFrame && CurrentState != PlayerState.LandingRoll)
        {
            // Only rotate if we have significant input
            if (MoveDir.magnitude > 0.1f)
            {
                Vector3 TargetDir = MoveDir.normalized;
                Quaternion TargetRot = Quaternion.LookRotation(TargetDir, Vector3.up);
                transform.rotation = TargetRot;
            }

            // Apply movement in camera-relative direction
            rb.linearVelocity = new Vector3(
                MoveDir.x * CurrSpeed,
                rb.linearVelocity.y,
                MoveDir.z * CurrSpeed
            );

        }
    }
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        
    }
}
