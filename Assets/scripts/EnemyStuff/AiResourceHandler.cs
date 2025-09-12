using UnityEngine;

public class AiResourceHandler : MonoBehaviour
{
    public float SetHealth;
    [HideInInspector] public float Health;
    public float SetPoise;
    [HideInInspector] public float Poise;
    [HideInInspector] public float BleedMeter;
    [HideInInspector] public float PoisonMeter;
    public float SetBleedMeter;
    public float SetPoisonMeter;
   
    public void TakeDamage(WeaponData AttackingWeapon)
    {
        if (AttackingWeapon.Owner != WeaponData.WeaponOwner.Player)
            return;

        Debug.Log("Damage Taken");
        Health -= AttackingWeapon.WeaponDamage;
        Poise -= AttackingWeapon.WeaponPoiseDamage;
    }
    void Start()
    {
        Health = SetHealth;
        Poise = SetPoise;
        BleedMeter = SetBleedMeter;
        PoisonMeter = SetPoisonMeter; 
    }

    void Update()
    {
        
    }
}
