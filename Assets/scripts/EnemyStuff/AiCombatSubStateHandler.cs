using UnityEngine;

public class AiCombatSubStateHandler : MonoBehaviour
{
    [SerializeField] AiStateHandler AiStateHandling;

    public enum CombatState {Windup, Active, Recovery, None}
    public CombatState CurrentCombatState = CombatState.None;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public bool AttackActive()
    {
        return CurrentCombatState == CombatState.Active;
    }
    bool InCombatState()
    {
        return CurrentCombatState != CombatState.None;
    }
    void AnimSetWindup()
    {
        CurrentCombatState = CombatState.Windup;
        AiStateHandling.AttackFinished = false;
    }
    void AnimSetActive()
    {
        CurrentCombatState = CombatState.Active;
    }
    void AnimSetRecovery()
    {
        CurrentCombatState = CombatState.Recovery;
    }
    void AnimExitCombat()
    {
        CurrentCombatState = CombatState.None;
        AiStateHandling.AttackFinished = true;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Current Ai Combat Substate: "+CurrentCombatState);
    }
}
