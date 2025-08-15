using UnityEngine;

public class Climbable : MonoBehaviour
{
    
    
    public enum ClimbType { Ladder, Vine, Rope};
    public ClimbType type = ClimbType.Ladder;

    private void OnTriggerEnter(Collider other)
    {
        TriggerHandling TriggerHandler = other.GetComponent<TriggerHandling>();

    }
}
