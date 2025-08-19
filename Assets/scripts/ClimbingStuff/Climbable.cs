using UnityEngine;

public class Climbable : MonoBehaviour
{
    
    
    public enum ClimbType { TopEnter, TopExit, BottomEnter, BottomExit};
    public ClimbType type;

    public Transform SnapPoint;

    void OnDrawGizmos()
    {
        // This ensures the gizmo has the same position, rotation, and scale as the trigger
        Gizmos.matrix = transform.localToWorldMatrix;

        // Set the color you want the gizmo to be
        switch (type)
        {
            case ClimbType.TopEnter:
            case ClimbType.BottomEnter:
                Gizmos.color = new Color(0, 1, 0, 0.5f); // Green
                break;
            case ClimbType.TopExit:
            case ClimbType.BottomExit:
                Gizmos.color = new Color(1, 0, 0, 0.5f); // Red
                break;
        }

        // Draw a cube that is filled in and semi-transparent
        // Vector3.one represents a 1x1x1 cube, which will be scaled by the object's transform
        Gizmos.DrawCube(Vector3.zero, Vector3.one);
    }


    private void OnTriggerEnter(Collider other)
    {
        TriggerHandling TriggerHandler = other.GetComponent<TriggerHandling>();
        AnimationHandler AnimationHandling = other.GetComponent<AnimationHandler>();

    }
}
