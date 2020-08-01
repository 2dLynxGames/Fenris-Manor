﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : PhysicsObject
{

    public enum MOVE_DIRECTION {
        left,
        right
    }

    public int maxHealth = 1;
    public int damage = 1;
    public float moveSpeed;

    protected LevelManager levelManager;    
    protected SpriteRenderer spriteRenderer;
    protected MOVE_DIRECTION moveDirection;
    protected Vector2 move;
    protected ResetObject resetObject;

    protected int currentHealth;

    public void TakeDamage(int damageToTake) {
        currentHealth -= damageToTake;
        CheckHealth();
    }

    public void DealDamage() {
        levelManager.playerController.TakeDamage(damage);
    }


    protected MOVE_DIRECTION MoveDirection(){
        return (levelManager.playerController.player.transform.position.x > transform.position.x) ? MOVE_DIRECTION.right : MOVE_DIRECTION.left;
    }

    protected Vector2 DetermineMoveX(MOVE_DIRECTION moveDirection) {
        return (moveDirection == MOVE_DIRECTION.left) ? Vector2.left : Vector2.right;
    }

    protected void FlipSprite(float moveX){
        if (moveX > 0)
            spriteRenderer.flipX = false;
        else 
            spriteRenderer.flipX = true;
    }

    protected void SetMovement() {
        moveDirection = MoveDirection();
        move = DetermineMoveX(moveDirection);
        FlipSprite(move.x);
    }

    public void CheckHealth() {
        if (currentHealth <= 0) {
            gameObject.GetComponentInParent<SpawnEnemy>().EnemyKilled();
            Destroy(gameObject);
        }
    }
}