using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{

    public GameObject enemyToSpawn;
    public LevelManager levelManager;
    public bool hasLeftScreen = true;
    public float minimumSpawnDistance;

    public bool enemyIsAlive = false;
    private bool canSpawnEnemy = true;
    private bool playerTooClose;
    private GameObject spawnedEnemy;

    void Awake() {
        levelManager = GameObject.FindObjectOfType<LevelManager>();
    }

    void OnBecameVisible() {
        hasLeftScreen = false;
        playerTooClose = (Mathf.Abs(transform.position.x - levelManager.playerController.transform.position.x) <= minimumSpawnDistance);
        if (canSpawnEnemy && !playerTooClose) {
            Instantiate(enemyToSpawn, transform);
            enemyIsAlive = true;
            canSpawnEnemy = false;
        }
    }

    void OnBecameInvisible() {
        hasLeftScreen = true;
    }

    public void EnemyKilled() {
        canSpawnEnemy = true;
        enemyIsAlive = false;
    }
}
