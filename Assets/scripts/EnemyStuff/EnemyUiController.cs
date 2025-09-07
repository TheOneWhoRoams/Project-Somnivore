// Create a new script, e.g., "EnemyUIController.cs"
// Attach it to the root of your enemy prefab.
using UnityEngine;
using UnityEngine.UI; // You need this for UI components like Slider

public class EnemyUIController : MonoBehaviour
{
    // Drag the references in the Inspector inside the prefab
    [SerializeField] private AiResourceHandler resourceHandler;
    [SerializeField] private Slider HealthSlider;
    [SerializeField] private Slider PoiseSlider;
    [SerializeField] private Slider BleedMeter;
    [SerializeField] private Slider PoisonMeter;

    // You'll need to add MaxHealth and MaxPoise to your AiResourceHandler
    // for this calculation to work.
    private float MaxHealth;
    private float MaxPoise;
    private float BleedThreshold;
    private float PoisonThreshold;

    void Start()
    {
        // Get the max values at the start
        MaxHealth = resourceHandler.SetHealth;
        MaxPoise = resourceHandler.SetPoise;
        BleedThreshold = resourceHandler.SetBleedMeter;
        PoisonThreshold = resourceHandler.SetPoisonMeter;
    }

    void Update()
    {
        // Normalize the current value to a 0-1 range for the slider
        HealthSlider.value = resourceHandler.Health / MaxHealth;
        PoiseSlider.value = resourceHandler.Poise / MaxPoise;
        BleedMeter.value = resourceHandler.Health / MaxHealth;
        PoisonMeter.value = resourceHandler.Poise / MaxPoise;
    }
}