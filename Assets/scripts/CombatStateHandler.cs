using UnityEngine;
using static InputHandler;

public class CombatStateHandler : MonoBehaviour
{

    [SerializeField] PlayerStateHandler PlayerStateHandling;
    [SerializeField] InputHandler InputHandling;
    [SerializeField] AnimationHandler AnimationHandling;
    [SerializeField] ResourceHandler ResourceHandling;
    [HideInInspector]
    public enum CombatState
    {
        LightAttackStart, LightAttackWindup, LightAttackActive,
        LightAttackRecovery, HeavyAttackStart, HeavyAttackWindup, HeavyAttackActive, HeavyAttackRecovery,
        GuardBreak, PlayerCritical, BlockStart, BlockActive, BlockRecovery, ParryStart, ParryWindup, ParryActive, ParryRecovery, Stagger, EnemyCritical, Backstab, None
    }
    public CombatState CurrentCombatState = CombatState.None;
    [HideInInspector]public bool AttackActive = false;


    void AnimLightWindup()
    {
        CurrentCombatState = CombatState.LightAttackWindup;
    }
    void AnimLightActive()
    {
        CurrentCombatState = CombatState.LightAttackActive;
        AttackActive = true;
    }
    void AnimLightRecovery()
    {
        CurrentCombatState = CombatState.LightAttackRecovery;
        AttackActive = false;
    }
    void AnimExitCombat()
    {
        Debug.Log($"<color=red>FRAME {Time.frameCount}: AnimExitCombat CALLED.</color> Setting CurrentCombatState to None.");
        PlayerStateHandling.CanExitCombat = true;
        InputHandling.CombatInput = InputHandler.PlayerCombatInput.None;
        CurrentCombatState = CombatState.None;
        AnimationHandling.AnimatorExitCombat();
    }

    void LightAttackHandler()
    {
        if (CurrentCombatState != CombatState.LightAttackStart)
            return;

        AnimationHandling.AnimatorEnterCombat();
        AnimationHandling.PlayLightAttack();

        CurrentCombatState = CombatState.LightAttackWindup;
        ResourceHandling.OnStaminaDrainingAction();

    }
    void InputToStateTranslation()
    {
        if (CurrentCombatState != CombatState.None) return;

       
        if (InputHandling.CombatInput != InputHandler.PlayerCombatInput.None)
        {
            Debug.LogError($"<color=orange>FRAME {Time.frameCount}: InputToStateTranslation WARNING!</color> CurrentCombatState was None, but found lingering input '{InputHandling.CombatInput}'. The state is about to be changed.");
        }

        switch (InputHandling.CombatInput)
        {
            case PlayerCombatInput.WantsToLightAttack:
                CurrentCombatState = CombatState.LightAttackStart;
                InputHandling.CombatInput = InputHandler.PlayerCombatInput.None;
                break;

            case PlayerCombatInput.WantsToHeavyAttack:
                CurrentCombatState = CombatState.HeavyAttackStart;
                InputHandling.CombatInput = InputHandler.PlayerCombatInput.None;
                break;

            case PlayerCombatInput.WantsToBlockAttack:
                CurrentCombatState = CombatState.BlockStart;
                InputHandling.CombatInput = InputHandler.PlayerCombatInput.None;
                break;

            case PlayerCombatInput.WantsToParryAttack:
                CurrentCombatState = CombatState.ParryStart;
                InputHandling.CombatInput = InputHandler.PlayerCombatInput.None;
                break;
        }
    }
    void CombatHandler()
    {
        if (PlayerStateHandling.CurrentState != PlayerStateHandler.PlayerState.Combat)
        {
            CurrentCombatState = CombatState.None;
            return;
        }

        InputToStateTranslation();
        LightAttackHandler();
    }

    void Update()
    {
        CombatHandler();
    }
}

