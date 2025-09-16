using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
    public static EnemySpawnManager Instance { get; private set; }

    [SerializeField]private List<AiSpawner> allSpawners = new List<AiSpawner>();

    private void Awake()
    {
        // Singleton pattern
        if (Instance != null && Instance != this) { Destroy(this.gameObject); }
        else { Instance = this; }
    }

    private void OnEnable()
    {
        Debug.Log("Enemy Spawn Manager is listening for the event");
        EventManager.OnPlayerRested += HandleRespawn;
    }

    private void OnDisable()
    {
        EventManager.OnPlayerRested -= HandleRespawn;
    }

    // This is the central logic that runs when the player rests
    private void HandleRespawn()
    {
        Debug.Log("Spawn Manager heard the rest event. Respawning all enemies.");
        Debug.Log("Number of registered spawners: " + allSpawners.Count);
        // 1. Find and destroy all existing enemies
        GameObject[] existingEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in existingEnemies)
        {
            Destroy(enemy);
        }

        // 2. Tell every registered spawner to spawn its enemies
        foreach (AiSpawner spawner in allSpawners)
        {
            spawner.SpawnAllEnemies();
        }
    }
}