﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Players require the following to take damage:
 *      Collider2D (trigger) child - PlayerHitbox (layer/tag Player)
 *      
*/
public class PlayerController : MonoBehaviour
{
    /*
     *  This will be the overarching PlayerController class, this will hold values used by all the other classes and be the only one with values expsed in the 
     *  editor. Determining how to organize these so that they are meaningful and grouping them so that they can be expanded and collapsed when in use will be 
     *  important. Initially will hold things like 'Player Max Speed' and 'Player Jump Force" but will also contains state_based things for use with both 
     *  animation and logic. This could be enumerators like Stair_State or values such as whip level / sub weapon level.
     *  
     *  There is a consideration for the possibility of renaming this class simply to the Player class, as it will contain the values for all the scripts and
     *  states of the player throughout the course of gameplay and provides no controls. This change is minor and only in name and not super relevant in the
     *  long run, mostly this is just for the purpose of maintaining good object oriented practices, which is important.
     */

    public GameObject player; 
    public Animator playerAnimator;
    public SpriteRenderer spriteRenderer;
    public AudioSource whipSound;
    public AudioSource hurtSound;
    
    protected const float bufferZone = 0.25f;
    protected const float whipHeight = 0.5f;
    protected const float crouchReduction = 0.4f;

    protected float whipLength = 2.4f;

    [SerializeField]
    protected float knockbackForce;

    public enum STAIR_STATE {
        on_stair,
        off_stair
    }

    public enum FACING {
        left,
        right
    }

    public enum WHIP_LEVEL {
        basic = 1,
        chain = 2,
        extended_chain = 3
    }

    public enum JUMPING {
        up,
        down,
        grounded
    }

    // declare variables and initialize values for all enums
    protected STAIR_STATE stairState = STAIR_STATE.off_stair;
    protected FACING facing = FACING.right;
    protected WHIP_LEVEL whipLevel = WHIP_LEVEL.basic;
    protected JUMPING jumpState = JUMPING.grounded;

    // declare boolean variables and initialize values to describe the state of the player
    protected bool isClimbing = false;
    protected bool isCrouching = false;
    protected bool isAttacking = false;
    protected bool isMoving = false;
    protected bool isIdle = true;
    protected bool isHurt = false;
    protected bool isKnockedBack = false;
    protected bool canMove = true;
    protected bool inDoor = false;

    protected Collider2D whipHitbox;

    protected int whipDamage = 0;
    
    public int startingHealth = 10;
    protected int health;
    private bool canTakeDamage = true;

    void Awake(){
        player = GameObject.Find("Player");
        playerAnimator = player.GetComponent<Animator>();
        spriteRenderer = player.GetComponent<SpriteRenderer>();
        whipDamage = DetermineWhipDamage(whipLevel);
        whipLength = DetermineWhipLength(whipLevel);
        health = startingHealth;
    }

    void Update(){
        SetIsIdle();
    }

    // public getters and setters for every state variable of the player

    public STAIR_STATE GetStairState(){ return stairState; }
    public void SetStairState(STAIR_STATE stairState){ this.stairState = stairState; }

    public FACING GetFacing(){ return facing; }
    public void SetFacing(FACING facing) { this.facing = facing; }

    public WHIP_LEVEL GetWhipLevel(){ return whipLevel; }
    public void SetWhipLevel(WHIP_LEVEL whipLevel){
        this.whipLevel = whipLevel;
        whipDamage = DetermineWhipDamage(whipLevel);
    }

    public JUMPING GetJumpState(){ return jumpState; }
    public void SetJumpState(JUMPING jumpState){
        this.jumpState = jumpState;
        if (jumpState == JUMPING.grounded) {
            playerAnimator.SetBool("grounded", true);
        } else {
            playerAnimator.SetBool("grounded", false);
        }
    }

    public Collider2D GetWhipHitbox(){ return whipHitbox; }
    public void SetWhipHitbox(Collider2D whipHitbox){ this.whipHitbox = whipHitbox; }

    public bool GetIsClimbing(){ return isClimbing; }
    public void SetIsClimbing(bool climbing){ isClimbing = climbing; }

    public bool GetIsCrouching(){ return isCrouching; }
    public void SetIsCrouching(bool isCrouching){ this.isCrouching = isCrouching; }

    public bool GetIsAttacking(){ return isAttacking; }
    public void SetIsAttacking(bool attacking){ isAttacking = attacking; }

    public bool GetIsIdle(){ return isIdle; }
    public void SetIsIdle(){ isIdle = CheckIdle(); }

    public bool GetIsMoving(){ return isIdle; }
    public void SetIsMoving(bool isMoving){ this.isMoving = isMoving; }

    public bool GetIsHurt(){ return isHurt; }
    public void SetIsHurt(bool isHurt){ this.isHurt = isHurt; }

