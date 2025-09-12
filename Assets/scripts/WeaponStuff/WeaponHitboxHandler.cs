
using UnityEngine;

public class WeaponHitboxController : MonoBehaviour
{
    private CombatStateHandler combatStateHandler;
    private BoxCollider hitboxCollider;

    void Awake()
    {
        hitboxCollider = GetComponent<BoxCollider>();
        hitboxCollider.enabled = false;
    }
    private void OnDrawGizmos()
    {
       
        if (hitboxCollider == null) return;

       
        if (hitboxCollider.enabled)
        {
            
            Gizmos.color = Color.red;

            
           
            Gizmos.matrix = transform.localToWorldMatrix;

            
            Gizmos.DrawWireCube(hitboxCollider.center, hitboxCollider.size);
        }
    }
   
    public void Setup(CombatStateHandler handler)
    {
        this.combatStateHandler = handler;
    }

    void Update()
    {
        
        if (combatStateHandler == null) return;

        
        if (combatStateHandler.AttackActive)
        {
            hitboxCollider.enabled = true;
            Debug.Log("Hitbox Enabled");
        }
        else if (!combatStateHandler.AttackActive)
        {
            hitboxCollider.enabled = false;
            Debug.Log("Hitbox Disabled");
        }
    }
}