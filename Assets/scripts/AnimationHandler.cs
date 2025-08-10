using UnityEngine;
using UnityEngine.Playables;

public class AnimationHandler : MonoBehaviour
{
    [SerializeField] private PlayerMovement PlayerMovement;
    Animator PlayerAnimator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    void AnimationHandling()
    {
        
        //is the player in air?
        if (PlayerMovement.IsGroundedThisFrame)
            PlayerAnimator.SetBool("IsGrounded", true);
        else
            PlayerAnimator.SetBool("IsGrounded", false);
        //walk and sprint handling
        if (PlayerMovement.CurrentState == PlayerMovement.PlayerState.Walking)
            PlayerAnimator.SetBool("IsWalking", true);
        else
            PlayerAnimator.SetBool("IsWalking", false);

        if (PlayerMovement.CurrentState == PlayerMovement.PlayerState.Sprinting)
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
