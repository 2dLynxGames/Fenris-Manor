using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStairController : MonoBehaviour
{
    private LevelManager levelManager;

    void Awake()
    {
        levelManager = FindObjectOfType<LevelManager>();
    }

    void OnTriggerStay2D(Collider2D other) {
        if (other.tag != "Stairs") 
            return;
        StairController stairController = other.GetComponentInParent<StairController>();

        if (Input.GetAxis("Climb") != 0 && levelManager.playerController.GetStairState() != PlayerController.STAIR_STATE.on_stair && levelManager.playerController.GetJumpState() == PlayerController.JUMPING.grounded) {
            levelManager.playerController.SetStairState(PlayerController.STAIR_STATE.on_stair);
            levelManager.playerController.ToggleControls(false);
            
            StartCoroutine(MovePlayerToStairs(stairController, other.gameObject));
            Debug.Log("Player wants to climb");
        }
        //Debug.Log(this.name + " hit " + other.name);
    }

    IEnumerator MovePlayerToStairs(StairController stairController, GameObject triggerStep) {
        Vector2 playerPos = levelManager.playerController.player.transform.position;
        Vector2 stepPos = triggerStep.transform.position;
        if (stairController.GetStairDirection() == StairController.STAIR_DIRECTION.up) {
            if (playerPos.x > stepPos.x){
                if (levelManager.playerController.GetFacing() != PlayerController.FACING.right) {
                    Debug.Log("Flip it");
                    levelManager.playerController.player.GetComponent<PlayerPlatformerController>().FlipSprite();
                }
                Debug.Log("Player is left of the step");
            } else {
                Debug.Log("Player is right of the step");
            }
            Debug.Log("Stairs go up");
        } else {
            Debug.Log("Stairs go down");
        }
        yield return new WaitForEndOfFrame();
    }
}