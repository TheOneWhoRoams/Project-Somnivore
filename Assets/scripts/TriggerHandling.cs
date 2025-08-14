using UnityEngine;

public class TriggerHandling : MonoBehaviour
{
    [HideInInspector] public bool InClimbZone = false;
    Climbable CurrentClimbable;
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Hit a trigger");
        var climbable = other.GetComponentInParent<Climbable>();
        if(climbable != null)
        {
            InClimbZone = true;
            CurrentClimbable = climbable;
            Debug.Log("entered climb zone");
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
