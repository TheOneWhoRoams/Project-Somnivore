using UnityEngine;

public class AiStateHandler : MonoBehaviour
{
    [SerializeField] private AiResourceHandler Resources;
    public enum AiState { Idling, Chasing, Attacking, SoftStaggerHit, HardStaggerHit, Death }
    public AiState CurrentAiState = AiState.Idling;
    public enum AiStateSwitch { WantsToDie, WantsToHardStagger, WantsToSoftStagger, WantsToAttack, WantsToIdle, WantsToChase}
    AiStateSwitch CurrentStateSwitch = AiStateSwitch.WantsToIdle;
    
    


    //placeholder vars
    [HideInInspector] public bool InDisengageRange;
    [HideInInspector] public bool InCloseRange;
    [HideInInspector] public bool InEngageRange;
    [HideInInspector] public bool InAttackRange;
    [HideInInspector] public bool GotParried;
    [HideInInspector] public bool IsBusy = false;
    [HideInInspector] public bool AttackFinished = true;


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
        if (Resources.Health<=0)
        {
            Debug.Log("Ai is suicidal");
            CurrentStateSwitch=AiStateSwitch.WantsToDie;
            return true;
        }
        
        else if (GotParried) 
        {
            CurrentStateSwitch = AiStateSwitch.WantsToHardStagger;
            return true;
        }
        else if (Resources.Poise<=0)
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
