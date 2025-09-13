using UnityEngine;

public class AiMovementController : MonoBehaviour
{
    [SerializeField] private AiStateHandler StateHandler;
    [SerializeField] private AiNavMeshHandler NavMeshHandler;
    [SerializeField] private AiCombatSubStateHandler CombatHandler;
    [SerializeField] private AiResourceHandler ResourceHandler;

    private Transform PlayerTransform;

    void Awake()
    {
        PlayerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Tick()
    {
        switch (StateHandler.CurrentAiState)
        {
            case AiStateHandler.AiState.Chasing:
                
                NavMeshHandler.MoveTo(PlayerTransform.position);
                NavMeshHandler.RotateTowards(PlayerTransform);
                break;

            case AiStateHandler.AiState.Attacking:
                switch (CombatHandler.CurrentCombatState)
                {
                    case AiCombatSubStateHandler.CombatState.Windup:
                        
                        NavMeshHandler.RotateTowards(PlayerTransform);
                        break;

                    default:
                        
                        NavMeshHandler.StopPathFinding();
                        break;
                }
                break;

            default:
                
                NavMeshHandler.StopPathFinding();
                break;
        }
    }
    // Update is called once per frame
    void Update()
    {
        Tick();
    }
}
