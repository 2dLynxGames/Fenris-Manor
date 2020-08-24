using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatController : EnemyController
{
    public BatData batData;

    private Animator batAnimator;
    private Vector3 awakePosition;
    private bool movedDown = false;
    private bool hasTurned = false;

    protected override void Awake() {
        base.Awake();

        batAnimator = GetComponent<Animator>();
        enemyData = batData;

        rb2d.gravityScale = 0;
        currentHealth = batData.health;
        awakePosition = transform.position;
    }

    protected override void Update() {
        base.Update();
        if (!isAwake) {
            WakeBat(batData.wakeDistance, batData.wakeHeight);
        }
        if (isAwake) {
            if (transform.position.y > levelManager.playerController.transform.position.y + 1) {
                MoveDown();
            }
            if (!hasTurned) {
                TurnBat();
            }
        }
    }

    void Start() {
        SetMovement();
    }

    protected override void ComputeVelocity() {
        if (isAwake) {
            targetVelocity = move * batData.moveSpeed;
        } 

    }

    protected override void AnimateActor() {
        if (isAwake) {
            batAnimator.SetBool("awake", true);
        }
    }

    void WakeBat(float wakeDistance, float wakeHeight) {
        isAwake = (Mathf.Abs(levelManager.playerController.transform.position.x - this.transform.position.x) <= wakeDistance) || (Mathf.Abs(levelManager.playerController.transform.position.y - this.transform.position.y) <= wakeHeight);
        if (isAwake) {
            destroyObject.enabled = true;
        }
    }

    void MoveDown() {
        if (!movedDown) {
            transform.position = new Vector2(transform.position.x, transform.position.y - 1.5f);
            StartCoroutine(ResetMoveDown());
        }
    }

    void TurnBat() {
        hasTurned = (Mathf.Abs(Mathf.Abs(awakePosition.x) - Mathf.Abs(this.transform.position.x)) >= batData.turnDistance);
        if (hasTurned) {
            ReverseMovement();
        }
    }

    IEnumerator ResetMoveDown() {
        movedDown = true;

        yield return new WaitForSecondsRealtime(0.5f);

        movedDown = false;
    }
}
