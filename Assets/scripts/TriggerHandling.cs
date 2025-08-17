using Unity.VisualScripting;
using UnityEngine;

public class TriggerHandling : MonoBehaviour
{
    public enum TriggerType { Entry, Exit};
    public TriggerType Trigger = TriggerType.Entry;
    public bool ClimbExit = false;
    bool HasChangedClimbingStates = false;
    public bool AnimatorWantsToExit = false;
    public bool CanClearClimbable = false;
    [HideInInspector] public bool InClimbZone = false;
    [HideInInspector] public Climbable CurrentClimbable;
    [HideInInspector] public enum ExitFrom { Top, Bottom }
    [SerializeField] PlayerMovement PlayerMovHandling;
    //todo: exiting climbing
    private void OnTriggerEnter(Collider other)
    {
        switch (Trigger)
        {
            case TriggerType.Entry:
                {
                    var climbable = other.GetComponentInParent<Climbable>();
                    if (climbable != null)
                    {
                        InClimbZone = true;
                        CurrentClimbable = climbable;
                        Debug.Log("Entered climb zone. CurrentClimbable: " + CurrentClimbable.name);
                    }
                    else
                    {
                        Debug.LogWarning("No Climbable component found on the object!");
                    }
                    break;
                }
            case TriggerType.Exit:
                {
                    AnimatorWantsToExit = true;
                    ClimbExit = true;
                    HasChangedClimbingStates = false;
                    InClimbZone = false;
                    Debug.Log("Has exited the ladder");
                    PlayerMovHandling.EndClimb();
                    Trigger = TriggerType.Entry;
                    break;
                }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponentInParent<Climbable>() == CurrentClimbable)
        {
            if (InClimbZone && !AnimatorWantsToExit && CanClearClimbable)
            {
                // Only clear CurrentClimbable if the player is not climbing
                InClimbZone = false;
                CurrentClimbable = null;
                Debug.Log("Exited climb zone and cleared CurrentClimbable.");
                CanClearClimbable = false;
            }
            else
            {
                Debug.Log("Exited climb zone but did not clear CurrentClimbable because the player is climbing.");
            }

            if (!HasChangedClimbingStates)
            {
                switch (Trigger)
                {
                    case TriggerType.Entry:
                        Trigger = TriggerType.Exit;
                        HasChangedClimbingStates = true;
                        break;
                    case TriggerType.Exit:
                        Trigger = TriggerType.Entry;
                        HasChangedClimbingStates = true;
                        
                        break;
                }
            }
        }
    }
}
