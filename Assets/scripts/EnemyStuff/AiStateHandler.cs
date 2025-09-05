using UnityEngine;

public class AiStateHandler : MonoBehaviour
{
    public enum AiState { Idling, Chasing, Attacking, SoftStaggerHit, HardStaggerHit, Death }
    public AiState CurrentAiState = AiState.Idling;
    public enum AiStateSwitch { WantsToDie, WantsToHardStagger, WantsToSoftStagger, WantsToAttack, WantsToIdle, WantsToChase}
    AiStateSwitch CurrentStateSwitch = AiStateSwitch.WantsToIdle;
    float Health = 1;
    float Poise = 1;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    //placeholder vars
    bool InEngageZone;
    bool GotParried;
    void StateTransition(bool IsCurrentActionInterruptible)
    {
        //if (IsCurrentActionInterruptible)
        //Coroutine waiting for animation end (via animation event?)
            

        
        switch (CurrentStateSwitch)
        {
            case AiStateSwitch.WantsToDie:
                TransitionTo(AiState.Death);
                break;
            case AiStateSwitch.WantsToHardStagger:
                TransitionTo(AiState.HardStaggerHit);
                break;
            case AiStateSwitch.WantsToSoftStagger:
                TransitionTo(AiState.HardStaggerHit);
                break;
            case AiStateSwitch.WantsToAttack:
                TransitionTo(AiState.Attacking);
                break; 
            case AiStateSwitch.WantsToChase:
                TransitionTo(AiState.Chasing);
                break;
            case AiStateSwitch.WantsToIdle:
                TransitionTo(AiState.Idling);
                break;
        }

    }
    void TransitionTo(AiState state)
    {
        CurrentAiState = state;
    }
    bool DisruptiveStateHandler()
    {
        if (Health<=0)
        {
            CurrentStateSwitch=AiStateSwitch.WantsToDie;
            return true;
        }
        
        else if (GotParried) 
        {
            CurrentStateSwitch = AiStateSwitch.WantsToHardStagger;
            return true;
        }
        else if (Poise<=0)
        {
            CurrentStateSwitch = AiStateSwitch.WantsToSoftStagger;
            return true;
        }
        
        return false;
    }
    void DetermineStateTransition()
    {
       if(DisruptiveStateHandler())
            return;
        else
        {
            //if(!InEngageZone)
            //StatesOtherThanSpecifiedFalse(WantsToIdle)
            //else if(InEngageZone&&!InAttackRange)
            //StatesOtherThanSpecifiedFalse(WantsToChase)
            //else if(InEngageZone&&InAttackRange)
            //StatesOtherThanSpecifiedFalse(WantsToAttack)
        }




    }
    void StateSwitch()
    {

        switch (CurrentAiState)
        {
            case AiState.Idling:
                StateTransition(true);
                break;
            case AiState.Chasing:
                StateTransition(true);
                break;
            case AiState.SoftStaggerHit:
                StateTransition(false);
                break;
            case AiState.HardStaggerHit:
                StateTransition(false);
                break;
            case AiState.Attacking:
                StateTransition(true);
                break;
            case AiState.Death:
                //insert death logic here
                break;

        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        DetermineStateTransition();
        StateSwitch();
    }
}
