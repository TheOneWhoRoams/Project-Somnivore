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
   public void AnimatorEnterCombat()
    {
        PlayerAnimator.SetTrigger("EnterCombat");
    }
    public void AnimatorExitCombat()
    {
        PlayerAnimator.SetTrigger("ExitCombat");
    }
    public void PlayLightAttack()
    {
        PlayerAnimator.SetTrigger("LightAttack");
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
                    
                    break;
                }
            default:
                {
                    PlayerAnimator.SetBool("IsWalking", false);
                    PlayerAnimator.SetBool("IsSprinting", false);
                    
                    break;
                }

        }
        
        PlayerAnimator.SetFloat("VelocityY", PlayerMovement.VerticalVelocity);
    }
    void Start()
    {
        PlayerAnimator = GetComponent<Animator>();
    }

    
    void Update()
    {
        RollParams();
        AnimationHandling();
    }
}
