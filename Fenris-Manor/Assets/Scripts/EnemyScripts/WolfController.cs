using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfController : EnemyController
{
    public float wakeDistance;
    public float turnAroundDistance;
    public float jumpStrength;
    public float moveSpeedMultiplier = 1.2f;

    private Animator wolfAnimator;
    private Vector3 awakePosition;

    private bool isAwake = false;
    private bool hasTurned = false;
    private bool hasJumped = false;

    protected override void Awake() {
        base.Awake();

        awakePosition = transform.position;
        wolfAnimator = GetComponent<Animator>();
    }

    void Start() {
        SetMovement();
    }

    // Update is called once per frame
    void Update() {
        if (!isAwake) {
            WakeWolf();
        }
        if (isAwake && !hasTurned) {
            TurnWolf();
        }
        ComputeVelocity();
        AnimateActor();
    }
    
    protected override void ComputeVelocity() {
        if (isAwake) {
            if (!hasJumped) {
                rb2d.velocity = Vector2.up * jumpStrength;
                hasJumped = true;
            }
            if (hasTurned) {
                targetVelocity = move * moveSpeed * moveSpeedMultiplier;
            } else {
                targetVelocity = move * moveSpeed;
            }
        }
    }

    protected override void AnimateActor(){
        wolfAnimator.SetFloat("velocityY", rb2d.velocity.y);
        if (rb2d.velocity.y == 0 && hasJumped) {
            wolfAnimator.SetBool("grounded", true);
        }
    }

    void WakeWolf() {
        isAwake = Mathf.Abs(levelManager.playerController.transform.position.x - this.transform.position.x) <= wakeDistance;
        if (isAwake) {
            destroyObject.enabled = true;
        }
    }

    void TurnWolf() {
        hasTurned = ((Mathf.Abs(Mathf.Abs(awakePosition.x) - Mathf.Abs(this.transform.position.x)) >= turnAroundDistance) && rb2d.velocity.y == 0);
        if (hasTurned) {
            ReverseMovement();
        }
    }

}
