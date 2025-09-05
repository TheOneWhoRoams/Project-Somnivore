// Create a new script called "Billboard.cs"
// Attach this script to the EnemyHUD Canvas object.
using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    // LateUpdate runs after all other Update calls, which is best for camera work.
    void LateUpdate()
    {
        // Make this object's forward direction point the same as the camera's
        transform.forward = mainCamera.transform.forward;
    }
}