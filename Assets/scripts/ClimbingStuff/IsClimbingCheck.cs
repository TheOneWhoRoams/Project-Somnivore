using UnityEngine;

public class IsClimbingCheck : MonoBehaviour
{
    public bool IsInClimbZone=false;
    private void OnTriggerEnter(Collider other)
    {
        TriggerHandling TriggerHandler = other.GetComponent<TriggerHandling>();
        if (TriggerHandler != null)
        {
            TriggerHandler.CurrentClimbingCheck = this;
        }
        IsInClimbZone = true;

    }
    private void OnTriggerExit(Collider other)
    {
        TriggerHandling TriggerHandler = other.GetComponent<TriggerHandling>();
        if (TriggerHandler != null)
        {
            TriggerHandler.CurrentClimbingCheck = null;
        }
        IsInClimbZone = false;
    }
    void OnDrawGizmos()
    {
        // This ensures the gizmo has the same position, rotation, and scale as the trigger
        Gizmos.matrix = transform.localToWorldMatrix;

         Gizmos.color = new Color(0, 0, 1, 0.5f); // Blue
          
        // Draw a cube that is filled in and semi-transparent
        // Vector3.one represents a 1x1x1 cube, which will be scaled by the object's transform
        Gizmos.DrawCube(Vector3.zero, Vector3.one);
    }
}









