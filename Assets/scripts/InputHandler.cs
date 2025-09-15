using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    // --- REFERENCES ---
    [SerializeField] private TriggerHandling TriggerHandler;
    [SerializeField] private PlayerStateHandler PlayerStateHandling;
    [SerializeField] private AnimationHandler AnimationHandling;
    [SerializeField] private PlayerMovement PlayerMovement; 
    [SerializeField] private ResourceHandler ResourceHandling; 

    // --- INPUT FLAGS ---
    [HideInInspector] public bool WantsToClimb;
    [HideInInspector] public bool WantsToJump;
    [HideInInspector] public bool WantsToRoll;
    [HideInInspector] public bool WantsToWalk;
    [HideInInspector] public bool WantsToSprint;
    [HideInInspector] public bool WantsToUseBonfire;
    [HideInInspector] public bool WantsToExitBonfire;
    // CORRECTED: Restored missing combat inputs
    public enum PlayerCombatInput { None, WantsToLightAttack, WantsToHeavyAttack, WantsToBlockAttack, WantsToParryAttack }
    public PlayerCombatInput CombatInput = PlayerCombatInput.None;

    // --- INPUT VALUES ---
    [HideInInspector] public float ClimbInput;

    // --- INPUT SYSTEM EVENTS ---
    
   public void ZeroOutClimbInput()
    {
        ClimbInput = 0;
    }
    void OnMove(InputValue value)
    {
        Vector2 input = value.Get<Vector2>();
        if (PlayerStateHandling.CurrentState != PlayerStateHandler.PlayerState.Climbing)
        {
            PlayerMovement.Move = input;
            WantsToWalk = PlayerMovement.MinMovMagnitude();
        }
        else
        {
            ClimbInput = input.y;
        }
    }

    void OnInteract()
    {
        if (WantsToUseBonfire)
        {
            WantsToExitBonfire = true;
        }
        else if (CanRestAtBonfire())
        {
            Debug.Log("Eeepy");
            WantsToUseBonfire = true;
        }
        else if (CanClimb())
        {
            
            WantsToClimb = true;
        }    
    }

    // Names like OnLightAttack, OnHeavyAttack, OnBlock, OnParry should match your Player Input Action Map
    void OnLightAttack()
    {
        if (CanPerformCombatAction())
        {
            CombatInput = PlayerCombatInput.WantsToLightAttack;
        }
    }

    void OnHeavyAttack()
    {
        if (CanPerformCombatAction())
        {
            CombatInput = PlayerCombatInput.WantsToHeavyAttack;
        }
    }

    void OnBlock()
    {
        if (CanPerformCombatAction())
        {
            CombatInput = PlayerCombatInput.WantsToBlockAttack;
        }
    }

    void OnParry()
    {
        if (CanPerformCombatAction())
        {
            CombatInput = PlayerCombatInput.WantsToParryAttack;
        }
    }


    void OnJump()
    {
        if (PlayerStateHandling.CurrentState == PlayerStateHandler.PlayerState.Rolling || !PlayerMovement.IsGroundedThisFrame || PlayerStateHandling.CurrentState == PlayerStateHandler.PlayerState.Rest) return;
        WantsToJump = true;
        AnimationHandling.PlayJump();
    }

    void OnRoll()
    {
        if (PlayerStateHandling.CurrentState == PlayerStateHandler.PlayerState.Rolling || !PlayerMovement.IsGroundedThisFrame || PlayerStateHandling.CurrentState == PlayerStateHandler.PlayerState.Rest) return;
        WantsToRoll = true;
        AnimationHandling.PlayRoll();
    }

    void OnSprint(InputValue value)
    {
        
            WantsToSprint = value.isPressed;
    }

    // CORRECTED: Added the missing OnDebugger method
    void OnDebugger()
    {
        PlayerMovement.ShowDebug = !PlayerMovement.ShowDebug;
    }


    // --- CONDITION CHECKS ---
    private bool CanRestAtBonfire()
    {
        return TriggerHandler.InBonfireRange && PlayerMovement.IsGroundedThisFrame && 
            PlayerStateHandling.CurrentState != PlayerStateHandler.PlayerState.Rolling && 
            PlayerStateHandling.CurrentState != PlayerStateHandler.PlayerState.Combat &&
            !WantsToUseBonfire; 
    }
    private bool CanClimb()
    {
        // Log each individual condition to find the exact point of failure.
        

        return TriggerHandler.CurrentClimbable != null &&
               PlayerStateHandling.CurrentState != PlayerStateHandler.PlayerState.Rolling &&
               PlayerStateHandling.CurrentState != PlayerStateHandler.PlayerState.Jumping &&
               (TriggerHandler.CurrentClimbable.Type == Climbable.ClimbType.TopEnter || TriggerHandler.CurrentClimbable.Type == Climbable.ClimbType.BottomEnter) &&
               TriggerHandler.CurrentClimbable.IsInEntryZone;
    }

    private bool CanPerformCombatAction()
    {
        return PlayerMovement.IsGroundedThisFrame && ResourceHandling.CanSpendStamina(ResourceHandling.Stamina) &&
               PlayerStateHandling.CurrentState != PlayerStateHandler.PlayerState.Rolling &&
               PlayerStateHandling.CurrentState != PlayerStateHandler.PlayerState.Jumping &&
               PlayerStateHandling.CurrentState != PlayerStateHandler.PlayerState.Climbing;
    }
}

