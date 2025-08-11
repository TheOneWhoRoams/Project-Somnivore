using UnityEngine;
using UnityEngine.Playables;

public class AnimationHandler : MonoBehaviour
{
    [SerializeField] private PlayerMovement PlayerMovement;
    [SerializeField] private InputHandler InputHandling;
    [SerializeField] private PlayerStateHandler PlayerStateHandling;
    Animator PlayerAnimator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    void TriggerHandler()
    {
        if (PlayerMovement.IsGroundedThisFrame && InputHandling.WantsToJump)
            PlayerAnimator.SetTrigger("Jump");            
        
        return;
    }
    void AnimationHandling()
    {
        
        //is the player in air?
        if (PlayerMovement.IsGroundedThisFrame)
            PlayerAnimator.SetBool("IsGrounded", true);
        else
            PlayerAnimator.SetBool("IsGrounded", false);
        //walk and sprint handling
        if (PlayerStateHandling.CurrentState == PlayerStateHandler.PlayerState.Walking)
            PlayerAnimator.SetBool("IsWalking", true);
        else
            PlayerAnimator.SetBool("IsWalking", false);

        if (PlayerStateHandling.CurrentState == PlayerStateHandler.PlayerState.Sprinting)
            PlayerAnimator.SetBool("IsSprinting", true);
        else
            PlayerAnimator.SetBool("IsSprinting", false);
        //Vertical Velocity calculations and passing


        PlayerAnimator.SetFloat("VelocityY", PlayerMovement.VerticalVelocity);

    }
    void Start()
    {
        PlayerAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        AnimationHandling();
    }
}
