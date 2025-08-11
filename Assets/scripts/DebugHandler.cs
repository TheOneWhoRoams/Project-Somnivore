using UnityEngine;

public class DebugHandler : MonoBehaviour
{

    [SerializeField] private PlayerMovement PlayerMovement;
    Animator PlayerAnimator;
    Rigidbody rb;
    void OnGUI()
    {
        if (!PlayerMovement.ShowDebug)
            return;
        float y = 10f;
        float LineHeight = 20f;

        //display animator bools
        GUI.Label(new Rect(10, y, 300, LineHeight), "IsGrounded: " + PlayerAnimator.GetBool("IsGrounded"));
        y += LineHeight;
        GUI.Label(new Rect(10, y, 300, LineHeight), "IsWalking: " + PlayerAnimator.GetBool("IsWalking"));
        y += LineHeight;
        GUI.Label(new Rect(10, y, 300, LineHeight), "IsSprinting: " + PlayerAnimator.GetBool("IsSprinting"));
        y += LineHeight;

        //display animator floats
        GUI.Label(new Rect(10, y, 300, LineHeight), "VelocityY: " + PlayerAnimator.GetFloat("VelocityY"));
        y += LineHeight;
        //other
        GUI.Label(new Rect(10, y, 300, LineHeight), "Player State: " + PlayerMovement.CurrentState);
        y += LineHeight;

        //display speed variables
        GUI.Label(new Rect(10, y, 300, LineHeight), "RollSpeed: " + PlayerMovement.RollSpeed);
        y += LineHeight;
        GUI.Label(new Rect(10, y, 300, LineHeight), "Current Speed: " + PlayerMovement.CurrSpeed);
        y += LineHeight;
        GUI.Label(new Rect(10, y, 300, LineHeight), "Current Linear Velocity: " + rb.linearVelocity);
        y += LineHeight;

    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PlayerAnimator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
