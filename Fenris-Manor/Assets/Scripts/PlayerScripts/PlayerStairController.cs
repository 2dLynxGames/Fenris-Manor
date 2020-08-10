using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStairController : MonoBehaviour
{
    public float transitionSpeed = 3f;
    public float climbSpeed = 5f;
    
    private float transitionPercent = 0f;
    private LevelManager levelManager;
    private GameObject player;
    private PlayerPlatformerController playerPlatformerController;
    private float oldGravityScale;
    private bool isMovingToStairs = false;
    private bool isMovingOnStairs = false;
    private bool isMovingOffStairs = false;
    private bool hasLeftStairTrigger = true;
    private StairController stairController;
    private Vector2 playerPos;

    private delegate void RunAfterMoving();
    private RunAfterMoving runAfterMoving;

    void Awake() {
        levelManager = FindObjectOfType<LevelManager>();
        player = GameObject.Find("Player");
        playerPlatformerController = player.GetComponent<PlayerPlatformerController>();
        oldGravityScale = player.GetComponent<Rigidbody2D>().gravityScale;
    }

    void FixedUpdate() {
        if (levelManager.playerController.GetStairState() == PlayerController.STAIR_STATE.on_stair && stairController != null && !isMovingToStairs) {
            if (Input.GetAxis("Horizontal") > 0.1f && !isMovingOnStairs) {
                MoveOnStairs(1, stairController.GetStairDirection());
            } else if (Input.GetAxis("Horizontal") < -0.1f && !isMovingOnStairs) {
                MoveOnStairs(-1, stairController.GetStairDirection());
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.tag != "Stairs") {
            return;
        }

        runAfterMoving = MovePlayerOffStairs;
    }
    
    void OnTriggerExit2D(Collider2D other) {
        if (other.tag != "Stairs" || levelManager.playerController.GetStairState() == PlayerController.STAIR_STATE.on_stair) {
            return;
        }

        hasLeftStairTrigger = true;
    }

    void OnTriggerStay2D(Collider2D other) {
        if (other.tag != "Stairs") 
            return;
        stairController = other.GetComponentInParent<StairController>();

        if (Input.GetAxis("Climb") != 0 && levelManager.playerController.GetStairState() != PlayerController.STAIR_STATE.on_stair && levelManager.playerController.GetJumpState() == PlayerController.JUMPING.grounded && hasLeftStairTrigger) {
            hasLeftStairTrigger = false;
            player.GetComponent<Rigidbody2D>().gravityScale = 0;
            levelManager.playerController.SetStairState(PlayerController.STAIR_STATE.on_stair);
            levelManager.playerController.playerAnimator.SetBool("onStair", true);
            
            transitionPercent = 0f;
            MovePlayerToStairs(stairController, other.gameObject);
        }
    }

    void MovePlayerOffStairs() {
        if (levelManager.playerController.GetStairState() == PlayerController.STAIR_STATE.on_stair) {
            if (Input.GetAxis("Horizontal") > 0.1f || (Input.GetAxis("Climb") > 0.1f && stairController.GetStairDirection() == StairController.STAIR_DIRECTION.up)) {
                MovePlayerOffStairs(1);
            } else if (Input.GetAxis("Horizontal") < -0.1f || Input.GetAxis("Climb") < -0.1f && (stairController.GetStairDirection() == StairController.STAIR_DIRECTION.down))
                MovePlayerOffStairs(-1);
        }
    }

    void MovePlayerToStairs(StairController stairController, GameObject triggerStep) {
        isMovingToStairs = true;
        isMovingOnStairs = false;
        playerPos = levelManager.playerController.player.transform.position;
        Vector2 targetPos = triggerStep.GetComponent<Collider2D>().bounds.center;
        if (stairController.GetStairDirection() == StairController.STAIR_DIRECTION.up) {
            // a staircase goes up if it moves from left(bottom) to right (top)
            if (triggerStep.transform.position == stairController.leftEndStep.transform.position) {
                // this means we're at the left(bottom) end of a staircase that goes up
                targetPos = DeterminePlayerTarget(triggerStep, 0.25f, 0.5f);
            } else {
                // we must be at the right(top) end of a staircase that goes up
                targetPos = DeterminePlayerTarget(triggerStep, -0.75f, 0.5f);
            }
        } else {
            // a staircase goes down if it moves from left(top) to right(bottom)
            if (triggerStep.transform.position == stairController.leftEndStep.transform.position) {
                // this means we're at the left(top) end of a staircase that goes down
                targetPos = DeterminePlayerTarget(triggerStep, 0.75f, 0.5f);
            } else {
                // we must be at the right(top) end of a staircase that goes up
                targetPos = DeterminePlayerTarget(triggerStep, -0.25f, 0.5f);
            }
        }
        player.GetComponent<PhysicsObject>().enabled = false;
        runAfterMoving = RunAfterMovingToStairs;
        StartCoroutine(MovePlayer(targetPos, transitionSpeed));
    }

    void MoveOnStairs(int directionToMove, StairController.STAIR_DIRECTION stairDirection) {
        isMovingOnStairs = true;
        playerPos = levelManager.playerController.player.transform.position;
        Vector2 targetPos = Vector2.zero;
        
        //target.x is the same regardless of stairDirection
        targetPos.x = playerPos.x + MoveXOnStairs(directionToMove);

        //target.y is determined by move direction and stair direction
        //target.y is the same as stair direction on right and inverted on left
        targetPos.y = playerPos.y + moveYOnStairs(directionToMove, stairDirection);
        
        if ((!isMovingToStairs || !isMovingOffStairs) && !CheckBound(targetPos.x, directionToMove)) {
            runAfterMoving = RunAfterMovingOnStairs;
            StartCoroutine(MovePlayer(targetPos, climbSpeed));
        } else {
            isMovingOnStairs = false;
        }
    }

    void MovePlayerOffStairs(int directionToMove) {
        isMovingOffStairs = true;
        Vector2 targetPos = Vector2.zero;
        playerPos = levelManager.playerController.player.transform.position;
        if (directionToMove == 1) {
            targetPos.x = playerPos.x + 1.5f;
            targetPos.y = (stairController.GetStairDirection() == StairController.STAIR_DIRECTION.up) ? playerPos.y : playerPos.y - 0.5f;
        } else {
            targetPos.x = playerPos.x + -1.5f;
            targetPos.y = (stairController.GetStairDirection() == StairController.STAIR_DIRECTION.down) ? playerPos.y : playerPos.y - 0.5f;
        }
        if (stairController.GetStairDirection() == StairController.STAIR_DIRECTION.up) {
            targetPos.y = playerPos.y;
        }
        runAfterMoving = RunAfterMovingOffStairs;
        StartCoroutine(MovePlayer(targetPos, transitionSpeed));
    }

    void RunAfterMovingToStairs() {
        levelManager.playerController.playerAnimator.SetFloat("velocityX", 0);
        levelManager.playerController.playerAnimator.Play("PlayerStairsIdle");
        isMovingToStairs = false;
    }

    void RunAfterMovingOnStairs() {
        isMovingOnStairs = false;
    }

    void RunAfterMovingOffStairs() {
        player.GetComponent<PhysicsObject>().enabled = true;
        player.GetComponent<Rigidbody2D>().gravityScale = oldGravityScale;
        levelManager.playerController.SetStairState(PlayerController.STAIR_STATE.off_stair);
    }

    bool CheckBound(float targetX, int directionToMove) {
        if (directionToMove == 1) {
            return (targetX > stairController.rightEndStep.transform.position.x - 0.5f);
        } else {
            Debug.Log(targetX);
            Debug.Log(stairController.leftEndStep.transform.position.x);
            return (targetX < stairController.leftEndStep.transform.position.x - 1f);
        }
    }

    bool CheckRightBound(Vector2 targetMove) {
        return (targetMove.x > stairController.rightEndStep.transform.position.x);
    }

    IEnumerator MovePlayer(Vector2 targetPos, float transitionSpeed) {
        transitionPercent = 0;
        while (transitionPercent < 1) {
            transitionPercent += Time.deltaTime * climbSpeed;
            if (transitionPercent > 1) {
                transitionPercent = 1;
            }
            levelManager.playerController.transform.position = Vector2.Lerp(playerPos, targetPos, transitionPercent);
            yield return new WaitForFixedUpdate();
        }
        runAfterMoving();
    }

    float MoveXOnStairs(int directionToMove) {
        return (directionToMove == 1) ? 0.5f : -0.5f;
    }

    float moveYOnStairs (int directionToMove, StairController.STAIR_DIRECTION stairDireciton) {
        if (stairDireciton == StairController.STAIR_DIRECTION.up) {
            // if player is moving right on upwards stairs they move up, otherwise they move down
            return (directionToMove == 1) ? 0.5f : -0.5f;
        }
        // if player is moving right on downwards stairs they move down, otherwise they move up
        return (directionToMove == 1) ? -0.5f : 0.5f;

    }

    Vector2 DeterminePlayerTarget(GameObject triggerStep, float xOffset, float yOffset) {
        return new Vector2(triggerStep.transform.position.x + xOffset, triggerStep.transform.position.y + yOffset);
    }
}