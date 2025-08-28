using UnityEngine;
using static InputHandler;

public class CombatStateHandler : MonoBehaviour
{

    [SerializeField] PlayerStateHandler PlayerStateHandling;
    [SerializeField] InputHandler InputHandling;
    [SerializeField] AnimationHandler AnimationHandling;
    [HideInInspector] public enum CombatState {LightAttackStart, LightAttackWindup, LightAttackActive, 
        LightAttackRecovery, HeavyAttackStart, HeavyAttackWindup, HeavyAttackActive, HeavyAttackRecovery, 
        GuardBreak, PlayerCritical, BlockStart, BlockActive, BlockRecovery, ParryStart,ParryWindup,ParryActive,ParryRecovery, Stagger, EnemyCritical, Backstab, None }
    CombatState CurrentCombatState=CombatState.None;
    

    void AnimLightWindup()
    {
        CurrentCombatState = CombatState.LightAttackWindup;
    }
    void AnimLightActive()
    {
        CurrentCombatState = CombatState.LightAttackActive;
    }
    void AnimLightRecovery()
    {
        CurrentCombatState = CombatState.LightAttackRecovery;
    }
    void AnimExitCombat()
    {
        PlayerStateHandling.CanExitCombat = true;
        InputHandling.CombatInput=InputHandler.PlayerCombatInput.None;
        CurrentCombatState = CombatState.None;
        AnimationHandling.AnimatorExitCombat();
    }

    void LightAttackHandler()
    {
        if (CurrentCombatState != CombatState.LightAttackStart)
            return;

        else
        {
            AnimationHandling.AnimatorEnterCombat();
            AnimationHandling.PlayLightAttack();
        }

        
    }
    void InputToStateTranslation()
    {
        switch (InputHandling.CombatInput)
        {
            case PlayerCombatInput.WantsToLightAttack:
                
                CurrentCombatState = CombatState.LightAttackStart;
                break;

            case PlayerCombatInput.WantsToHeavyAttack:
                
                CurrentCombatState = CombatState.HeavyAttackStart;
                break;

            case PlayerCombatInput.WantsToBlockAttack:
                
                CurrentCombatState = CombatState.BlockStart;
                break;

            case PlayerCombatInput.WantsToParryAttack:
                
                CurrentCombatState = CombatState.ParryStart;
                break;

            case PlayerCombatInput.None:
                
                break;

            default:
                
                break;

        }
        if (InputHandling.CombatInput != PlayerCombatInput.None)
        {
            InputHandling.CombatInput = PlayerCombatInput.None;
        }
    }
    void CombatHandler()
    {
        if (PlayerStateHandling.CurrentState != PlayerStateHandler.PlayerState.Combat)
            return;
        InputToStateTranslation();
        LightAttackHandler();

    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CombatHandler();
    }
}
