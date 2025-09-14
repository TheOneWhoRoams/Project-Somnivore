using UnityEngine;

public class AiAnimationHandler : MonoBehaviour
{
    [SerializeField] AiStateHandler StateHandler;
    [SerializeField] AiCombatSubStateHandler CombatSubHandler;
    
    private bool FlagConsumed = false;

    Animator AiAnimator;
    private void Tick()
    {
        switch (StateHandler.CurrentAiState)
        {
            case AiStateHandler.AiState.Idling:
                ResetAllAnimatorParameters();
                break;
            case AiStateHandler.AiState.Chasing:
                
                ToggleWalkOn();
                break;
            case AiStateHandler.AiState.Attacking:
                //insert more complicated attacking logic here later
                
                PlayAttack_CloseRange_1();
                break;
            case AiStateHandler.AiState.SoftStaggerHit:
                
                PlaySoftStagger();
                break;
            case AiStateHandler.AiState.HardStaggerHit:
                
                PlayHardStagger();
                break;
            case AiStateHandler.AiState.Death:
                ResetAllAnimatorParameters();
                PlayDeath();
                break;
        }
    }
    public void ResetAllAnimatorParameters()
    {
        

        foreach (AnimatorControllerParameter parameter in AiAnimator.parameters)
        {
            
            if (parameter.type == AnimatorControllerParameterType.Trigger)
            {
                AiAnimator.ResetTrigger(parameter.name);
            }
            
            if (parameter.type == AnimatorControllerParameterType.Bool)
            {
                AiAnimator.SetBool(parameter.name, false);
            }
           
            if (parameter.type == AnimatorControllerParameterType.Float)
            {
                AiAnimator.SetFloat(parameter.name, 0f);
            }
            
            if (parameter.type == AnimatorControllerParameterType.Int)
            {
                AiAnimator.SetInteger(parameter.name, 0);
            }
        }
    }
    void Start()
    {
        AiAnimator = GetComponent<Animator>();
    }
    public void ToggleWalkOn()
    {
        AiAnimator.SetBool("Walking", true);
    }
    public void ToggleWalkOff()
    {
        AiAnimator.SetBool("Walking", false);
    }
    void FlagConsumedTrue()
    {
        
        FlagConsumed = true;
    }
    void FlagConsumedFalse()
    {
        //set via anim event on animation finished where needed
        FlagConsumed = false;
    }
    public void PlayAttack_CloseRange_1()
    {
        if(FlagConsumed ==false)
        {
            AiAnimator.SetTrigger("Attack_CloseRange_1");
            FlagConsumedTrue();
        }
        
    }
    public void PlayAttack_LongRange_Charge_1()
    {
        AiAnimator.SetTrigger("Attack_LongRange_Charge_1");
    }
    public void PlayDeath()
    {
        AiAnimator.SetTrigger("Death");
    }
    public void PlaySoftStagger()
    {
        
    }
    public void PlayHardStagger()
    {
        AiAnimator.SetTrigger("HardStagger");
    }
    void Update()
    {
        Tick();
    }
}
