using UnityEngine;

public class PlayerHitBoxHandler : MonoBehaviour
{
    [SerializeField] TriggerHandling TriggerHandler;
    [SerializeField] ResourceHandler ResourceHandling;
    [HideInInspector] public WeaponData AttackingWeaponData;



    
    private void OnTriggerEnter(Collider other)
    {
        AttackingWeaponData = other.GetComponent<WeaponData>();
        ResourceHandling.TakeDamage(AttackingWeaponData);
    }
    private void OnTriggerExit(Collider other)
    {
        AttackingWeaponData = null;
    }
}
