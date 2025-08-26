using Unity.VisualScripting;
using UnityEngine;

public class PlayerStateHandler : MonoBehaviour
{
    [SerializeField] private TriggerHandling TriggerHandler;
    [SerializeField] private PlayerMovement PlayerMovement;
    [SerializeField] private InputHandler InputHandling;


    [HideInInspector] public enum PlayerState { Idling, Walking, Sprinting, Jumping, Rolling, Falling, LandingRoll, Climbing, ClimbExit, Combat };
    [HideInInspector] public PlayerState CurrentState = PlayerState.Idling;
    [HideInInspector] public bool HasSnappedToEntry = true;
    [HideInInspector] public bool HasSnappedToExit = true;

     bool CombatCheck()
    {
        
        if (CurrentState == PlayerState.Combat)
            return true;
        else
            return false;
        
        
    }
    void CombatState()
    {
        if (InputHandling.WantsToLightAttack/* or any other combat action */)
        {
            CurrentState = PlayerState.Combat;
        }
    }
    public void ClimbExit()
    {
        
        if ((TriggerHandler.CurrentClimbingCheck == null && CurrentState == PlayerState.Climbing && PlayerMovement.HasSnappedToEntry)/*||TriggerHandler.CurrentClimbable.WantsToExitClimb*/)
        {
            HasSnappedToEntry = false;
            HasSnappedToExit = false;
            CurrentState = PlayerState.Idling;
            PlayerMovement.HasSnappedToEntry = false;
        }
    }
    void ClimbState()
    {

        if (CombatCheck())
            return;

        ClimbExit();

        if (CurrentState == PlayerState.Climbing && TriggerHandler.CurrentClimbingCheck == null&&PlayerMovement.HasSnappedToEntry)
        {
            CurrentState = PlayerState.Idling;
        }
        else if (InputHandling.WantsToClimb)
        {
            InputHandling.WantsToClimb = false;
            CurrentState = PlayerState.Climbing;
        }/*
        else
        {
            CurrentState = PlayerState.Idling;
        }*/
    }
    void SprintState()
    {

        if (CombatCheck())
            return;

        if (InputHandling.WantsToSprint&&InputHandling.WantsToWalk && CurrentState != PlayerState.Climbing)
            CurrentState = PlayerState.Sprinting;
    }
    void WalkState()
    {

        if (CombatCheck())
            return;

        if (InputHandling.WantsToWalk&&!InputHandling.WantsToSprint && CurrentState != PlayerState.Climbing)
            CurrentState = PlayerState.Walking;
    }
    void RollState()
    {

        if (CombatCheck())
            return;

        if (InputHandling.WantsToRoll && CurrentState != PlayerState.Climbing)
            CurrentState = PlayerState.Rolling;
    }
    void JumpState()
    {

        if (CombatCheck())
            return;

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

        if (CombatCheck())
            return;

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
        CombatState();
        ClimbState();
        SprintState();
        WalkState();
        JumpState();
        RollState();
        IdleCheck();
    }
}
