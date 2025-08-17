using UnityEngine;

public class Climbable : MonoBehaviour
{
    
    
    public enum ClimbType { top, bottom};
    public ClimbType type;

    public Transform SnapPointEnter;
    public Transform SnapPointExit;
    
    

    private void OnTriggerEnter(Collider other)
    {
        TriggerHandling TriggerHandler = other.GetComponent<TriggerHandling>();
        AnimationHandler AnimationHandling = other.GetComponent<AnimationHandler>();

    }
}
