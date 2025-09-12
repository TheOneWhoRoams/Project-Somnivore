using UnityEngine;
using System.Collections;
public class AttachWeapon : MonoBehaviour
{
    
    public GameObject MainHandPrefab;
    public GameObject OffHandPrefab;
    [SerializeField] private CombatStateHandler combatStateHandler;
    [HideInInspector] public enum BothHandsOccupied { Yes, No}
    public BothHandsOccupied AreBothHandsOccupied;
    
    public Transform HandSocketR;
    public Transform HandSocketL;
    IEnumerator AttachWeaponAfterInitialization()
    {
        yield return new WaitForEndOfFrame();

        GameObject Weapon = null; 
        GameObject OffHand = null;

        switch (AreBothHandsOccupied)
        {
            case BothHandsOccupied.Yes:
                

                OffHand = Instantiate(OffHandPrefab, HandSocketL.position, HandSocketL.rotation);
                OffHand.transform.SetParent(HandSocketL);
                OffHand.transform.localPosition = Vector3.zero;
                
                Weapon = Instantiate(MainHandPrefab, HandSocketR.position, HandSocketR.rotation);
                Weapon.transform.SetParent(HandSocketR);
                Weapon.transform.localPosition = Vector3.zero;
                break;
            case BothHandsOccupied.No:
                if (MainHandPrefab != null && HandSocketR != null)
                {
                    
                    Weapon = Instantiate(MainHandPrefab, HandSocketR.position, HandSocketR.rotation);
                    Weapon.transform.SetParent(HandSocketR);
                    Weapon.transform.localPosition = Vector3.zero;
                }
                break;
        }

       
        if (Weapon != null && combatStateHandler != null)
        {
            WeaponHitboxController hitboxController = Weapon.GetComponentInChildren<WeaponHitboxController>();
            if (hitboxController != null)
            {
                hitboxController.Setup(combatStateHandler);
            }
        }
    }
    void Start()
    {
        StartCoroutine(AttachWeaponAfterInitialization());
        
    }
}