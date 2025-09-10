
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
        // If we don't have a collider, do nothing.
        if (HitboxCollider == null) return;

        // Only draw the gizmo IF the hitbox collider is currently enabled.
        if (HitboxCollider.enabled)
        {
            // Set the color for our gizmo
            Gizmos.color = Color.red;

            // This is a pro-tip: setting the Gizmos.matrix ensures that the
            // cube is drawn with the exact rotation and scale of your hitbox object.
            Gizmos.matrix = transform.localToWorldMatrix;

            // Draw a wireframe cube that matches the center and size of our collider.
            Gizmos.DrawWireCube(HitboxCollider.center, HitboxCollider.size);
        }
    }
    // This public method allows another script to give us the reference.
 

    void Update()
    {
        // Now we can check the reference that was given to us.
        // We need to check if it's null in case it hasn't been set up yet.
        if (CombatStateHandler == null)
        {
            Debug.Log("you done fucked up");
            return;
        }


            // The rest of your logic
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