using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{

    public GameObject enemyToSpawn;
    public bool hasLeftScreen = true;

    public bool enemyIsAlive = false;
    private bool canSpawnEnemy = true;
    private GameObject spawnedEnemy;

    void OnBecameVisible() {
        hasLeftScreen = false;
        if (canSpawnEnemy) {
            Debug.Log("Spawning Enemy");
            Instantiate(enemyToSpawn, transform);
            enemyIsAlive = true;
            canSpawnEnemy = false;
        }
    }

    void OnBecameInvisible() {
        Debug.Log("Became invisible");
        hasLeftScreen = true;
    }

    public void EnemyKilled() {
        canSpawnEnemy = true;
        enemyIsAlive = false;
    }
}
