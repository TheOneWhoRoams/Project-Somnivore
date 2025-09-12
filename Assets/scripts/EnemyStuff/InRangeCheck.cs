using UnityEngine;

public class InCloseRangeCheck : MonoBehaviour
{
    public enum CheckType { CloseRange, AttackRange, EngageRange};
    public CheckType Check;
    bool InTrigger;
    
    AiStateHandler AiStateHandler;
    

    void Awake()
    {
        AiStateHandler = GetComponentInParent<AiStateHandler>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            InTrigger = true;
            
        }
            
        
        
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            InTrigger= false;
            
        
    }
    
    void Update()
    {
        switch (Check)
        {
            case CheckType.CloseRange:
                AiStateHandler.InCloseRange = InTrigger;
                
                break;
            case CheckType.AttackRange:
                AiStateHandler.InAttackRange = InTrigger;
                
                break;
            case CheckType.EngageRange:
                AiStateHandler.InEngageRange = InTrigger;
                
                break;
        }
        
    }
}
