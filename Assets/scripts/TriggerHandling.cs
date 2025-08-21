using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using static Climbable;
using static PlayerStateHandler;

public class TriggerHandling : MonoBehaviour
{

    [HideInInspector] public bool InClimbZone = false;
    [HideInInspector] public Climbable CurrentClimbable;
    [HideInInspector] public IsClimbingCheck CurrentClimbingCheck;
    [HideInInspector] public PlayerStateHandler PlayerStateHandling;
    
    [SerializeField] PlayerMovement PlayerMovHandling;
    

    // In TriggerHandling.cs
    public Climbable GetBestClimbable()
    {
        // Simple logic: if we have multiple climbables, pick based on what we're doing
        if (PlayerStateHandling.CurrentState == PlayerState.Climbing)
        {
            // Look for any exit triggers first
            if (CurrentClimbable != null &&
                (CurrentClimbable.Type == ClimbType.TopExit || CurrentClimbable.Type == ClimbType.BottomExit))
                return CurrentClimbable;
        }
        else
        {
            // Look for entry triggers
            if (CurrentClimbable != null &&
                (CurrentClimbable.Type == ClimbType.TopEnter || CurrentClimbable.Type == ClimbType.BottomEnter))
                return CurrentClimbable;
        }

        return CurrentClimbable; // Fallback to whatever we have
    }
    private void OnTriggerEnter(Collider other)
    {
        
    }
    private void OnTriggerExit(Collider other)
    {
       
    }
}
