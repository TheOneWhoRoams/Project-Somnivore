using UnityEngine;

public class WeaponData : MonoBehaviour
{
    public enum WeaponOwner { Player, AI}
    public WeaponOwner Owner;
    public float WeaponDamage;
    public float WeaponPoiseDamage;
    public float WeaponBleedBuildup;
    public float WeaponPoisonBuildup;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
