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
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerController = GetComponent<PlayerController>();
    }

    protected override void AnimateActor(){
        if(velocity.y != 0 && !playerController.GetIsHurt()) {
            playerController.SetJumpState( velocity.y > 0 ? PlayerController.JUMPING.up : PlayerController.JUMPING.down );
            playerController.playerAnimator.SetFloat("velocityY", velocity.y);
            return;
        }
        if (velocity.y == 0) {
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
                velocity.y = jumpTakeOffSpeed;
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
                velocity.y = playerController.GetKnockbackForce() / 2f;
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

    public Vector2 GetPlayerVelocity() {
        return velocity;
    }

    public void SetPlayerVelocity(Vector2 newVelocity) {
        velocity = newVelocity;
    }
}
