using UnityEngine;

public class ResourceHandler : MonoBehaviour
{
    [SerializeField] PlayerStateHandler PlayerStateHandling;
    [SerializeField] CombatStateHandler CombatStateHandling;
    [SerializeField] float SetStamina;
    [HideInInspector] public float Stamina;
    enum ActionType { Single, Continuous};
    ActionType Action = ActionType.Single;
    float StaminaDrainAmount = 30;
    
    void SetStaminaDrainAmount()
    {
        switch (CombatStateHandling.CurrentCombatState)
        {
            case CombatStateHandler.CombatState.LightAttackStart:
            case CombatStateHandler.CombatState.LightAttackActive:
            case CombatStateHandler.CombatState.LightAttackWindup:
                {
                    StaminaDrainAmount = 30;
                    break;
                }
            case CombatStateHandler.CombatState.HeavyAttackStart:
            case CombatStateHandler.CombatState.HeavyAttackActive:
            case CombatStateHandler.CombatState.HeavyAttackWindup:
                {
                    StaminaDrainAmount = 50;
                    break;
                }
        }

    }
    void StaminaDrain(float Value)
    {
        Stamina = Stamina - Value;
    }
    bool HasStamina()
    {
        return Stamina >= 0;
    }
  public void OnStaminaDrainingAction()
    {
        if (!HasStamina())
            return;
        SetStaminaDrainAmount();
        switch (Action)
        {
            case ActionType.Single:
                {
                    StaminaDrain(StaminaDrainAmount);
                    break;
                }
            case ActionType.Continuous:
                {
                    break;
                }
        }

    }
    void Start()
    {
        Stamina = SetStamina;   
    }

    
    void Update()
    {
        
    }
}
