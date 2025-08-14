using Unity.VisualScripting;
using UnityEngine;

public class PlayerStateHandler : MonoBehaviour
{
    [SerializeField] private PlayerMovement PlayerMovement;
    [SerializeField] private InputHandler InputHandling;
    [HideInInspector] public enum PlayerState { Idling, Walking, Sprinting, Jumping, Rolling, Falling, LandingRoll, Climbing };
    [HideInInspector] public PlayerState CurrentState = PlayerState.Idling;

    void ClimbState()
    {
        if (InputHandling.WantsToClimb)
        {
            CurrentState = PlayerState.Climbing;
            
        }
    }
    void SprintState()
    {
        if(InputHandling.WantsToSprint&&InputHandling.WantsToWalk && CurrentState != PlayerState.Climbing)
            CurrentState = PlayerState.Sprinting;
    }
    void WalkState()
    {
        if(InputHandling.WantsToWalk&&!InputHandling.WantsToSprint && CurrentState != PlayerState.Climbing)
            CurrentState = PlayerState.Walking;
    }
    void RollState()
    {
        if(InputHandling.WantsToRoll && CurrentState != PlayerState.Climbing)
            CurrentState = PlayerState.Rolling;
    }
    void JumpState()
    {
        if (PlayerMovement.IsGroundedThisFrame && InputHandling.WantsToJump && CurrentState != PlayerState.Climbing)
            CurrentState = PlayerState.Jumping;    
    }
    bool IdleRequirements()
    {

        return PlayerMovement.IsGroundedThisFrame && PlayerMovement.Move.sqrMagnitude < 0.1f
                && CurrentState != PlayerState.Rolling && CurrentState != PlayerState.Jumping 
                && CurrentState != PlayerState.LandingRoll && CurrentState!=PlayerState.Climbing;
    }
    void IdleCheck()
    {
        //is the player idle?
        if (IdleRequirements())
            CurrentState = PlayerState.Idling;

    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ClimbState();
        SprintState();
        WalkState();
        JumpState();
        RollState();
        IdleCheck();
    }
}
