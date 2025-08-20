using Unity.VisualScripting;
using UnityEngine;

public class TriggerHandling : MonoBehaviour
{

    [HideInInspector] public bool InClimbZone = false;
    [HideInInspector] public Climbable CurrentClimbable;
    [HideInInspector] public IsClimbingCheck CurrentClimbingCheck;
    
    [SerializeField] PlayerMovement PlayerMovHandling;
    
    private void OnTriggerEnter(Collider other)
    {
        
    }
    private void OnTriggerExit(Collider other)
    {
       
    }
}
