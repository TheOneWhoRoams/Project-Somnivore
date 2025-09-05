using UnityEngine;
using System.Collections;
public class AttachWeapon : MonoBehaviour
{
    // Drag your sword prefab here in the Inspector
    public GameObject MainHandPrefab;
    public GameObject OffHandPrefab;
    [SerializeField] private CombatStateHandler combatStateHandler;
    [HideInInspector] public enum BothHandsOccupied { Yes, No}
    public BothHandsOccupied AreBothHandsOccupied;
    // Drag your character's hand socket/bone here
    public Transform HandSocketR;
    public Transform HandSocketL;
    IEnumerator AttachWeaponAfterInitialization()
    {
        yield return new WaitForEndOfFrame();

        GameObject Weapon = null; // Declare the variable in the wider scope

        switch (AreBothHandsOccupied)
        {
            case BothHandsOccupied.Yes:
                // ... (your existing OffHand logic) ...

                // Assign to the existing 'Weapon' variable
                Weapon = Instantiate(MainHandPrefab, HandSocketR.position, HandSocketR.rotation);
                Weapon.transform.SetParent(HandSocketR);
                Weapon.transform.localPosition = Vector3.zero;
                break;
            case BothHandsOccupied.No:
                if (MainHandPrefab != null && HandSocketR != null)
                {
                    // Assign to the existing 'Weapon' variable
                    Weapon = Instantiate(MainHandPrefab, HandSocketR.position, HandSocketR.rotation);
                    Weapon.transform.SetParent(HandSocketR);
                    Weapon.transform.localPosition = Vector3.zero;
                }
                break;
        }

        // Now you can access the 'Weapon' variable here
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