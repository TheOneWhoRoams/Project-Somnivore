using UnityEngine;
using UnityEngine.InputSystem;
using static PlayerMovement;



public class InputHandler : MonoBehaviour
{
    [SerializeField] private PlayerMovement PlayerMovement;
    Rigidbody rb;
    
    [HideInInspector] public bool WantsToJump;
    [HideInInspector] public bool WantsToRoll;
    [HideInInspector] public bool WantsToWalk;
    [HideInInspector] public bool WantsToSprint;

    void OnJump()
    {
        if (PlayerMovement.CurrentState == PlayerState.Rolling || !PlayerMovement.IsGroundedThisFrame)
            return;

        WantsToJump = true;

        /*if (PlayerMovement.IsGroundedThisFrame)
        {
            rb.AddForce(Vector3.up * JumpForce);
            PlayerMovement.CurrentState = PlayerState.Jumping;
            PlayerAnimator.SetTrigger("Jump");
        }
        return;*/
    }
    void OnRoll()
    {
        if (PlayerMovement.CurrentState == PlayerMovement.PlayerState.Rolling || !PlayerMovement.IsGroundedThisFrame || PlayerMovement.CurrentState == PlayerMovement.PlayerState.LandingRoll) return;
        PlayerMovement.WantsToRoll = true;
    }
    void OnSprint(InputValue value)
    {
        bool IsPressed = value.isPressed;
        if (IsPressed && PlayerMovement.CurrentState != PlayerState.Rolling && PlayerMovement.IsGroundedThisFrame && PlayerMovement.MinMovMagnitude())
            PlayerMovement.CurrentState = PlayerState.Sprinting;
        else
            PlayerMovement.CurrentState = PlayerState.Idling;

    }
    void OnDebugger()
    {

        PlayerMovement.ShowDebug = !PlayerMovement.ShowDebug;

    }
    bool CanMove()
    {
        return PlayerMovement.CurrentState != PlayerState.Rolling && PlayerMovement.IsGroundedThisFrame && PlayerMovement.CurrentState != PlayerState.Sprinting && PlayerMovement.CurrentState != PlayerState.Jumping && PlayerMovement.MinMovMagnitude();
    }
    void OnMove(InputValue value)
    {

        PlayerMovement.Move = value.Get<Vector2>();
        if (CanMove())
            PlayerMovement.CurrentState = PlayerState.Walking;
    }
    //[SerializeField] private PhysicsHandler PhysicsHandling;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb= GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
