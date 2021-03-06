﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public PlayerController playerController;
    public SpawnEnemy[] enemies;
    public CameraController sceneCamera;
    public AudioSource destroyEnemySound;
    public int points = 0;

    // Start is called before the first frame update
    void Awake()
    {
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        enemies = GameObject.FindObjectsOfType<SpawnEnemy>();
        sceneCamera = GameObject.FindObjectOfType<CameraController>();
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

    public void AddPoints(int pointsToAdd) {
        points += pointsToAdd;
    }
}
