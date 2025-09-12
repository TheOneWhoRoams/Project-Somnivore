
using UnityEngine;

public class AiWeaponHitboxController : MonoBehaviour
{
    private AiCombatSubStateHandler CombatStateHandler;
    private BoxCollider HitboxCollider;

    void Awake()
    {
        CombatStateHandler=GetComponentInParent<AiCombatSubStateHandler>();
        HitboxCollider = GetComponent<BoxCollider>();
        HitboxCollider.enabled = false;
    }
    private void OnDrawGizmos()
    {
        
        if (HitboxCollider == null) return;

        
        if (HitboxCollider.enabled)
        {
            Gizmos.color = Color.red;
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawWireCube(HitboxCollider.center, HitboxCollider.size);
        }
    }
    
 

    void Update()
    {
        
        if (CombatStateHandler == null)
        {
            Debug.Log("you done fucked up");
            return;
        }


            
            if (CombatStateHandler.AttackActive())
        {
            HitboxCollider.enabled = true;
            Debug.Log("Enemy Hitbox Enabled");
        }
        else if (!CombatStateHandler.AttackActive())
        {
            HitboxCollider.enabled = false;
            Debug.Log("Enemy Hitbox Disabled");
        }
    }
}