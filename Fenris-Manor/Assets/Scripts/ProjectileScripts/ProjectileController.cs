using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    public ProjectileData projectileData;
    private LevelManager levelManager;

    private bool isDestroyed = false;

    void Awake()
    {
        levelManager = FindObjectOfType<LevelManager>();
    }

    public void MoveProjectileLinearly(GameObject projectile, EnemyController source) {
        SpriteRenderer projectileSprite = projectile.GetComponent<SpriteRenderer>();
        if (source.GetMoveDirection() == EnemyController.MOVE_DIRECTION.left) {
            projectile.GetComponent<Rigidbody2D>().AddForce(Vector2.left * projectileData.projectileSpeed);
                if (!projectileSprite.flipX) {
                    projectileSprite.flipX = true;
                }
        } else {
            projectile.GetComponent<Rigidbody2D>().AddForce(Vector2.right * projectileData.projectileSpeed);
                if (projectileSprite.flipX) {
                    projectileSprite.flipX = false;
                }
        }
    }

    public void DestroyProjectile() {
        if (!isDestroyed) {
            isDestroyed = true;
            Destroy(this.gameObject);
        }
    }

    public void DealDamage() {
        levelManager.playerController.TakeDamage(projectileData.damage);
    }

}
