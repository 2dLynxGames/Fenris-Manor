using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPlatformerController : PhysicsObject
{
    public float maxSpeed;
    public float jumpTakeOffSpeed;

    private SpriteRenderer spriteRenderer;
    private PlayerController playerController;

    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerController = GetComponent<PlayerController>();
    }

    protected override void AnimateActor(){
        if(rb2d.velocity.y != 0 && !playerController.GetIsHurt()) {
            playerController.SetJumpState( rb2d.velocity.y > 0 ? PlayerController.JUMPING.up : PlayerController.JUMPING.down );
            playerController.playerAnimator.SetFloat("velocityY", rb2d.velocity.y);
            return;
        }
        if (rb2d.velocity.y == 0) {
            playerController.playerAnimator.SetFloat("velocityY", 0);
            playerController.SetJumpState(PlayerController.JUMPING.grounded);
        }
        if (velocity.x != 0) {
            playerController.playerAnimator.SetFloat("velocityX", Mathf.Abs(velocity.x));
            if (!playerController.GetIsHurt()) {
                FlipSprite();
            }
        }
        if (playerController.GetIsIdle() && Input.GetButton("Crouch")){
            playerController.playerAnimator.SetBool("crouched", true);
            playerController.SetCanMove(false);
            playerController.SetIsCrouching(true);
        }
        if (Input.GetButtonUp("Crouch")){
            playerController.playerAnimator.SetBool("crouched", false);
            playerController.SetCanMove(true);
            playerController.SetIsCrouching(false);
        }
        playerController.playerAnimator.SetBool("idle", playerController.GetIsIdle());
    }

    protected override void ComputeVelocity(){
        //  Reset the move vector every time this function is called
        Vector2 move = Vector2.zero;

        if (playerController.GetCanMove()) {
            move.x = Input.GetAxisRaw("Horizontal");
            if (Input.GetButtonDown("Jump") && (playerController.GetJumpState() == PlayerController.JUMPING.grounded)) {
                rb2d.velocity = Vector2.up * jumpTakeOffSpeed;
            }
            // player is in the air and moving backwards
            if (playerController.GetJumpState() != PlayerController.JUMPING.grounded && playerController.PlayerMovingBackwards(move.x)) {
                move.x *= 0.5f;
            }

            targetVelocity = move * maxSpeed;

            if(!targetVelocity.Equals(Vector2.zero) ){
                playerController.SetIsMoving(true);
            } else {
                playerController.SetIsMoving(false);
                playerController.playerAnimator.SetFloat("velocityX", 0);
            }
        } else {
            if (playerController.GetIsKnockedBack()) {
                if (playerController.GetFacing() == PlayerController.FACING.right) {
                    move.x = -1;
                }
                else {
                    move.x = 1;
                }
                rb2d.velocity = (Vector2.up * playerController.GetKnockbackForce()) / 2f;
            }
            targetVelocity = move * playerController.GetKnockbackForce();
        }
    }

    private void FlipSprite() {
        bool flipSprite = (spriteRenderer.flipX ? (velocity.x > 0.01f) : (velocity.x < 0.01f));
        if (flipSprite) {
            spriteRenderer.flipX = !spriteRenderer.flipX;
            playerController.FlipFacing();
        }
    }

}
