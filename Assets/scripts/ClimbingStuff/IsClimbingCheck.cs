using UnityEngine;
using static Climbable;

public class IsClimbingCheck : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        TriggerHandling TriggerHandler = other.GetComponent<TriggerHandling>();
        AnimationHandler AnimationHandling = other.GetComponent<AnimationHandler>();

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









