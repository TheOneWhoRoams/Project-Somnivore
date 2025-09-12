
using UnityEngine;

public class AiTriggerHandler : MonoBehaviour
{
   
    [SerializeField] private AiResourceHandler ResourceHandler;

    
    public void OnTriggerEnter(Collider other)
    {
        Debug.Log(this.gameObject.name + " was hit by " + other.gameObject.name);
     
        WeaponData AttackingWeapon = other.GetComponent<WeaponData>();

        
        if (AttackingWeapon != null)
        {
          
            ResourceHandler.TakeDamage(AttackingWeapon);
        }
    }
}