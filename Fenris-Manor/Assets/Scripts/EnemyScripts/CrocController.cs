using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrocController : EnemyController
{
    public CrocData crocData;

    private Animator crocAnimator;

    private bool hasFired = false;
    private bool hasJumped = false;
    private bool resetFacing = true;

    protected override void Awake() {
        base.Awake();


        enemyData = crocData;
        crocAnimator = GetComponent<Animator>();
    }

    void Start() {
        SetMovement();
    }

    // Update is called once per frame
    protected override void Update() {
        base.Update();

        if (isAwake) {
            if (!hasFired && rb2d.velocity.y == 0) {
                StartCoroutine(ShootFireball());
            }
        } else {
            WakeEnemy(crocData.wakeDistance);
        }

        if (rb2d.velocity.y < -0.1f) {
            actorFeetCollider.enabled = true;
        }

        if (resetFacing) {
            StartCoroutine(ResetFacing());
        }
    }

    protected override void ComputeVelocity() {
        if (isAwake) {
            if (!hasJumped) {
                actorFeetCollider.enabled = false;
                rb2d.velocity = Vector2.up * crocData.jumpStrength;
                hasJumped = true;
            }
        }
    }

    IEnumerator ShootFireball() {
        hasFired = true;
        crocAnimator.SetBool("attacking", true);
        var fireball = Instantiate(crocData.fireball, new Vector2(transform.position.x, transform.position.y + 0.25f), transform.rotation);
        fireball.GetComponent<ProjectileController>().MoveProjectileLinearly(fireball, this);

        yield return new WaitForSecondsRealtime(0.5f);

        crocAnimator.SetBool("attacking", false);

        yield return new WaitForSecondsRealtime(Random.Range(0.5f, 1.5f));

        hasFired = false;
    }

    IEnumerator ResetFacing() {
        resetFacing = false;
        SetMovement();

        yield return new WaitForSecondsRealtime(3f);

        resetFacing = true;
    }

}
