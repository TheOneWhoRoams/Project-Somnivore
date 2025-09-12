using UnityEngine;
using UnityEngine.UI;

public class PlayerHudHandler : MonoBehaviour
{

    [SerializeField] private Slider HealthBar;
    [SerializeField] private Slider StaminaBar;
    [SerializeField] private ResourceHandler PlayerResource;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerResource != null)
        {
            HealthBar.value = PlayerResource.Health / PlayerResource.SetHealth;
            StaminaBar.value = PlayerResource.Stamina / PlayerResource.SetStamina;
        }
        
    }
}
