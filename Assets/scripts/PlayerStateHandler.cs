using Unity.VisualScripting;
using UnityEngine;

public class PlayerStateHandler : MonoBehaviour
{
    
    [SerializeField] private PlayerMovement PlayerMovement;
    [SerializeField] private InputHandler InputHandling;
    [SerializeField] private TriggerHandling TriggerHandler; 
    [SerializeField] private ResourceHandler ResourceHandling;

    
    public enum PlayerState { Idling, Walking, Sprinting, Jumping, Rolling, Falling, Combat, Climbing, LandingRoll, Rest };
    public PlayerState CurrentState = PlayerState.Idling;

    
    [HideInInspector] public bool CanExitCombat = false;
    [HideInInspector] public bool HasSnappedToEntry = false;
    [HideInInspector] public bool CanRegenStamina = true;
    [HideInInspector] public bool StaminaDrainActive = false;

    void Update()
    {
        switch (CurrentState)
        {
            case PlayerState.Idling:
                StaminaDrainActive = false;
                CanRegenStamina = true;
                HandleIdleState();
                break;
            case PlayerState.Walking:
                StaminaDrainActive = false;
                CanRegenStamina = true;
                HandleWalkingState();
                break;
            case PlayerState.Sprinting:
                StaminaDrainActive = true;
                CanRegenStamina = false;
                HandleSprintingState();
                break;
            case PlayerState.Climbing:
                StaminaDrainActive = false;
                CanRegenStamina = true;
                HandleClimbingState();
                break;
            case PlayerState.Combat:
                CanRegenStamina = false;
                HandleCombatState();
                break;
            case PlayerState.LandingRoll:
                StaminaDrainActive = false;
                CanRegenStamina = true;
                HandleLandingRollState();
                break;
            case PlayerState.Rest:
                CanRegenStamina = true;
                StaminaDrainActive = false;
                HandleRestState();
                break;
                
        }
    }

   
    private void HandleRestState()
    {
        if (!InputHandling.WantsToExitBonfire)
        {
            Debug.Log("Bazinga 1");
            return;
        }
        else
        {
            Debug.Log("Bazinga 2");
            InputHandling.WantsToExitBonfire = false;
            InputHandling.WantsToUseBonfire = false;
            Debug.Log("Bazinga 3");
            TransitionTo(PlayerState.Idling);
            Debug.Log("Bazinga 4");
            Debug.Log("Bazinga: " + CurrentState);        
        }
    }
    private void HandleIdleState()
    {
        if (InputHandling.WantsToUseBonfire) {TransitionTo(PlayerState.Rest); return; }
            if (CheckForClimbTransition()) return;
        if (InputHandling.CombatInput != InputHandler.PlayerCombatInput.None) { TransitionTo(PlayerState.Combat); return; }
        if (InputHandling.WantsToJump) { TransitionTo(PlayerState.Jumping); return; }
        if (InputHandling.WantsToRoll) { TransitionTo(PlayerState.Rolling); return; }
        if (InputHandling.WantsToSprint && InputHandling.WantsToWalk && ResourceHandling.CanSpendStamina(ResourceHandling.Stamina)) { TransitionTo(PlayerState.Sprinting); return; }
        if (InputHandling.WantsToWalk) { TransitionTo(PlayerState.Walking); return; }
    }

    private void HandleWalkingState()
    {
        if (InputHandling.WantsToUseBonfire){ TransitionTo(PlayerState.Rest); return; }
        if (CheckForClimbTransition()) return; 
        if (InputHandling.CombatInput != InputHandler.PlayerCombatInput.None) { TransitionTo(PlayerState.Combat); return; }
        if (InputHandling.WantsToJump) { TransitionTo(PlayerState.Jumping); return; }
        if (InputHandling.WantsToRoll) { TransitionTo(PlayerState.Rolling); return; }
        if (InputHandling.WantsToSprint && ResourceHandling.CanSpendStamina(ResourceHandling.Stamina)) { TransitionTo(PlayerState.Sprinting); return; }
        if (!InputHandling.WantsToWalk) { TransitionTo(PlayerState.Idling); return; }
    }

    private void HandleSprintingState()
    {
        if (InputHandling.WantsToUseBonfire) {TransitionTo(PlayerState.Rest); return; }
        if (!InputHandling.WantsToSprint || !InputHandling.WantsToWalk || !ResourceHandling.CanSpendStamina(ResourceHandling.Stamina)) { TransitionTo(PlayerState.Walking); return; }
        if (InputHandling.WantsToRoll) { TransitionTo(PlayerState.Rolling); return; }
    }

    private void HandleClimbingState()
    {
        
        if (HasSnappedToEntry && TriggerHandler.CurrentClimbingCheck == null)
        {
            TransitionTo(PlayerState.Idling);
            
        }
    }

    private void HandleCombatState()
    {
        if (CanExitCombat)
        {
            CanExitCombat = false; 
            TransitionTo(PlayerState.Idling);
        }
    }

    private void HandleLandingRollState()
    {
        
    }

    

    private bool CheckForClimbTransition()
    {
        if (InputHandling.WantsToClimb)
        {
            Debug.Log("HandleState sees WantsToClimb=true. Transitioning to Climbing.");
            InputHandling.ZeroOutClimbInput();
            InputHandling.WantsToClimb = false; 
            HasSnappedToEntry = false;          
            TransitionTo(PlayerState.Climbing);
            return true;
        }
        return false;
    }

    private void TransitionTo(PlayerState newState)
    {
        Debug.Log($"Transitioning from {CurrentState} to {newState}");
        CurrentState = newState;
    }
}
