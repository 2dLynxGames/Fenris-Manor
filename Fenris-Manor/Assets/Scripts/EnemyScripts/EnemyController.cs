﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    * Enemies require the following to due and take damage (in addition to Physics Object requirements)
    *      Collider2D (trigger) child - Hitbox (Layer and Tag = enemy) (should be slightly bigger than the sprite to avoid BS misses)
    *      Collider2D (trigger) child - Hurtbox (Layer and Tag = enemy) (should be slightly smaller than the model to avoid BS hits)
    *          DamagePlayer script attaches to Hurtbox
    *      
*/
public class EnemyController : PhysicsObject
{

    public enum MOVE_DIRECTION {
        left,
        right
    }

    protected EnemyData enemyData;

    protected LevelManager levelManager;    
    protected SpriteRenderer spriteRenderer;
    protected MOVE_DIRECTION moveDirection;
    protected Vector2 move;
    protected SpawnEnemy enemySpawner;
    protected DestroyObjectOverTime destroyObject;

    protected bool isAwake = false;    
    protected bool isHurt;
    protected int currentHealth;

    protected override void Awake() {
        base.Awake();
        levelManager = FindObjectOfType<LevelManager>();

        spriteRenderer = GetComponent<SpriteRenderer>();
        rb2d = GetComponent<Rigidbody2D>();
        
        if (destroyObject = GetComponent<DestroyObjectOverTime>())
            destroyObject.enabled = false;
    }

    public bool GetIsHurt(){ return isHurt; }
    public void SetIsHurt(bool isHurt){ this.isHurt = isHurt; }

    public void TakeDamage(int damageToTake) {
        if (!isHurt) {
            currentHealth -= damageToTake;
            CheckHealth();
            StartCoroutine(ResetIsHurt());
        }

    }

    public void DealDamage() {
        levelManager.playerController.TakeDamage(enemyData.damageToDo);
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

    protected void ReverseMovement() {
        if (moveDirection == MOVE_DIRECTION.right) {
            moveDirection = MOVE_DIRECTION.left;
        } else {
            moveDirection = MOVE_DIRECTION.right;
        }
        move = DetermineMoveX(moveDirection);
        FlipSprite(move.x);
    }

    protected void SetMovement() {
        moveDirection = MoveDirection();
        move = DetermineMoveX(moveDirection);
        FlipSprite(move.x);
    }

    public void CheckHealth() {
        if (currentHealth <= 0) {
            if (enemySpawner = gameObject.GetComponentInParent<SpawnEnemy>())
                enemySpawner.EnemyKilled();
            levelManager.destroyEnemySound.Play();
            Instantiate(enemyData.death, new Vector2(transform.position.x + Random.Range(-0.5f, 0.5f), transform.position.y + Random.Range(-1f, 1f)), transform.rotation);
            Destroy(gameObject);
        }
    }

    protected void WakeEnemy(float wakeDistance) {
        isAwake = Mathf.Abs(levelManager.playerController.transform.position.x - this.transform.position.x) <= wakeDistance;
        if (isAwake) {
            destroyObject.enabled = true;
        }
    }

    // If time between attacks is changed, this number should change. However, this will only likely change once or twice (if ever) throughout development 
    // so hardcoding is fine.
    IEnumerator ResetIsHurt() {
        isHurt = true;

        yield return new WaitForSecondsRealtime(0.4f);
        
        isHurt = false;
    }

    public MOVE_DIRECTION GetMoveDirection() { return this.moveDirection; }
}
