using UnityEngine;

public class Bonfire : MonoBehaviour
{

    private TriggerHandling TriggerHandler;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        TriggerHandler = other.GetComponent<TriggerHandling>();

        if (TriggerHandler != null)
        {
            TriggerHandler.InBonfireRange = true;
        }

    }
    private void OnTriggerExit(Collider other)
    {
        TriggerHandling triggerHandler = other.GetComponent<TriggerHandling>();

        if (triggerHandler != null)
        {
            triggerHandler.InBonfireRange = false;
        }
    }
}
