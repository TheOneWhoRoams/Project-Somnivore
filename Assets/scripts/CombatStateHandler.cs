using UnityEngine;

public class CombatStateHandler : MonoBehaviour
{

    [SerializeField] PlayerStateHandler PlayerStateHandling;
    [HideInInspector] public enum CombatState {LightAttackStart, LightAttackWindup, LightAttackActive, 
        LightAttackRecovery, HeavyAttackStart, HeavyAttackWindup, HeavyAttackActive, HeavyAttackRecovery, 
        GuardBreak, PlayerCritical, Block, Parry, Stagger, EnemyCritical, Backstab, None }
    CombatState CurrentCombatState;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
