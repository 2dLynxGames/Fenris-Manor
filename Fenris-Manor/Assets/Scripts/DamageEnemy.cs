﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageEnemy : MonoBehaviour
{
    //public GameObject deathAnimation;

    private GameObject player;
    private PlayerController playerController;
    private EnemyController enemy;

    private int damageToDo;


    private
    // Start is called before the first frame update
    void Awake()
    {
        player = GameObject.Find("Player");
        playerController = player.GetComponent<PlayerController>();
    }

    void Start() {
        damageToDo = playerController.GetWhipDamage();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy") {
            if (((enemy = other.GetComponent<EnemyController>()) || (enemy = other.GetComponentInParent<EnemyController>())) && !enemy.GetIsHurt()){
                enemy.SetIsHurt(true);
                enemy.TakeDamage(damageToDo);
            }
        }
    }
}