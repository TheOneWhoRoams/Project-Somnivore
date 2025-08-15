using UnityEngine;

public class TriggerHandling : MonoBehaviour
{
    public enum TriggerType { Entry, Exit};
    public TriggerType Trigger = TriggerType.Entry;
    public bool ClimbExit = false;
    bool HasChangedClimbingStates = false;
    [HideInInspector] public bool InClimbZone = false;
    Climbable CurrentClimbable;
    //todo: exiting climbing
    private void OnTriggerEnter(Collider other)
    {
        switch(Trigger)
        {
            case TriggerType.Entry:
            {
                 var climbable = other.GetComponentInParent<Climbable>();
                 if (climbable != null)
                 {
                        InClimbZone = true;
                        CurrentClimbable = climbable;
                        Debug.Log("entered climb zone");
                 }
                    break;
            }
            case TriggerType.Exit:
                {
                    ClimbExit = true;
                    HasChangedClimbingStates = false;
                    InClimbZone = false;
                    Debug.Log("Has exited the ladder");
                    Trigger = TriggerType.Entry;
                    break;
                }
            
        }
        
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponentInParent<Climbable>() == CurrentClimbable)
        {
            InClimbZone = false;
            CurrentClimbable = null;
            Debug.Log("Exited climb Zone");
            if (!HasChangedClimbingStates)
            switch (Trigger)
            {
                case TriggerType.Entry:
                    {
                        Trigger = TriggerType.Exit;
                        HasChangedClimbingStates =true;
                        break;
                    }
                case TriggerType.Exit:
                    {
                        Trigger = TriggerType.Entry;
                        HasChangedClimbingStates = true;
                        break;
                    }
            }
        }
    }
}
