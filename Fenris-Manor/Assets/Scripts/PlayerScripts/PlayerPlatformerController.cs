using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPlatformerController : PhysicsObject
{
    public float maxSpeed = 7;
    public float jumpTakeOffSpeed = 7;

    private SpriteRenderer spriteRenderer;
    //private Animator animator;

    // Start is called before the first frame update
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        //animator = GetComponent<Animator>();
    }

    protected override void AnimateActor(){

        if(velocity.y != 0) {
            if (velocity.y > 0) {
                animator.SetFloat("velocityY", velocity.y);
            }
            else if(velocity.y < 0) {
                animator.SetFloat("velocityY", velocity.y);
            }
            return;
        }           
        animator.SetFloat("velocityY", 0);
        if (velocity.x != 0) {
            animator.SetFloat("velocityX", Mathf.Abs(velocity.x));
            FlipSprite();
        }
        if (playerController.GetIsIdle() && Input.GetButton("Crouch")){
            animator.SetBool("crouched", true);
            playerController.SetCanMove(false);
        }
        if (Input.GetButtonUp("Crouch")){
            animator.SetBool("crouched", false);
            playerController.SetCanMove(true);
        }
        animator.SetBool("idle", playerController.GetIsIdle());
    }

    protected override void ComputeVelocity(){
        //  Reset the move vector every time this function is called
        Vector2 move = Vector2.zero;

        if (playerController.GetCanMove()) {
            move.x = Input.GetAxisRaw("Horizontal");
            if (Input.GetButtonDown("Jump") && playerController.GetIsGrounded()) {
                velocity.y = jumpTakeOffSpeed;
            }
        
            targetVelocity = move * maxSpeed;

            if(targetVelocity.x > 0 || targetVelocity.y > 0 || targetVelocity.x < 0 || targetVelocity.y < 0){
                playerController.SetIsMoving(true);
            } else {
                playerController.SetIsMoving(false);
                animator.SetFloat("velocityX", 0);
            }
        }
    }

    private void FlipSprite() {
        bool flipSprite = (spriteRenderer.flipX ? (velocity.x > 0.01f) : (velocity.x < 0.01f));
        if (flipSprite) {
            spriteRenderer.flipX = !spriteRenderer.flipX;
            playerController.FlipFacing();
        }
    }

    public Vector2 GetPlayerVelocity() {
        return velocity;
    }

    public void SetPlayerVelocity(Vector2 newVelocity) {
        velocity = newVelocity;
    }
}
