using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public PlayerController playerController;
    public SpawnEnemy[] enemies;

    // Start is called before the first frame update
    void Awake()
    {
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        enemies = GameObject.FindObjectsOfType<SpawnEnemy>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToggleEnemies(bool state) {
        foreach (var enemy in enemies) {
            enemy.gameObject.SetActive(state);
        }
    }
}
