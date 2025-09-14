using UnityEngine;

public class AiStatusEffectHandler : MonoBehaviour
{
   [SerializeField] private GameObject DyingGameObject;
    void AnimCallDeath()
    {
        Debug.Log("Change Da world, My final message, goodbye");
        Destroy(DyingGameObject);
    }
    void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
