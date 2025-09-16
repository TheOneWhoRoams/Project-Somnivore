using System;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    // Declare the static event
    public static event Action OnPlayerRested;

    // Create a public static method to "raise" or "invoke" the event
    public static void RaisePlayerRestedEvent()
    {
        Debug.Log("OnPlayerRested event has been raised.");
        OnPlayerRested?.Invoke();
    }
}