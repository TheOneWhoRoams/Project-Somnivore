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
    [HideInInspector] public ResourceHandler ResourceHandling;
    
    [SerializeField] PlayerMovement PlayerMovHandling;


    
    public Climbable GetBestClimbable()
    {
        
        if (PlayerStateHandling.CurrentState == PlayerState.Climbing)
        {
           
            if (CurrentClimbable != null &&
                (CurrentClimbable.Type == ClimbType.TopExit || CurrentClimbable.Type == ClimbType.BottomExit))
                return CurrentClimbable;
        }
        else
        {
          
            if (CurrentClimbable != null &&
                (CurrentClimbable.Type == ClimbType.TopEnter || CurrentClimbable.Type == ClimbType.BottomEnter))
                return CurrentClimbable;
        }

        return CurrentClimbable; 
    }
    private void OnTriggerEnter(Collider other)
    {

    }
    private void OnTriggerExit(Collider other)
    {

    }
}



