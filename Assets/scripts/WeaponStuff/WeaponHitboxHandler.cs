
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
        // If we don't have a collider, do nothing.
        if (hitboxCollider == null) return;

        // Only draw the gizmo IF the hitbox collider is currently enabled.
        if (hitboxCollider.enabled)
        {
            // Set the color for our gizmo
            Gizmos.color = Color.red;

            // This is a pro-tip: setting the Gizmos.matrix ensures that the
            // cube is drawn with the exact rotation and scale of your hitbox object.
            Gizmos.matrix = transform.localToWorldMatrix;

            // Draw a wireframe cube that matches the center and size of our collider.
            Gizmos.DrawWireCube(hitboxCollider.center, hitboxCollider.size);
        }
    }
    // This public method allows another script to give us the reference.
    public void Setup(CombatStateHandler handler)
    {
        this.combatStateHandler = handler;
    }

    void Update()
    {
        // Now we can check the reference that was given to us.
        // We need to check if it's null in case it hasn't been set up yet.
        if (combatStateHandler == null) return;

        // The rest of your logic
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