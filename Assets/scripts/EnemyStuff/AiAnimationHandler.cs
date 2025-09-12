using UnityEngine;

public class AiAnimationHandler : MonoBehaviour
{
   
    

    Animator AiAnimator;
    void Start()
    {
        AiAnimator = GetComponent<Animator>();
    }
    
    public void PlayAttack_CloseRange_1()
    {
        AiAnimator.SetTrigger("Attack_CloseRange_1");
    }
    public void PlayAttack_LongRange_Charge_1()
    {
        AiAnimator.SetTrigger("Attack_LongRange_Charge_1");
    }
    public void PlayDeath()
    {
        AiAnimator.SetTrigger("Death");
    }
    public void PlaySoftStagger()
    {
        AiAnimator.SetTrigger("SoftStagger");
    }
    public void PlayHardStagger()
    {
        AiAnimator.SetTrigger("HardStagger");
    }
    void Update()
    {
        
    }
}
