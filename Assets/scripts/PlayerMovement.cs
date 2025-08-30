using JetBrains.Annotations;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
//TODO:fix climbing animations
[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private InputHandler InputHandling;
    [SerializeField] private PlayerStateHandler PlayerStateHandling;
    [SerializeField] private TriggerHandling TriggerHandler;
    [HideInInspector] public float VerticalVelocity;
    [HideInInspector] public bool HasSnappedToEntry = false;
    [HideInInspector] public bool HasSnappedToExit = false;
    Rigidbody rb;
    
    public float ClimbSpeed;
    public float MoveSpeed;
    public float JumpForce;
    public float SprintSpeedAddition;
    [HideInInspector] public float RollSpeed;
    
    [HideInInspector] public float CurrSpeed;
    bool StartRollVelocity = false;
    bool RollDirSet = false;
    private Climbable ClimbingClimbable; // Store the climbable we're currently using
    [HideInInspector] public bool ShowDebug = false;
    [HideInInspector] public Vector2 Move;
    Vector3 MoveForce;
    Vector3 MoveDir;
    private Vector3 RollDirection;
    private Vector3 PreLandDirection;
    Animator PlayerAnimator;
    private Camera MainCamera;
    
    
    public bool IsGroundedThisFrame;




    private void Start()
    {
        MainCamera = Camera.main;
        rb = GetComponent<Rigidbody>();
       
        PlayerAnimator = GetComponent<Animator>();
        PlayerAnimator.applyRootMotion = false;
        
    }
    public bool MinMovMagnitude()
    { 
        return Move.sqrMagnitude > 0.01f;
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
        PlayerStateHandling.CurrentState = PlayerStateHandler.PlayerState.LandingRoll;
        RollDirection = PreLandDirection.normalized;
        RollDirSet = true;
        InitiateRollVelocity();

    }
  
    void RollIntentHandler()
    {
        if (InputHandling.WantsToRoll)
        {
            if (MinMovMagnitude())
                RollDirection = MoveDir.normalized;
            else
                RollDirection = transform.forward.normalized;

            RollDirSet = false;
        }

    }
    public void EndRoll()
    {
        InputHandling.WantsToRoll = false;
        PlayerStateHandling.CurrentState = PlayerStateHandler.PlayerState.Idling;
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
    void ClimbSnap()
    {
        // Only run when climbing
        if (PlayerStateHandling.CurrentState != PlayerStateHandler.PlayerState.Climbing)
        {
            ClimbingClimbable = null;
            HasSnappedToEntry = false;
            HasSnappedToExit = false;
            return;
        }

        // Store the climbable when we start climbing (for entry snap)
        if (ClimbingClimbable == null && TriggerHandler.CurrentClimbable != null)
        {
            ClimbingClimbable = TriggerHandler.CurrentClimbable;
        }

        // Entry snap
        if (!PlayerStateHandling.HasSnappedToEntry && ClimbingClimbable != null &&
            (ClimbingClimbable.Type == Climbable.ClimbType.TopEnter || ClimbingClimbable.Type == Climbable.ClimbType.BottomEnter))
        {
            Debug.Log("SUCCESS: Snapping to entry: " + ClimbingClimbable.Type);
            transform.position = ClimbingClimbable.SnapPoint.position;
            transform.rotation = ClimbingClimbable.SnapPoint.rotation;
            PlayerStateHandling.HasSnappedToEntry = true; // THIS IS THE CRITICAL LINE
        }

        // Debug the current trigger state while climbing
        if (TriggerHandler.CurrentClimbable != null)
        {
            Debug.Log("While climbing, current climbable is: " + TriggerHandler.CurrentClimbable.Type);
        }
        else
        {
            Debug.Log("While climbing, no current climbable");
        }

        // Exit snap (use CURRENT climbable - this detects new exit triggers while climbing)
        if (!HasSnappedToExit && TriggerHandler.CurrentClimbable != null &&
            (TriggerHandler.CurrentClimbable.Type == Climbable.ClimbType.TopExit || TriggerHandler.CurrentClimbable.Type == Climbable.ClimbType.BottomExit))
        {
            Debug.Log("SUCCESS: Snapping to exit: " + TriggerHandler.CurrentClimbable.Type);
            transform.position = TriggerHandler.CurrentClimbable.SnapPoint.position;
            transform.rotation = TriggerHandler.CurrentClimbable.SnapPoint.rotation;
            HasSnappedToExit = true;
        }
    }
    void ClimbHandler()
    {
        ClimbSnap();
        if (PlayerStateHandling.CurrentState == PlayerStateHandler.PlayerState.Climbing)
        {
            Climb(InputHandling.ClimbInput);

        }
    }
    public void Climb(float VerticalInput)
    {
        Vector3 ClimbDir = new Vector3(0, VerticalInput * ClimbSpeed, 0); 
        rb.linearVelocity = ClimbDir;
    }
    
    public void GravityHandler()
    {
        if (PlayerStateHandling.CurrentState != PlayerStateHandler.PlayerState.Climbing)
            rb.useGravity = true;
        
        else
            rb.useGravity = false;
        
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
        if (PlayerStateHandling.CurrentState == PlayerStateHandler.PlayerState.Sprinting)
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
        return !(Move.sqrMagnitude < 0.01f && IsGroundedThisFrame && 
            PlayerStateHandling.CurrentState != PlayerStateHandler.PlayerState.Rolling && 
            PlayerStateHandling.CurrentState != PlayerStateHandler.PlayerState.LandingRoll && 
            PlayerStateHandling.CurrentState != PlayerStateHandler.PlayerState.Jumping);
    }
    void SpeedKillerOnNoInput() 
    {
        if (PlayerStateHandling.CurrentState == PlayerStateHandler.PlayerState.Jumping)
            return;    
        
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
        if (PlayerStateHandling.CurrentState == PlayerStateHandler.PlayerState.Rolling || PlayerStateHandling.CurrentState == PlayerStateHandler.PlayerState.LandingRoll)
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
        if (PlayerStateHandling.CurrentState == PlayerStateHandler.PlayerState.Rolling 
            || PlayerStateHandling.CurrentState == PlayerStateHandler.PlayerState.LandingRoll
            || PlayerStateHandling.CurrentState == PlayerStateHandler.PlayerState.Climbing) 
            return;
        
        
        else if (MinMovMagnitude() && IsGroundedThisFrame && PlayerStateHandling.CurrentState != PlayerStateHandler.PlayerState.LandingRoll)
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
           // PlayerStateHandling.CurrentState = PlayerStateHandler.PlayerState.Jumping;
            //PlayerAnimator.SetTrigger("Jump");
            InputHandling.WantsToJump = false;
        }
        return;
    }
    private void Update()
    {
        VerticalVelocity = VerticalVelocityCalc();
        IsGroundedThisFrame = GroundedCheck();
        RollIntentHandler();
        

    }
    private void FixedUpdate()
    {
        GravityHandler();
        
        RollDirectionHandler();
        RollHandler();
        CameraHandler();
        ClimbHandler();
        SprintAndWalkSpeedSwitcher();
        JumpHandler();
        MovementHandler();
        SpeedKillerOnNoInput();




    }

}