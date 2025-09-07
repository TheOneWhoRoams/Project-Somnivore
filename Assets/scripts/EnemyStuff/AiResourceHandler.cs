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
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void TakeDamage(WeaponData AttackingWeapon)
    {
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

    // Update is called once per frame
    void Update()
    {
        
    }
}
