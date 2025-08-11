using JetBrains.Annotations;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
//TODO:fix input, Split playerstate, despaghettify the code, get bitches
[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private InputHandler InputHandling;
    [HideInInspector] public float VerticalVelocity;
    Rigidbody rb;
    public float MoveSpeed;
    public float JumpForce;
    public float SprintSpeedAddition;
    [HideInInspector] public float RollSpeed;
    public float RollAnimationSpeed;
    [HideInInspector] public float CurrSpeed;
    bool StartRollVelocity = false;
    bool RollDirSet = false;

    [HideInInspector] public bool ShowDebug = false;
    [HideInInspector] public bool WantsToRoll = false;
    [HideInInspector] public Vector2 Move;
    Vector3 MoveForce;
    Vector3 MoveDir;
    private Vector3 RollDirection;
    private Vector3 PreLandDirection;
    Animator PlayerAnimator;
    private Camera MainCamera;
    public enum RollType { Light, Medium, Heavy, Over };
    public RollType CurrentRollType;
    [HideInInspector]public enum PlayerState { Idling, Walking, Sprinting, Jumping, Rolling, Falling, LandingRoll };
    [HideInInspector]public PlayerState CurrentState = PlayerState.Idling;
    public bool IsGroundedThisFrame;




    private void Start()
    {
        MainCamera = Camera.main;
        rb = GetComponent<Rigidbody>();
        //animator setup
        PlayerAnimator = GetComponent<Animator>();
        PlayerAnimator.applyRootMotion = false;
        /*
        PlayerAnimator.SetBool("IsRunning", false);
        PlayerAnimator.SetBool("IsFloating", !IsGrounded);
        PlayerAnimator.SetTrigger("Jump");
        PlayerAnimator.SetTrigger("roll");
        */
    }
    public bool MinMovMagnitude()
    { 
        return Move.sqrMagnitude > 0.01f;
    }
   
    void IdleCheck()
    {
        //is the player idle?
        if (IsGroundedThisFrame && Move.sqrMagnitude < 0.1f && CurrentState != PlayerState.Rolling && CurrentState != PlayerState.Jumping && CurrentState != PlayerState.LandingRoll)
            CurrentState = PlayerState.Idling;

    }
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
    float VerticalVelocityCalc()
    {
        float VerticalVelocity = rb.linearVelocity.y;
        // round down if velocity less than 0.05f
        if (Mathf.Abs(VerticalVelocity) < 0.05f)
            VerticalVelocity = 0f;
        return VerticalVelocity;
    }
    void InitiateRollVelocity()
    {
        StartRollVelocity = true;
    }
   
    public void StartLandingRoll()
    {
        CurrentState = PlayerState.LandingRoll;
        RollDirection = PreLandDirection.normalized;
        RollDirSet = true;
        InitiateRollVelocity();

    }
  
    void RollIntentHandler()
    {
        if (WantsToRoll)
        {
            WantsToRoll = false;

            CurrentState = PlayerState.Rolling;
            PlayerAnimator.SetTrigger("Roll");

            if (MoveDir.sqrMagnitude > 0.1f)
                RollDirection = MoveDir.normalized;
            else
                RollDirection = transform.forward.normalized;

            RollDirSet = false;
        }

    }
    public void EndRoll()
    {
        CurrentState = PlayerState.Idling;
        RollDirSet = false;
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
    bool ShouldMainTainMomentum()
    {
        return !(Move.sqrMagnitude < 0.01f && IsGroundedThisFrame && CurrentState != PlayerState.Rolling && CurrentState != PlayerState.LandingRoll && CurrentState != PlayerState.Jumping);
    }
    void SpeedKillerOnNoInput()
    {
        if (CurrentState == PlayerState.Jumping)
            Task.Delay(500);
        // Snap player to stop if no input is pressed and not rolling
        if (!ShouldMainTainMomentum())
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
        if (MinMovMagnitude() && IsGroundedThisFrame && CurrentState != PlayerState.LandingRoll)
        {
            // Only rotate if we have significant input
            if (MinMovMagnitude())
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
    void JumpHandler()
    {
        if (IsGroundedThisFrame&&InputHandling.WantsToJump)
        {
            rb.AddForce(Vector3.up * JumpForce);
            CurrentState = PlayerState.Jumping;
            PlayerAnimator.SetTrigger("Jump");
            InputHandling.WantsToJump = false;
        }
        return;
    }
    private void Update()
    {
        VerticalVelocity = VerticalVelocityCalc();
        IsGroundedThisFrame = GroundedCheck();
        RollIntentHandler();
        IdleCheck();
        RollParams();

    }
    private void FixedUpdate()
    {

        RollDirectionHandler();
        RollHandler();
        CameraHandler();
        SprintAndWalkSpeedSwitcher();
        JumpHandler();
        MovementHandler();
        SpeedKillerOnNoInput();




    }

}