    public bool GetCanMove(){ return canMove; }
    public void SetCanMove(bool canMove){ this.canMove = canMove; }

    public bool GetIsKnockedBack() { return isKnockedBack; }
    public void SetIsKnockedBack(bool isKnockedBack) { this.isKnockedBack = isKnockedBack; }

    public bool GetInDoor() { return inDoor; }
    public void SetInDoor(bool inDoor) { this.inDoor = inDoor; }

    public int GetWhipDamage() { return whipDamage; }
    public void SetWhipDamage(int whipDamage) { this.whipDamage = whipDamage; }

    public float GetKnockbackForce() { return knockbackForce; }
    public void SetKnockbackForce(float knockbackForce) { this.knockbackForce = knockbackForce; }

    public float GetWhipLength() { return whipLength; }
    public void SetWhipLength(float whipLength) { this.whipLength = whipLength; }

    public float GetBufferZone() { return bufferZone; }
    public float GetWhipHeight() { return whipHeight; }
    public float GetCrouchReduction() { return crouchReduction; }

    //#Helpers
    private bool CheckIdle() {
        return !(!(jumpState == JUMPING.grounded) || isMoving || isAttacking);
    }

    public void FlipFacing() {
        if (facing == PlayerController.FACING.right) {
            facing = PlayerController.FACING.left;
        } else {
            facing = PlayerController.FACING.right;
        }
    }

    public bool PlayerMovingBackwards(float move) {
        return ((facing == FACING.right && move < 0.1f) || (facing == FACING.left && move > 0.1f));
    }

    public void TakeDamage(int damageToTake) {
        if (canTakeDamage) {
            canTakeDamage = false;
            StartCoroutine(TakeDamage());
            health -= damageToTake;
        }
    }

    private int DetermineWhipDamage(WHIP_LEVEL whipLevel) {
        switch (whipLevel) {
            case WHIP_LEVEL.basic:
                return 1;
            case WHIP_LEVEL.chain:
                return 2;
            case WHIP_LEVEL.extended_chain:
                return 3;
            default:
                return 1;
        }
    }

    private float DetermineWhipLength(WHIP_LEVEL whipLevel) {
        switch (whipLevel) {
            case WHIP_LEVEL.basic:
            case WHIP_LEVEL.chain:
                return 2.4f;
            case WHIP_LEVEL.extended_chain:
            //TODO: Determine whipLength for level 3 whip. 3 is a placeholder until testing can be done.
                return 3f;
            default:
                return 2.4f;
        }
    }

    IEnumerator TakeDamage() {
        StartCoroutine(PlayerHurt(0.4f));
        isHurt = true;
        canMove = false;
        isKnockedBack = true;
        isAttacking = true; // effectively canAttack = false
        hurtSound.Play();
        yield return new WaitForSecondsRealtime(0.3f);
        isKnockedBack = false;

        canMove = true;
        StartCoroutine(PlayerFlash(0.4f, 4));

        isAttacking = false; // effectively canAttack = true
        yield return new WaitForSecondsRealtime(0.6f);

        isHurt = false;
        canTakeDamage = true;
    }

    IEnumerator PlayerHurt (float hurtDuration) {
        playerAnimator.SetBool("hurt", true);
        yield return new WaitForSecondsRealtime(hurtDuration);
        playerAnimator.SetBool("hurt", false);
    }

    public IEnumerator DisableControls(GameObject player, float timeToWait) {
        ToggleControls(false);
        yield return new WaitForSecondsRealtime(timeToWait);
        ToggleControls(true);
    }

    /*
     * Toggle the platforming controlls for the player.
     * bool state should represent the state of the controls for the player
     * use false to disable, true to enable
    */
    public void ToggleControls(bool state) {
        player.GetComponent<PlayerPlatformerController>().enabled = state;
        playerAnimator.SetBool("idle", !state);
    }

    IEnumerator PlayerFlash(float flashDuration, int numFlashes) {
        var transparent = new Color32(255, 255, 255, 0);
        for (float i = 0f; i <= flashDuration; i += flashDuration / (float)numFlashes) {
            player.GetComponent<SpriteRenderer>().material.color = Color.white;
            yield return new WaitForSecondsRealtime(flashDuration / (numFlashes * 2f));
            player.GetComponent<SpriteRenderer>().material.color = transparent;
            yield return new WaitForSecondsRealtime(flashDuration / (numFlashes * 2f));
        }
        player.GetComponent<SpriteRenderer>().material.color = Color.white;
    }

    public void FlipSprite(int directionToMove) {
        bool flipSprite = (spriteRenderer.flipX ? (directionToMove > 0) : (directionToMove < 0));
        if (flipSprite) {
            spriteRenderer.flipX = !spriteRenderer.flipX;
            FlipFacing();
        }
    }
}
