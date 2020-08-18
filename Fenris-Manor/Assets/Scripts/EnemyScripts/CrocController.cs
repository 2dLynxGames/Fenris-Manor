using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrocController : EnemyController
{
    public CrocData crocData;

    public bool hasFired = false;

    protected override void Awake() {
        base.Awake();

        enemyData = crocData;
    }

    void Start() {
        SetMovement();
    }

    // Update is called once per frame
    void Update()
    {
        if (isAwake) {
            if (isGrounded && !hasFired) {
                StartCoroutine(ShootFireball());
            }
        } else {
            WakeEnemy(crocData.wakeDistance);
        }
    }

    IEnumerator ShootFireball() {
        hasFired = true;
        var fireball = Instantiate(crocData.fireball);
        MoveProjectileLinearly(fireball);

        yield return new WaitForSecondsRealtime(Random.Range(1f, 2f));

        hasFired = true;
    }

    void MoveProjectileLinearly(GameObject projectile) {
        if (moveDirection == MOVE_DIRECTION.left) {
            rb2d.AddForce(Vector2.left * crocData.projectileSpeed);
        } else {
            rb2d.AddForce(Vector2.right * crocData.projectileSpeed);
        }
    }

}
