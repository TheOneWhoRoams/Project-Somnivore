using UnityEngine;
using UnityEngine.UI;

public class PlayerHudHandler : MonoBehaviour
{

    [SerializeField] private Slider HealthBar;
    [SerializeField] private Slider StaminaBar;
    [SerializeField] private ResourceHandler PlayerResource;
    
    void Start()
    {
        
    }

    
    void Update()
    {
        if (PlayerResource != null)
        {
            HealthBar.value = PlayerResource.Health / PlayerResource.SetHealth;
            StaminaBar.value = PlayerResource.Stamina / PlayerResource.SetStamina;
        }
        
    }
}
