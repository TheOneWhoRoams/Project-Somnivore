using UnityEngine;
using System.Collections.Generic; 

public class AiSpawner : MonoBehaviour
{
    [SerializeField] private GameObject EnemyPrefab;

    
    [SerializeField] private List<Transform> SpawnPoints;

    void Start()
    {
        SpawnAllEnemies();
    }

    void SpawnAllEnemies()
    {
        // Loop through every spawn point you've added to the list
        foreach (Transform SpawnPoint in SpawnPoints)
        {
            if (EnemyPrefab != null && SpawnPoint != null)
            {
                // Instantiate an enemy at each spawn point's position
                Instantiate(EnemyPrefab, SpawnPoint.position, SpawnPoint.rotation);
            }
            else
            {
                Debug.LogWarning("A spawn point or the enemy prefab is not assigned!");
            }
        }
    }
}