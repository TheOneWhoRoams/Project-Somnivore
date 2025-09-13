using UnityEngine;

public class AiStateHandler : MonoBehaviour
{
    public enum AiState { Idling, Chasing, Attacking, SoftStaggerHit, HardStaggerHit, Death }
    public AiState CurrentAiState = AiState.Idling;
    public enum AiStateSwitch { WantsToDie, WantsToHardStagger, WantsToSoftStagger, WantsToAttack, WantsToIdle, WantsToChase}
    AiStateSwitch CurrentStateSwitch = AiStateSwitch.WantsToIdle;
    float Health = 1;
    float Poise = 1;


    //placeholder vars
    public bool InDisengageRange;
    public bool InCloseRange;
    public bool InEngageRange;
    public bool InAttackRange;
    public bool GotParried;
    public bool IsBusy = false;
    public bool AttackFinished = true;


    public void AnimSetBusy()
    {
        IsBusy = true;
    }
    public void AnimSetNotBusy()
    {
        IsBusy = false;
    }
    void StateTransition()
    {
        
            

        if(!IsBusy)
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
            if (!InEngageRange&&!InDisengageRange)
                CurrentStateSwitch = AiStateSwitch.WantsToIdle;
            else if (InEngageRange && !InAttackRange)
                CurrentStateSwitch = AiStateSwitch.WantsToChase;
            else if(InEngageRange && InAttackRange)
                CurrentStateSwitch = AiStateSwitch.WantsToAttack;
        }




    }
    void StateSwitch()
    {

        switch (CurrentAiState)
        {
            case AiState.Idling:
                StateTransition();
                break;
            case AiState.Chasing:
                StateTransition();
                break;
            case AiState.SoftStaggerHit:
                StateTransition();
                break;
            case AiState.HardStaggerHit:
                StateTransition();
                break;
            case AiState.Attacking:
                StateTransition();
                break;
            case AiState.Death:
                //insert death logic here
                break;

        }
    }
    void Start()
    {
        
    }

   
    void Update()
    {
        Debug.Log("Current Ai State: "+CurrentAiState);
        DetermineStateTransition();
        StateSwitch();
    }
}
