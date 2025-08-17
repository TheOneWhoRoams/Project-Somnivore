using UnityEngine;
using UnityEngine.Playables;

public class AnimationHandler : MonoBehaviour
{
    [SerializeField] private TriggerHandling TriggerHandler;
    [SerializeField] private PlayerMovement PlayerMovement;
    [SerializeField] private InputHandler InputHandling;
    [SerializeField] private PlayerStateHandler PlayerStateHandling;
    Animator PlayerAnimator;
    
    public enum RollType { Light, Medium, Heavy, Over };
    public RollType CurrentRollType;
    public float RollAnimationSpeed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void RollParams()
    {
        if (PlayerStateHandling.CurrentState != PlayerStateHandler.PlayerState.LandingRoll)
        {
            switch (CurrentRollType)
            {
                case RollType.Light:
                    {
                        PlayerMovement.RollSpeed = 10f;
                        RollAnimationSpeed = 1.3f; break;
                    }
                case RollType.Medium:
                    {
                        PlayerMovement.RollSpeed = 8f;
                        RollAnimationSpeed = 1f; break;
                    }
                case RollType.Heavy:
                    {
                        PlayerMovement.RollSpeed = 6f;
                        RollAnimationSpeed = 0.7f; break;
                    }
                case RollType.Over:
                    {
                        PlayerMovement.RollSpeed = 100f;
                        RollAnimationSpeed = 1f; break;
                    }
            }
        }
        else PlayerMovement.RollSpeed = 4f;
        PlayerAnimator.SetFloat("RollAnimationSpeed", RollAnimationSpeed);
    }
    void ClimbHandling()
    {
        switch (InputHandling.ClimbInput)
        {
            case -1:
                {
                    PlayClimbDescend();
                    break;
                }
            case 0:
                {
                    PlayClimbIdle();
                    break;
                }
            case 1:
                {
                    PlayClimbAscend();
                    break;
                }
        }


    }
    void PlayClimbDescend()
    {
        PlayerAnimator.SetFloat("ClimbSpeed", -1f);
        PlayerAnimator.SetBool("IsClimbing", true);
        PlayerAnimator.SetBool("IsClimbingMoving", true);
        
    }
    void PlayClimbAscend()
    {
        PlayerAnimator.SetFloat("ClimbSpeed", 1f);
        PlayerAnimator.SetBool("IsClimbing", true);
        PlayerAnimator.SetBool("IsClimbingMoving", true);
    }
    void PlayClimbIdle()
    {
        PlayerAnimator.SetBool("IsClimbing", true);
        PlayerAnimator.SetBool("IsClimbingMoving", false);
    }
    void ClimbExitHandling()
    {
        if (TriggerHandler.AnimatorWantsToExit)
        {
            PlayerAnimator.SetBool("IsClimbing", false);
            PlayerAnimator.SetBool("IsClimbingMoving", false);
            TriggerHandler.AnimatorWantsToExit = false;
            PlayerAnimator.SetTrigger("ClimbExit");
        }
    }
    public void PlayRoll()
    {
        PlayerAnimator.SetTrigger("Roll");
    }
    public void PlayJump()
    {
        PlayerAnimator.SetTrigger("Jump");
    }
    
    void AnimationHandling()
    {
        
        if (PlayerMovement.IsGroundedThisFrame)
            PlayerAnimator.SetBool("IsGrounded", true);
        else
            PlayerAnimator.SetBool("IsGrounded", false);

        switch (PlayerStateHandling.CurrentState)
        {
            case PlayerStateHandler.PlayerState.Walking:
                {
                    PlayerAnimator.SetBool("IsWalking", true);
                    PlayerAnimator.SetBool("IsSprinting", false);
                    break;
                }
            case PlayerStateHandler.PlayerState.Sprinting:
                {
                    PlayerAnimator.SetBool("IsWalking", true);
                    PlayerAnimator.SetBool("IsSprinting", true);
                    break;
                }
            case PlayerStateHandler.PlayerState.Climbing: 
                {
                    ClimbHandling();
                    break;
                }
            default:
                {
                    PlayerAnimator.SetBool("IsWalking", false);
                    PlayerAnimator.SetBool("IsSprinting", false);
                    ClimbExitHandling();
                    break;
                }

        }
        
        PlayerAnimator.SetFloat("VelocityY", PlayerMovement.VerticalVelocity);
    }
    void Start()
    {
        PlayerAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        RollParams();
        AnimationHandling();
    }
}
