using UnityEngine;
using System.Collections;
public class ResourceHandler : MonoBehaviour
{
    [SerializeField] PlayerStateHandler PlayerStateHandling;
    [SerializeField] CombatStateHandler CombatStateHandling;
    [SerializeField] public float SetStamina;
    [SerializeField] public float SetHealth;
    [SerializeField] float SetPoise;
    [SerializeField] float SetStaminaRegenAmount;
    [SerializeField] float SetStaminaRegenDelay;
    [SerializeField] float SetSprintStaminaDrainAmountOnTick;
    [HideInInspector] public float Stamina;
    [HideInInspector] public float Health;
    [HideInInspector] public float Poise;
    [HideInInspector] public float StaminaRegenDelay;
    [HideInInspector] public float StaminaRegenAmount;
    [HideInInspector] public float SprintStaminaDrainAmountOnTick;
    enum ActionType { Single, Continuous};
    ActionType Action = ActionType.Single;
    float StaminaDrainAmount = 30;

    
    private Coroutine RegenerationCoroutine;




    
    private void OnEnable()
    {
        
        EventManager.OnPlayerRested += ReplenishResources;
    }

    private void OnDisable()
    {
        
        EventManager.OnPlayerRested -= ReplenishResources;
    }

    void ReplenishResources()
    {
        Health = SetHealth;
        Poise = SetPoise;
        Stamina = SetStamina;
    }
    public void TakeDamage(WeaponData WeaponData)
    {
        if (!CombatStateHandling.BlockActive)
        {
            Health -= WeaponData.WeaponDamage;
        }
        else
        {
            Stamina -= WeaponData.WeaponDamage;
            if(Stamina <= 0)
                EventManager.RaisePlayerGuardBreak();
            
                
        }
        
    }
   public bool CanSpendStamina(float CurrentStamina)
    {
        
        return CurrentStamina > 0;
    }
   
    public void ManageStaminaDrain(bool DrainingActionActive, float StaminaDrainEachTick)
    {
        if  (DrainingActionActive && CanSpendStamina(Stamina))
        {
            
            Stamina = Stamina - (StaminaDrainEachTick * Time.deltaTime);
        }
    }
    private IEnumerator RegenStaminaAfterDelay()
    {
        
        yield return new WaitForSeconds(StaminaRegenDelay);
        

       
        while (Stamina < SetStamina)
        {
            Stamina += StaminaRegenAmount * Time.deltaTime;
            yield return null;
        }
        Stamina = SetStamina;
        
        RegenerationCoroutine = null;
    }
    void ManageStaminaRegeneration()
    {
        
        if (PlayerStateHandling.CanRegenStamina)
        {
            
            if (RegenerationCoroutine == null)
            {
                
                RegenerationCoroutine = StartCoroutine(RegenStaminaAfterDelay());
            }
        }
        else
        {
            if (RegenerationCoroutine != null)
            {
                StopCoroutine(RegenerationCoroutine);
                RegenerationCoroutine = null;
            }
        }
    }

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
        Health = SetHealth;
        Poise = SetPoise;
        Stamina = SetStamina;
        StaminaRegenAmount = SetStaminaRegenAmount;
        StaminaRegenDelay = SetStaminaRegenDelay; 
        SprintStaminaDrainAmountOnTick = SetSprintStaminaDrainAmountOnTick; 
    }


    void Update()
    {
        if (Health <= 0)
            EventManager.RaisePlayerDeathInitiate();

        ManageStaminaDrain(PlayerStateHandling.StaminaDrainActive, SprintStaminaDrainAmountOnTick);
        ManageStaminaRegeneration();
    }
}
