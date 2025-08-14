using UnityEngine;

public class Climbable : MonoBehaviour
{
    [SerializeField] private TriggerHandling TriggerHandler;
    
    public enum ClimbType { Ladder, Vine, Rope};
    public ClimbType type = ClimbType.Ladder;
    private void Awake()
    {
        if (TriggerHandler != null)
        {
            TriggerHandler.Trigger = TriggerHandling.TriggerType.Entry;
        }

    }
}
