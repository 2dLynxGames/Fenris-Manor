using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CultistController : PhysicsObject, IEnemyController
{
    public int maxHealth = 1;
    public int damage = 1;

    private enum MOVE_DIRECTION {
        left,
        right
    }

    private bool isAwake = false;
    private bool dead = false;

    private LevelManager levelManager;
    private Animator cultistAnimator;
    private SpriteRenderer spriteRenderer;
    private MOVE_DIRECTION moveDirection;
    private Vector2 move;
    private ResetObject resetObject;

    private int health;

    public float moveSpeed;
    
    // Start is called before the first frame update
    void Awake()
    {
        levelManager = FindObjectOfType<LevelManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        cultistAnimator = GetComponent<Animator>();
        rbObject = GetComponent<Rigidbody2D>();
        resetObject = GetComponent<ResetObject>();
        health = maxHealth;
    }

    void Start()
    {
        SetMovement();
    }
    
    protected override void ComputeVelocity(){
        if (isAwake && !dead) {
            targetVelocity = move * moveSpeed;
        }
    }

    protected override void AnimateActor(){
        if (isAwake && !dead) {
            cultistAnimator.SetBool("isAwake", true);
        }
    }

    void OnEnable()
	{
        isAwake = false;
        cultistAnimator.SetBool("isAwake", false);
	}

    void OnBecameVisible()
    {
        isAwake = true;
        cultistAnimator.SetBool("isAwake", true);
    }

    public void TakeDamage(int damageToTake) {
        health -= damageToTake;
        CheckHealth();
    }

    public void DealDamage() {
        levelManager.playerController.TakeDamage(damage);
    }

    public bool GetIsDead() {
        return dead;
    }

    public void SetIsDead(bool dead){
        this.dead = dead;
        if (dead == false) {
            SetMovement();
        }
    }

    public void CheckHealth() {
        if (health <= 0) {
            dead = true;
            gameObject.GetComponentInParent<SpawnEnemy>().EnemyKilled();
            Destroy(gameObject);
        }
    }

    private MOVE_DIRECTION MoveDirection(){
        return (levelManager.playerController.player.transform.position.x > transform.position.x) ? MOVE_DIRECTION.right : MOVE_DIRECTION.left;
    }

    private Vector2 DetermineMoveX(MOVE_DIRECTION moveDirection) {
        return (moveDirection == MOVE_DIRECTION.left) ? Vector2.left : Vector2.right;
    }

    private void FlipSprite(float moveX){
        if (moveX > 0)
            spriteRenderer.flipX = false;
        else 
            spriteRenderer.flipX = true;
    }

    private void SetMovement() {
        moveDirection = MoveDirection();
        move = DetermineMoveX(moveDirection);
        FlipSprite(move.x);
    }

}
