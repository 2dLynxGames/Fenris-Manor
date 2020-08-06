using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWhipController : MonoBehaviour
{
    public LayerMask layerMask;

    private PlayerController playerController;
    private Vector2 point;
    private Vector2 size;
    private ContactFilter2D contactFilter;
    private List<Collider2D> enemiesHit = new List<Collider2D>(16);


    void Awake() {
        playerController = GetComponent<PlayerController>();

        size  = new Vector2(playerController.whipLength, playerController.whipHeight);

        contactFilter.useTriggers = false;
        contactFilter.SetLayerMask(layerMask);
        contactFilter.useLayerMask = true;
    }

    void FixedUpdate() {

        if (playerController.GetFacing() == PlayerController.FACING.right) {
            if (playerController.GetIsCrouching()) {
                point = new Vector2(transform.position.x + playerController.bufferZone + (playerController.whipLength / 2f), transform.position.y - playerController.crouchReduction + (playerController.whipLength / 2f));
            } else {
                point = new Vector2(transform.position.x + playerController.bufferZone + (playerController.whipLength / 2f), transform.position.y + (playerController.whipLength / 2f));
            }

        } else {
            if (playerController.GetIsCrouching()) {
                point = new Vector2(transform.position.x - playerController.bufferZone - (playerController.whipLength / 2f), transform.position.y - playerController.crouchReduction + (playerController.whipLength / 2f));
            } else {
                point =  new Vector2(transform.position.x - playerController.bufferZone - (playerController.whipLength / 2f), transform.position.y + (playerController.whipLength / 2f));
            }
        }

        Physics2D.OverlapBox(point, size, 0f, contactFilter, enemiesHit);

        foreach (var enemy in enemiesHit) {
            enemy.GetComponentInParent<EnemyController>().TakeDamage(playerController.GetWhipDamage());
        }
        Debug.Log(Time.deltaTime);
        DebugStuff();
    }

    void DebugStuff() {
        Color rayColor;

        if (enemiesHit.Count != 0) {
            rayColor = Color.green;
            Debug.Log(enemiesHit.Count);
            foreach (var enemy in enemiesHit) {
                Debug.Log(enemy.name);
            }
        } else {
            rayColor = Color.red;
        }
        if (playerController.GetFacing() == PlayerController.FACING.right) {
            if (playerController.GetIsCrouching()) {
                Debug.DrawRay(new Vector2(transform.position.x + playerController.bufferZone, transform.position.y - playerController.crouchReduction), Vector2.right * playerController.whipLength, rayColor);
                Debug.DrawRay(new Vector2(transform.position.x + playerController. bufferZone, transform.position.y + playerController.whipHeight - playerController.crouchReduction), Vector2.right * playerController.whipLength, rayColor);
                Debug.DrawRay(new Vector2(transform.position.x + playerController.bufferZone + playerController.whipLength, transform.position.y - playerController.crouchReduction), Vector2.up * playerController.whipHeight, rayColor);
            } else {
                Debug.DrawRay(new Vector2(transform.position.x + playerController.bufferZone, transform.position.y), Vector2.right * playerController.whipLength, rayColor);
                Debug.DrawRay(new Vector2(transform.position.x + playerController. bufferZone, transform.position.y + playerController.whipHeight), Vector2.right * playerController.whipLength, rayColor);
                Debug.DrawRay(new Vector2(transform.position.x + playerController.bufferZone + playerController.whipLength, transform.position.y), Vector2.up * playerController.whipHeight, rayColor);
                }
        } else {
            if (playerController.GetIsCrouching()) {
                Debug.DrawRay(new Vector2(transform.position.x - playerController.bufferZone, transform.position.y - playerController.crouchReduction), Vector2.left * playerController.whipLength, rayColor);
                Debug.DrawRay(new Vector2(transform.position.x - playerController. bufferZone, transform.position.y + playerController.whipHeight - playerController.crouchReduction), Vector2.left * playerController.whipLength, rayColor);
                Debug.DrawRay(new Vector2(transform.position.x - playerController.bufferZone - playerController.whipLength, transform.position.y - playerController.crouchReduction), Vector2.up * playerController.whipHeight, rayColor);
            } else {
                Debug.DrawRay(new Vector2(transform.position.x - playerController.bufferZone, transform.position.y), Vector2.left * playerController.whipLength, rayColor);
                Debug.DrawRay(new Vector2(transform.position.x - playerController. bufferZone, transform.position.y + playerController.whipHeight), Vector2.left * playerController.whipLength, rayColor);
                Debug.DrawRay(new Vector2(transform.position.x - playerController.bufferZone - playerController.whipLength, transform.position.y), Vector2.up * playerController.whipHeight, rayColor);
            }
        }
    }
}
