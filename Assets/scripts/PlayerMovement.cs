using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{


    Rigidbody rb;
    public float MoveSpeed;
    public float JumpForce;
    public float SprintSpeedAddition;
    public float RollSpeed;
    public float RollAnimationSpeed;
    float CurrSpeed;
    bool IsGrounded = true;
    
    bool RollDirSet = false;
    bool IsFloating = false;
    bool HasJumped = false;
    bool ShowDebug = false;
    Vector2 Move;
    Vector3 MoveForce;
    Vector3 MoveDir;
    Vector3 TargetDir;
    private Vector3 RollDirection;
    private Vector3 PreLandDirection;
    float zero = 0;
    Animator PlayerAnimator;
    private Camera MainCamera;
    public enum RollType { Light, Medium, Heavy, Over };
    public RollType CurrentRollType;
    enum PlayerState {Idling, Walking, Sprinting, Jumping, Rolling, Falling, LandingRoll };
    PlayerState CurrentState = PlayerState.Idling;
    bool IsLandingRoll = false;
    


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
    void OnGUI()
    {
        if (!ShowDebug)
            return;
        float y = 10f;
        float LineHeight = 20f;

        //display animator bools
        GUI.Label(new Rect(10, y, 300, LineHeight), "IsGrounded: " + PlayerAnimator.GetBool("IsGrounded"));
        y += LineHeight;
        GUI.Label(new Rect(10, y, 300, LineHeight), "IsWalking: " + PlayerAnimator.GetBool("IsWalking"));
        y += LineHeight;
        GUI.Label(new Rect(10, y, 300, LineHeight), "IsSprinting: " + PlayerAnimator.GetBool("IsSprinting"));
        y += LineHeight;

        //display animator floats
        GUI.Label(new Rect(10, y, 300, LineHeight), "VelocityY: " + PlayerAnimator.GetFloat("VelocityY"));
        y += LineHeight;
        //other
        GUI.Label(new Rect(10, y, 300, LineHeight), "Player State: " + CurrentState);
        y += LineHeight;
        GUI.Label(new Rect(10, y, 300, LineHeight), "Player State: " + CurrentState);
    }
    void OnDebugger()
    {
        
        ShowDebug = !ShowDebug;
        
    }
    void IdleCheck()
    {
        //is the player idle?
        if (IsGrounded && Move.sqrMagnitude < 0.1f && CurrentState != PlayerState.Rolling && CurrentState != PlayerState.Jumping && CurrentState != PlayerState.LandingRoll)
            CurrentState = PlayerState.Idling;

    }
    float VerticalVelocityCalc()
    {
        float VerticalVelocity = rb.linearVelocity.y;
        if (Mathf.Abs(VerticalVelocity) < 0.05f)
            VerticalVelocity = 0f;
        return VerticalVelocity;
    }
    void AnimationHandler()
    {
        
        //is the player in air?
        if (IsGrounded) 
            PlayerAnimator.SetBool("IsGrounded", true);
        else 
            PlayerAnimator.SetBool("IsGrounded", false);
        //walk and sprint handling
        if (CurrentState == PlayerState.Walking ) 
            PlayerAnimator.SetBool("IsWalking", true);
        else 
            PlayerAnimator.SetBool("IsWalking", false);

        if (CurrentState  == PlayerState.Sprinting)  
            PlayerAnimator.SetBool("IsSprinting", true);
        else 
            PlayerAnimator.SetBool("IsSprinting", false);
        //Vertical Velocity calculations and passing
        

        PlayerAnimator.SetFloat("VelocityY", VerticalVelocityCalc());

        //roll fire handling
        if(CurrentState == PlayerState.Rolling)
        {
            PlayerAnimator.SetTrigger("Roll");
        }
        //roll parameter handling
        if (!IsLandingRoll)
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
                        RollSpeed = 2f;
                        RollAnimationSpeed = 0.4f; break;
                    }
            }
        }
        else RollSpeed = 4f;
        PlayerAnimator.SetFloat("RollAnimationSpeed", RollAnimationSpeed);
    }
    void OnMove(InputValue value)
    {
        
        Move = value.Get<Vector2>();
        if (CurrentState != PlayerState.Rolling && IsGrounded && CurrentState != PlayerState.Sprinting && CurrentState != PlayerState.Jumping && Move.sqrMagnitude > 0.01f)
            CurrentState = PlayerState.Walking;
    }
    void OnJump()
    {
        if (CurrentState == PlayerState.Rolling || !IsGrounded)
            return;

        if (IsGrounded)
        {
            rb.AddForce(Vector3.up * JumpForce);
            CurrentState = PlayerState.Jumping;
            PlayerAnimator.SetTrigger("Jump");

            HasJumped = true;
            IsGrounded = false;
        }
        return;
    }
    public void StartLandingRoll()
    {
        IsLandingRoll = true;
        RollDirection = PreLandDirection;
        OnRoll();
    }
    void OnRoll()
    {

        if (CurrentState == PlayerState.Rolling || !IsGrounded) return;

            if (MoveDir.sqrMagnitude > 0.1f)
                 RollDirection = MoveDir.normalized;
            else
                 RollDirection = transform.forward.normalized;
           
        
        RollDirSet = false;
        if (!IsLandingRoll)
            CurrentState = PlayerState.Rolling;          
    }

    public void EndRoll()
    {
        IsLandingRoll = false;

        CurrentState = PlayerState.Idling;
        
        RollDirSet = false;
        
    }
    public void MidRollSpeedVariance()
    {
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
    }
  
    void OnSprint(InputValue value) 
    {
        bool IsPressed = value.isPressed;
        if (IsPressed && CurrentState != PlayerState.Rolling && IsGrounded && Move.sqrMagnitude > 0.01f)
            CurrentState = PlayerState.Sprinting;
        else
            CurrentState = PlayerState.Idling;
        
    }
    

    private void Update()
    {
        IdleCheck();
        AnimationHandler();
        
    }
    private void FixedUpdate()
    {
        IsGrounded = Physics.Raycast(transform.position + Vector3.up * 0.1f, Vector3.down, 0.45f);
        
       
        
        

        
        if (CurrentState ==PlayerState.Sprinting) CurrSpeed = MoveSpeed + SprintSpeedAddition;
        else CurrSpeed = MoveSpeed;
        Vector3 CameraForward = MainCamera.transform.forward;
        Vector3 CameraRight = MainCamera.transform.right;
        CameraForward.y = 0;
        CameraRight.y = 0;
        CameraForward.Normalize();
        CameraRight.Normalize();
        MoveDir = (CameraForward * Move.y + CameraRight * Move.x).normalized;

        // Snap player to stop if no input is pressed and not rolling
    
        if (Move.sqrMagnitude < 0.01f && IsGrounded && CurrentState != PlayerState.Rolling)
        {
            rb.linearVelocity = new Vector3(0f, rb.linearVelocity.y, 0f);
            return;
        }
        if (!IsGrounded)
        {
            PreLandDirection = MoveDir.sqrMagnitude > 0.1f
                ? MoveDir.normalized : transform.forward;
        }
        if (CurrentState == PlayerState.Rolling)
        {

            if (!RollDirSet)
            {
                
                RollDirSet = true;
            }
            rb.linearVelocity = new Vector3(RollDirection.x * RollSpeed, rb.linearVelocity.y, RollDirection.z * RollSpeed);
            return;
        }

        if (MoveDir.sqrMagnitude > 0.00001f && IsGrounded&&!IsLandingRoll)
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
    
}
