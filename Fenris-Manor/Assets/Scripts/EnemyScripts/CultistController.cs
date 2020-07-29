using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CultistController : PhysicsObject
{
    private enum MOVE_DIRECTION {
        left,
        right
    }

    private bool isAwake;
    private LevelManager levelManager;
    private Animator cultistAnimator;
    private SpriteRenderer spriteRenderer;
    private MOVE_DIRECTION moveDirection;
    private Vector2 move;

    public float moveSpeed;
    
    // Start is called before the first frame update
    void Awake()
    {
        levelManager = FindObjectOfType<LevelManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        cultistAnimator = GetComponent<Animator>();
        rbObject=GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        moveDirection = MoveDirection();
        move = DetermineMoveX(moveDirection);
        FlipSprite(move.x);
    }
    
    protected override void ComputeVelocity(){
        if (isAwake) {
            targetVelocity = move * moveSpeed;
        }
    }

    protected override void AnimateActor(){
        if (isAwake) {
            cultistAnimator.SetBool("isAwake", true);
        }
    }

    void OnEnable()
	{
		isAwake = false;
        moveDirection = MoveDirection();
        move = DetermineMoveX(moveDirection);
        FlipSprite(move.x);
        cultistAnimator.SetBool("isAwake", false);
	}

    void OnBecameVisible()
    {
        isAwake = true;
        cultistAnimator.SetBool("isAwake", true);
    }

    private MOVE_DIRECTION MoveDirection(){
        //return (levelManager.playerController.player.transform.position.x > transform.position.x) ? MOVE_DIRECTION.right : MOVE_DIRECTION.left;
        return MOVE_DIRECTION.left;
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

}
