
using UnityEngine;

public class AiTriggerHandler : MonoBehaviour
{
    // You'll need a reference to the main resource handler for this AI.
    // You can drag this in from the Inspector.
    [SerializeField] private AiResourceHandler ResourceHandler;

    
    public void OnTriggerEnter(Collider other)
    {
        Debug.Log(this.gameObject.name + " was hit by " + other.gameObject.name);
        // Try to get a WeaponData component from the object that just hit us.
        WeaponData AttackingWeapon = other.GetComponent<WeaponData>();

        // If the object that hit us has WeaponData, we know it's a weapon attack.
        if (AttackingWeapon != null)
        {
            // Now, tell our own resource handler to process the damage.
            // We need to create the TakeDamage method on AiResourceHandler.
            ResourceHandler.TakeDamage(AttackingWeapon);
        }
    }
}