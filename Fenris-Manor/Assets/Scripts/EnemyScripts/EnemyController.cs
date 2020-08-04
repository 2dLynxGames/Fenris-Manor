using System.Collections;
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

    public int maxHealth = 1;
    public int damage = 1;
    public float moveSpeed;

    protected LevelManager levelManager;    
    protected SpriteRenderer spriteRenderer;
    protected MOVE_DIRECTION moveDirection;
    protected Vector2 move;
    protected ResetObject resetObject;
    protected SpawnEnemy enemySpawner;
    protected DestroyObjectOverTime destroyObject;
    
    protected bool isHurt;

    protected int currentHealth;

    protected override void Awake() {
        base.Awake();
        levelManager = FindObjectOfType<LevelManager>();

        spriteRenderer = GetComponent<SpriteRenderer>();
        rb2d = GetComponent<Rigidbody2D>();
        
        destroyObject = GetComponent<DestroyObjectOverTime>();
        resetObject = GetComponent<ResetObject>();
        destroyObject.enabled = false;

        currentHealth = maxHealth;
    }

    public bool GetIsHurt(){ return isHurt; }
    public void SetIsHurt(bool isHurt){ this.isHurt = isHurt; }

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
            Destroy(gameObject);
        } else {
            isHurt = false;
        }
    }
}
