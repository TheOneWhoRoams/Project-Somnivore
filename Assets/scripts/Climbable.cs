using UnityEngine;

public class Climbable : MonoBehaviour
{
    public enum ClimbType { Ladder, Vine, Rope};
    public ClimbType type = ClimbType.Ladder;
}
