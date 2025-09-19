using UnityEngine;

public class OnDeathEvent : MonoBehaviour
{
    
    [SerializeField] private Transform CurrentPlayerSpawnPoint;
    public GameObject PlayerPrefab;
    private void OnEnable()
    {
        EventManager.OnPlayerRestSetSpawnPoint += SetSpawnPoint;
        EventManager.OnPlayerDeathFinish += OnDeath;
    }
    private void OnDisable()
    {
        EventManager.OnPlayerRestSetSpawnPoint -= SetSpawnPoint;
        
        EventManager.OnPlayerDeathFinish -= OnDeath;
    }
    void SetSpawnPoint(Transform SpawnPoint)
    {
        CurrentPlayerSpawnPoint = SpawnPoint;
    }
    void SpawnPlayer()
    {
        Instantiate(PlayerPrefab, CurrentPlayerSpawnPoint.position, CurrentPlayerSpawnPoint.rotation);
    }
    void OnDeath(GameObject Player)
    {
        Destroy(Player);
        SpawnPlayer();
        EventManager.RaisePlayerRestedEvent();
    }
    void Start()
    {
        SpawnPlayer();
    }

    
    void Update()
    {
        
    }
}
