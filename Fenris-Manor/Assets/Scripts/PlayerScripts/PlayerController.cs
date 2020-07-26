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

    public GameObject Player;
    public Animator PlayerAnimator;
    
    public enum PLAYER_STATE {
        crouch,
        walking,
        up_stairs,
        down_stairs,
        jump_up,
        jump_down,
        damage,
        idle
    }

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

    // create values for all the previous enums
    protected PLAYER_STATE playerState = PLAYER_STATE.idle;
    protected STAIR_STATE stairState = STAIR_STATE.off_stair;
    protected FACING facing = FACING.right;
    protected WHIP_LEVEL whipLevel = WHIP_LEVEL.basic;
    protected JUMPING jumpState = JUMPING.grounded;

    // create boolean values to allow other classes to change thes tate of the player
    // considerations are being made as to whether to make this an enum like the two above
    // this would give us more control, however players can exist in two states at once for many of the states on this list
    // likely it is better for each of them to remain as individual boolean variables
    protected bool isClimbing = false;
    protected bool isAttacking = false;
    protected bool isMoving = false;
    protected bool isIdle = true;
    protected bool isHurt = false;
    protected bool isGrounded = true;
    protected bool canMove = true;

    void Awake(){
        Player = GameObject.Find("Player");
        PlayerAnimator = Player.GetComponent<Animator>();
    }

    void Update(){
        SetIsIdle();
    }

    // create public getters and setters for anything that another class will need access to
    public PLAYER_STATE GetPlayerState(){
        return playerState;
    }

    public void SetStairState(PLAYER_STATE playerState){
        this.playerState = playerState;
    }

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
    }

    public JUMPING GetJumpState(){
        return jumpState;
    }
    public void SetJumpState(JUMPING jumpState){
        this.jumpState = jumpState;
    }

    public bool GetIsClimbing(){
        return isClimbing;
    }
    public void SetIsClimbing(bool climbing){
        isClimbing = climbing;
    }

    public bool GetIsGrounded(){
        return isGrounded;
    }

    public void SetIsGrounded(bool isGrounded){
        this.isGrounded = isGrounded;
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
}
