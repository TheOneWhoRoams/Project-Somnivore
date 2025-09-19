using UnityEngine;

public class OnDeathEvent : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
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

    // Update is called once per frame
    void Update()
    {
        
    }
}
