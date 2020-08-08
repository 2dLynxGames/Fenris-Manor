using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStairController : MonoBehaviour
{
    public float transitionSpeed = 3f;
    
    private float transitionPercent = 0f;
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
            levelManager.playerController.playerAnimator.SetBool("onStair", true);
            
            transitionPercent = 0f;
            StartCoroutine(MovePlayerToStairs(stairController, other.gameObject));
            Debug.Log("Player wants to climb");
        }
        //Debug.Log(this.name + " hit " + other.name);
    }

    IEnumerator MovePlayerToStairs(StairController stairController, GameObject triggerStep) {
        Vector2 playerPos = levelManager.playerController.player.transform.position;
        Vector2 stepPos = triggerStep.GetComponent<Collider2D>().bounds.center;
        if (stairController.GetStairDirection() == StairController.STAIR_DIRECTION.up) {
            // a staircase goes up if it moves from left(bottom) to right (top)
            if (triggerStep.transform.position == stairController.leftEndStep.transform.position) {
                // this means we're at the left end of a staircase that goes up
                stepPos = new Vector2(triggerStep.transform.position.x + 0.25f, triggerStep.transform.position.y + 0.5f);
            } else {
                // we must be at the right end of a staircase that goes up
                stepPos = new Vector2(triggerStep.transform.position.x - 0.25f, triggerStep.transform.position.y + 1);
            }
            Debug.Log("Stairs go up");
        } else {
            // a staircase goes down if it moves from left(top) to right(bottom)
            Debug.Log("Stairs go down");
        }
        Debug.Log(playerPos);
        Debug.Log(stepPos);
        levelManager.playerController.player.GetComponent<PhysicsObject>().enabled = false;
        while (transitionPercent < 1) {
            transitionPercent += Time.deltaTime * transitionSpeed * 2;
            Debug.Log("Moving Player to stairs");
            Debug.Log(transitionPercent);
            if (transitionPercent > 1){
                transitionPercent = 1;
            }
            //Debug.Log(playerPos);
            levelManager.playerController.player.transform.position = Vector2.Lerp(playerPos, stepPos, transitionPercent);
            yield return new WaitForEndOfFrame();
        }
        levelManager.playerController.player.GetComponent<PhysicsObject>().enabled = false;
        levelManager.playerController.playerAnimator.SetFloat("velocityX", 0);
        levelManager.playerController.playerAnimator.Play("PlayerStairsIdle");

    }
}