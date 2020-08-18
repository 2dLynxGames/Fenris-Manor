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

    protected override void AnimateActor() {
        
    }

    IEnumerator ShootFireball() {
        hasFired = true;
        var fireball = Instantiate(crocData.fireball, new Vector2(transform.position.x, transform.position.y + 0.25f), transform.rotation);
        MoveProjectileLinearly(fireball);

        yield return new WaitForSecondsRealtime(Random.Range(1f, 2f));

        hasFired = false;
    }

    void MoveProjectileLinearly(GameObject projectile) {
        SpriteRenderer projectileSprite = projectile.GetComponent<SpriteRenderer>();
        if (moveDirection == MOVE_DIRECTION.left) {
            projectile.GetComponent<Rigidbody2D>().AddForce(Vector2.left * crocData.projectileSpeed);
                if (!projectileSprite.flipX) {
                    projectileSprite.flipX = true;
                }
        } else {
            projectile.GetComponent<Rigidbody2D>().AddForce(Vector2.right * crocData.projectileSpeed);
                if (projectileSprite.flipX) {
                    projectileSprite.flipX = false;
                }
        }
    }

    IEnumerator ResetFacing() {
        resetFacing = false;
        SetMovement();

        yield return new WaitForSecondsRealtime(3f);

        resetFacing = true;
    }

}
