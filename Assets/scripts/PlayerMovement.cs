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
    bool IsSprinting = false;
    bool IsGrounded = true;
    bool IsRolling = false;
    bool RollDirSet = false;
    bool IsFloating = false;
    bool HasJumped = false;
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
    void OnMove(InputValue value)
    {
        
        Move = value.Get<Vector2>();
        
    }
    void OnJump()
    {
        if (IsRolling||!IsGrounded)
            return;

        if (IsGrounded)
        {
            rb.AddForce(Vector3.up * JumpForce);
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

        if (IsRolling || !IsGrounded) return;


        // Set roll speed multiplier
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
        if (IsLandingRoll) RollDirection = transform.forward.normalized;
        else
        {
            if (MoveDir.sqrMagnitude > 0.1f)
            {
                RollDirection = MoveDir.normalized;
            }
            else
            {
                RollDirection = transform.forward.normalized;
            }
        }
       
        IsRolling = true;
        RollDirSet = false;

        // Set direction ONCE!

        if (!IsLandingRoll)
        {
            PlayerAnimator.SetFloat("RollAnimationSpeed", RollAnimationSpeed);
            PlayerAnimator.SetTrigger("Roll");
        }
        
    }

    public void EndRoll()
    {
        IsLandingRoll = false;
        IsRolling = false;
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
        if (IsPressed&&!IsRolling && IsGrounded) IsSprinting = true;
        else if (!IsPressed) IsSprinting = false;
    }
    

    private void Update()
    {
        
    }
    private void FixedUpdate()
    {
        IsGrounded = Physics.Raycast(transform.position + Vector3.up * 0.1f, Vector3.down, 0.45f);
        if(IsGrounded ) PlayerAnimator.SetBool("IsGrounded",true);
        else PlayerAnimator.SetBool("IsGrounded", false);
        //animation handling and all that bollocks
        if (!IsSprinting && IsGrounded && !IsRolling && Move.sqrMagnitude > 0.01f) PlayerAnimator.SetBool("IsWalking", true);
        else PlayerAnimator.SetBool("IsWalking", false);
        if (IsSprinting && IsGrounded && !IsRolling && Move.sqrMagnitude > 0.01f) PlayerAnimator.SetBool("IsSprinting", true);
        else PlayerAnimator.SetBool("IsSprinting", false);
        float VerticalVelocity = rb.linearVelocity.y;
        if (Mathf.Abs(VerticalVelocity) < 0.05f) VerticalVelocity = 0f;
        PlayerAnimator.SetFloat("VelocityY", VerticalVelocity);

        if (IsSprinting && (IsRolling || !IsGrounded||Move.sqrMagnitude < 0.01f)) IsSprinting = false;
        if (IsSprinting) CurrSpeed = MoveSpeed + SprintSpeedAddition;
        else CurrSpeed = MoveSpeed;
        float debug = rb.linearVelocity.y;
       
        Debug.Log("Vertical Velocity: "+VerticalVelocity);
        
        

        
        

        Vector3 CameraForward = MainCamera.transform.forward;
        Vector3 CameraRight = MainCamera.transform.right;
        CameraForward.y = 0;
        CameraRight.y = 0;
        CameraForward.Normalize();
        CameraRight.Normalize();
        MoveDir = (CameraForward * Move.y + CameraRight * Move.x).normalized;

        // Snap player to stop if no input is pressed and not rolling
    
        if (Move.sqrMagnitude < 0.01f && IsGrounded && !IsRolling)
        {
            rb.linearVelocity = new Vector3(0f, rb.linearVelocity.y, 0f);
            return;
        }
        if (!IsGrounded)
        {
            PreLandDirection = MoveDir.sqrMagnitude > 0.1f
                ? MoveDir.normalized : transform.forward;
        }
        if (IsRolling)
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
