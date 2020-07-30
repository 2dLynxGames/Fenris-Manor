using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{

    public GameObject enemyToSpawn;
    public bool isVisible = false;

    public bool enemyIsAlive = false;
    private bool canSpawnEnemy = true;

    void OnBecameVisible()
    {
        isVisible = true;
        Debug.Log("Spawning Enemy");
        if (canSpawnEnemy) {
            Instantiate(enemyToSpawn, transform);
            enemyIsAlive = true;
            canSpawnEnemy = false;
        }
    }

    void OnBecameInvisible()
    {
        if (!enemyIsAlive) {
            canSpawnEnemy = true;
            isVisible = false;
        }
    }

    void Update()
    {
        if (isVisible && canSpawnEnemy && !enemyIsAlive) {
            Instantiate(enemyToSpawn, transform);
            enemyIsAlive = true;
            canSpawnEnemy = false;
        }
    }
}
