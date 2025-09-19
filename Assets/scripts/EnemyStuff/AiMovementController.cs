using UnityEngine;

public class AiMovementController : MonoBehaviour
{
    [SerializeField] private AiStateHandler StateHandler;
    [SerializeField] private AiNavMeshHandler NavMeshHandler;
    [SerializeField] private AiCombatSubStateHandler CombatHandler;
    [SerializeField] private AiResourceHandler ResourceHandler;
    private bool IsSearchingForPlayer = false;
    private int FrameCounter = 0;

    private Transform PlayerTransform;

    private void OnEnable()
    {
        FindPlayer();
    }

    void FindPlayer()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            PlayerTransform = playerObj.transform;
            IsSearchingForPlayer = false; 
            Debug.Log("AI has found the player.", this.gameObject);
        }
        else
        {
            IsSearchingForPlayer = true; 
        }
    }

    void Tick()
    {
        if (!IsSearchingForPlayer && PlayerTransform == null)
        {
            Debug.Log("Player reference was lost. Re-initializing search.", this.gameObject);
            IsSearchingForPlayer = true;
        }
        if (IsSearchingForPlayer)
        {
            
            FrameCounter++;
            if (FrameCounter >= 5)
            {
                FrameCounter = 0;
                Debug.Log("AI is searching for player...", this.gameObject);
                FindPlayer(); 
            }
            return; 
        }

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
