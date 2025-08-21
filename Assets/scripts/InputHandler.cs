using UnityEngine;
using UnityEngine.InputSystem;
using static PlayerMovement;



public class InputHandler : MonoBehaviour
{
    [SerializeField] private TriggerHandling TriggerHandler;
    [SerializeField] private PlayerMovement PlayerMovement;
    [SerializeField] private PlayerStateHandler PlayerStateHandling;
    [SerializeField] private AnimationHandler AnimationHandling;
    Rigidbody rb;
    [HideInInspector] public bool WantsToClimb;
    [HideInInspector] public bool WantsToJump;
    [HideInInspector] public bool WantsToRoll;
    [HideInInspector] public bool WantsToWalk;
    [HideInInspector] public bool WantsToSprint;
    [HideInInspector] public float ClimbInput;
    

    bool CanClimb()
    {
        return PlayerStateHandling.CurrentState != PlayerStateHandler.PlayerState.Rolling && PlayerMovement.IsGroundedThisFrame &&
               PlayerStateHandling.CurrentState != PlayerStateHandler.PlayerState.Jumping&& (TriggerHandler.CurrentClimbable.Type==Climbable.ClimbType.TopEnter||
               TriggerHandler.CurrentClimbable.Type == Climbable.ClimbType.BottomEnter)&&TriggerHandler.CurrentClimbable.IsInEntryZone;
    }
    void OnInteract()
    {
        if (CanClimb())
        {
            
            WantsToClimb = true;
        }
    }
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
        Vector2 input = value.Get<Vector2>();
        if(PlayerStateHandling.CurrentState!=PlayerStateHandler.PlayerState.Climbing)
        {
            ClimbInput = 0;
            PlayerMovement.Move = input;
            WantsToWalk = PlayerMovement.MinMovMagnitude();
            
        }
        else
        {
            ClimbInput = input.y;
        }
        
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
