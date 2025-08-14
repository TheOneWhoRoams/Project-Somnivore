using UnityEngine;

public class TriggerHandling : MonoBehaviour
{
    public enum TriggerType { Entry, Exit};
    public TriggerType Trigger;
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
        }
    }
}
