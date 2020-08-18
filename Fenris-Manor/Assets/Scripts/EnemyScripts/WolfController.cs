using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfController : EnemyController
{
    private Animator wolfAnimator;
    private Vector3 awakePosition;

    private bool hasTurned = false;
    private bool hasJumped = false;
    
    public WolfData wolfData;

    protected override void Awake() {
        base.Awake();

        awakePosition = transform.position;
        wolfAnimator = GetComponent<Animator>();
        enemyData = wolfData;
    }

    void Start() {
        SetMovement();
    }

    // Update is called once per frame
    protected override void Update() {
        base.Update();
        if (!isAwake) {
            WakeEnemy(wolfData.wakeDistance);
        }
        if (isAwake && !hasTurned) {
            TurnWolf();
        }
    }
    
    protected override void ComputeVelocity() {
        if (isAwake) {
            if (!hasJumped) {
                rb2d.velocity = Vector2.up * wolfData.jumpStrength;
                hasJumped = true;
            }
            if (hasTurned) {
                targetVelocity = move * wolfData.moveSpeed * wolfData.moveSpeedMultiplier;
            } else {
                targetVelocity = move * wolfData.moveSpeed;
            }
        }
    }

    protected override void AnimateActor() {
        wolfAnimator.SetFloat("velocityY", rb2d.velocity.y);
        if (rb2d.velocity.y == 0 && hasJumped) {
            wolfAnimator.SetBool("grounded", true);
        }
    }

    void TurnWolf() {
        hasTurned = ((Mathf.Abs(Mathf.Abs(awakePosition.x) - Mathf.Abs(this.transform.position.x)) >= wolfData.turnAroundDistance) && rb2d.velocity.y == 0);
        if (hasTurned) {
            ReverseMovement();
        }
    }

}
