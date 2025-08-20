using UnityEngine;

public class Climbable : MonoBehaviour
{


    [HideInInspector] public enum ClimbType { TopEnter, TopExit, BottomEnter, BottomExit};
    [HideInInspector] public bool IsInEntryZone = false;
    [HideInInspector] public bool WantsToExitClimb = false;
    [HideInInspector] public bool ShowInDebug = false;
    public ClimbType Type;
    private Climbable CurrentClimbable;

    public Transform SnapPoint;
    // Call this from Climbable.OnTriggerEnter
    public void SetCurrentClimbable(Climbable climbable)
    {
        CurrentClimbable = climbable;
    }

    // Call this from Climbable.OnTriggerExit
    public void ClearCurrentClimbable()
    {
        CurrentClimbable = null;
    }
    void OnDrawGizmos()
    {
        // This ensures the gizmo has the same position, rotation, and scale as the trigger
        Gizmos.matrix = transform.localToWorldMatrix;

        // Set the color you want the gizmo to be
        switch (Type)
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
        ShowInDebug = true;
        TriggerHandling TriggerHandler = other.GetComponent<TriggerHandling>();
        if (TriggerHandler != null)
        {
            TriggerHandler.CurrentClimbable=this;
        }
        switch (Type)
        {
            case ClimbType.TopEnter:
            case ClimbType.BottomEnter:
                {
                    IsInEntryZone = true;
                    WantsToExitClimb = false;
                    break;
                }
            default:
                {
                    WantsToExitClimb = true;
                    IsInEntryZone = false;
                    break;
                }
        }

    }
    private void OnTriggerExit(Collider other)
    {
        ShowInDebug = false;
        TriggerHandling TriggerHandler = other.GetComponent<TriggerHandling>();
        if (TriggerHandler != null)
        {
            TriggerHandler.CurrentClimbable = null;
        }
        IsInEntryZone = false;
        WantsToExitClimb = false;
    }
}
