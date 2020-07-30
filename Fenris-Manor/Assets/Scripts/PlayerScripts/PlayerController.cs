using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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
    public Collider2D baseWhipHitbox;
    public Collider2D upgradedWhipHitbox;

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
        grounded,
        in_air
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
    protected bool canMove = true;

    protected Collider2D whipHitbox;

    protected int whipDamage = 0;
    
    private int startingHealth = 10;
    protected int health;

    void Awake(){
        player = GameObject.Find("Player");
        playerAnimator = player.GetComponent<Animator>();
        whipDamage = DetermineWhipDamage(whipLevel);
        health = startingHealth;
        whipHitbox = DetermineWhipHitbox(whipLevel);
    }

    void Update(){
        SetIsIdle();
    }

    // public getters and setters for every state variable of the player

    public STAIR_STATE GetStairState(){
        return stairState;
    }

    public void SetStairState(STAIR_STATE stairState){
        this.stairState = stairState;
    }

    public FACING GetFacing(){
        return facing;
    }

    public void SetFacing(FACING facing) {
        this.facing = facing;
    }

    public WHIP_LEVEL GetWhipLevel(){
        return whipLevel;
    }

    public void SetWhipLevel(WHIP_LEVEL whipLevel){
        this.whipLevel = whipLevel;
        whipDamage = DetermineWhipDamage(whipLevel);
    }

    public JUMPING GetJumpState(){
        return jumpState;
    }

    public void SetJumpState(JUMPING jumpState){
        this.jumpState = jumpState;
        if (jumpState == JUMPING.grounded) {
            playerAnimator.SetBool("grounded", true);
        } else {
            playerAnimator.SetBool("grounded", false);
        }
    }

    public Collider2D GetWhipHitbox(){
        return whipHitbox;
    }

    public void SetWhipHitbox(Collider2D whipHitbox){
        this.whipHitbox = whipHitbox;
    }

    public bool GetIsClimbing(){
        return isClimbing;
    }
    public void SetIsClimbing(bool climbing){
        isClimbing = climbing;
    }

    public bool GetIsCrouching(){
        return isCrouching;
    }
    public void SetIsCrouching(bool isCrouching){
        this.isCrouching = isCrouching;
    }

    public bool GetIsAttacking(){
        return isAttacking;
    }
    public void SetIsAttacking(bool attacking){
        isAttacking = attacking;
    }

    public bool GetIsIdle(){
        return isIdle;
    }
    public void SetIsIdle(){
        isIdle = CheckIdle();
    }

    public bool GetIsMoving(){
        return isIdle;
    }
    public void SetIsMoving(bool isMoving){
        this.isMoving = isMoving;
    }

    public bool GetIsHurt(){
        return isHurt;
    }

    public void SetIsHurt(bool isHurt){
        this.isHurt = isHurt;
    }

    public bool GetCanMove(){
        return canMove;
    }

    public void SetCanMove(bool canMove){
        this.canMove = canMove;
    }

    public int GetWhipDamage() {
        return whipDamage;
    }

    public void SetWhipDamage(int whipDamage) {
        this.whipDamage = whipDamage;
    }

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
        health -= damageToTake;
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

    private Collider2D DetermineWhipHitbox(WHIP_LEVEL whipLevel) {
        switch (whipLevel) {
            case WHIP_LEVEL.basic:
            case WHIP_LEVEL.chain:
                return baseWhipHitbox;
            case WHIP_LEVEL.extended_chain:
                return upgradedWhipHitbox;
            default:
                return baseWhipHitbox;
        }
    }
}
