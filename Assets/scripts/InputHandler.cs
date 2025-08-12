using UnityEngine;
using UnityEngine.InputSystem;
using static PlayerMovement;



public class InputHandler : MonoBehaviour
{
    [SerializeField] private PlayerMovement PlayerMovement;
    [SerializeField] private PlayerStateHandler PlayerStateHandling;
    [SerializeField] private AnimationHandler AnimationHandling;
    Rigidbody rb;
    
    [HideInInspector] public bool WantsToJump;
    [HideInInspector] public bool WantsToRoll;
    [HideInInspector] public bool WantsToWalk;
    [HideInInspector] public bool WantsToSprint;


    void OnJump()
    {
        if (PlayerStateHandling.CurrentState == PlayerStateHandler.PlayerState.Rolling || !PlayerMovement.IsGroundedThisFrame)
            return;
        WantsToJump = true;
        AnimationHandling.PlayJump();
    }
    void OnRoll()
    {
        if (PlayerStateHandling.CurrentState == PlayerStateHandler.PlayerState.Rolling || !PlayerMovement.IsGroundedThisFrame || PlayerStateHandling.CurrentState == PlayerStateHandler.PlayerState.LandingRoll) 
            return;
        WantsToRoll = true;
        AnimationHandling.PlayRoll();
    }
    void OnSprint(InputValue value)
    {
        bool IsPressed = value.isPressed;
        if (IsPressed&&PlayerStateHandling.CurrentState != PlayerStateHandler.PlayerState.Rolling && PlayerMovement.IsGroundedThisFrame && PlayerMovement.MinMovMagnitude())
            WantsToSprint = true;
        else
            WantsToSprint = false;
        Debug.Log("sprint pressed? " + WantsToSprint);
    }
    void SprintStopOnReleasingKey()
    {
        if (!PlayerMovement.IsGroundedThisFrame || !PlayerMovement.MinMovMagnitude())
        {
            WantsToSprint = false;
        }
    }
    void OnDebugger()
    {

        PlayerMovement.ShowDebug = !PlayerMovement.ShowDebug;

    }
    bool CanMove()
    {
        return PlayerStateHandling.CurrentState != PlayerStateHandler.PlayerState.Rolling && PlayerMovement.IsGroundedThisFrame &&
            PlayerStateHandling.CurrentState != PlayerStateHandler.PlayerState.Sprinting && 
            PlayerStateHandling.CurrentState != PlayerStateHandler.PlayerState.Jumping && PlayerMovement.MinMovMagnitude();
    }
    void OnMove(InputValue value)
    {
        
        PlayerMovement.Move = value.Get<Vector2>();

        WantsToWalk = PlayerMovement.MinMovMagnitude();
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
        SprintStopOnReleasingKey();
    }
}
