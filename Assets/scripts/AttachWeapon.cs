using UnityEngine;
using System.Collections;
public class AttachWeapon : MonoBehaviour
{
    // Drag your sword prefab here in the Inspector
    public GameObject MainHandPrefab;
    public GameObject OffHandPrefab;

    // Drag your character's hand socket/bone here
    public Transform HandSocketR;
    public Transform HandSocketL;
    IEnumerator AttachWeaponAfterInitialization()
    {
        // Wait until the end of the first frame
        yield return new WaitForEndOfFrame();
        if (OffHandPrefab != null && MainHandPrefab != null && HandSocketL != null&& HandSocketR != null)
        {
            // Create a new instance of the weapon
            GameObject Weapon = Instantiate(MainHandPrefab, HandSocketR.position, HandSocketR.rotation);
            GameObject OffHand = Instantiate(OffHandPrefab, HandSocketL.position, HandSocketL.rotation);

            // Attach it to the hand socket
            Weapon.transform.SetParent(HandSocketR);
            OffHand.transform.SetParent(HandSocketL);

            Weapon.transform.localPosition = Vector3.zero;
            OffHand.transform.localPosition = Vector3.zero;
        }
        // ... code to attach the weapon ...
    }
    void Start()
    {
        StartCoroutine(AttachWeaponAfterInitialization());
        
    }
}