using UnityEngine;

public class PlayerStateHandler : MonoBehaviour
{
    // --- REFERENCES ---
    [SerializeField] private PlayerMovement PlayerMovement;
    [SerializeField] private InputHandler InputHandling;
    [SerializeField] private TriggerHandling TriggerHandler; // Added for climb logic
    [SerializeField] private ResourceHandler ResourceHandling;

    // --- STATE MANAGEMENT ---
    public enum PlayerState { Idling, Walking, Sprinting, Jumping, Rolling, Falling, Combat, Climbing, LandingRoll };
    public PlayerState CurrentState = PlayerState.Idling;

    // --- FLAGS ---
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
                // Other states like Jumping, Rolling would be handled here
        }
    }

    // --- STATE HANDLERS ---

    private void HandleIdleState()
    {
        // Check for transitions OUT of Idling
        if (CheckForClimbTransition()) return;
        if (InputHandling.CombatInput != InputHandler.PlayerCombatInput.None) { TransitionTo(PlayerState.Combat); return; }
        if (InputHandling.WantsToJump) { TransitionTo(PlayerState.Jumping); return; }
        if (InputHandling.WantsToRoll) { TransitionTo(PlayerState.Rolling); return; }
        if (InputHandling.WantsToSprint && InputHandling.WantsToWalk && ResourceHandling.CanSpendStamina(ResourceHandling.Stamina)) { TransitionTo(PlayerState.Sprinting); return; }
        if (InputHandling.WantsToWalk) { TransitionTo(PlayerState.Walking); return; }
    }

    private void HandleWalkingState()
    {
        // Check for transitions OUT of Walking
        if (CheckForClimbTransition()) return; // BUG FIX: Added climb check to Walking
        if (InputHandling.CombatInput != InputHandler.PlayerCombatInput.None) { TransitionTo(PlayerState.Combat); return; }
        if (InputHandling.WantsToJump) { TransitionTo(PlayerState.Jumping); return; }
        if (InputHandling.WantsToRoll) { TransitionTo(PlayerState.Rolling); return; }
        if (InputHandling.WantsToSprint && ResourceHandling.CanSpendStamina(ResourceHandling.Stamina)) { TransitionTo(PlayerState.Sprinting); return; }
        if (!InputHandling.WantsToWalk) { TransitionTo(PlayerState.Idling); return; }
    }

    private void HandleSprintingState()
    {
        if (!InputHandling.WantsToSprint || !InputHandling.WantsToWalk || !ResourceHandling.CanSpendStamina(ResourceHandling.Stamina)) { TransitionTo(PlayerState.Walking); return; }
        if (InputHandling.WantsToRoll) { TransitionTo(PlayerState.Rolling); return; }
    }

    private void HandleClimbingState()
    {
        // Exit condition is gated by the snap check
        if (HasSnappedToEntry && TriggerHandler.CurrentClimbingCheck == null)
        {
            TransitionTo(PlayerState.Idling);
            
        }
    }

    private void HandleCombatState()
    {
        if (CanExitCombat)
        {
            CanExitCombat = false; // Consume the flag
            TransitionTo(PlayerState.Idling);
        }
    }

    private void HandleLandingRollState()
    {
        // This state is exited by an animation event that calls EndRoll() in PlayerMovement.
        // EndRoll() then sets the state back to Idling.
        // Therefore, no transition logic is needed here.
    }

    // --- TRANSITION LOGIC ---

    private bool CheckForClimbTransition()
    {
        if (InputHandling.WantsToClimb)
        {
            Debug.Log("HandleState sees WantsToClimb=true. Transitioning to Climbing.");
            InputHandling.ZeroOutClimbInput();
            InputHandling.WantsToClimb = false; // Consume the flag
            HasSnappedToEntry = false;          // Reset for the new climb
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
