using UnityEngine;

public class AiReferenceManager : MonoBehaviour
{
    // 1. Drag the component that has your AiCombatSubStateHandler script here.
    [SerializeField]
    private AiCombatSubStateHandler combatStateHandler;

    // 2. Drag the child GameObject that has the weapon and its hitbox here.
    [SerializeField]
    private AttachWeapon AttachWeaponScript;

    private GameObject weaponObject;

    void Start()
    {
        weaponObject = AttachWeaponScript.MainHandPrefab;
        // We will find the hitbox controller on the weapon object you assigned.
        AiWeaponHitboxController hitboxController = weaponObject.GetComponentInChildren<AiWeaponHitboxController>();

        // Check to make sure everything is assigned correctly before we connect them.
        if (combatStateHandler != null && hitboxController != null)
        {
            // This is the line that makes the connection.
            // It calls Setup() on the hitbox and gives it the reference it needs.
            hitboxController.Setup(combatStateHandler);

            Debug.Log("SUCCESS: AiWeaponManager has connected the combat state to the hitbox.", this.gameObject);
        }
        else
        {
            // If something is missing, this error will tell you exactly what.
            Debug.LogError("ERROR: AiWeaponManager is missing a reference! " +
                           "Ensure the Combat State Handler and Weapon Object are assigned in the Inspector.", this.gameObject);
        }
    }
}
