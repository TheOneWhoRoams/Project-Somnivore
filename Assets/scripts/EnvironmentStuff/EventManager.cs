using System;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    // Declare the static event
    public static event Action OnPlayerRested;
    public static event Action OnPlayerDeathInitiate;
    public static event Action<GameObject> OnPlayerDeathFinish;
    public static event Action<Transform> OnPlayerRestSetSpawnPoint;

    // Create a public static method to "raise" or "invoke" the event
    public static void RaisePlayerRestedEvent()
    {
        Debug.Log("OnPlayerRested event has been raised.");
        OnPlayerRested?.Invoke();
    }
    public static void RaisePlayerDeathInitiate()
    {
        OnPlayerDeathInitiate?.Invoke();
    }
    public static void RaisePlayerDeathFinish(GameObject Player)
    {
        OnPlayerDeathFinish?.Invoke(Player);
    }
    public static void RaisePlayerRestSpawnPointSet(Transform NewSpawnPoint)
    {
        OnPlayerRestSetSpawnPoint?.Invoke(NewSpawnPoint);
    }
}