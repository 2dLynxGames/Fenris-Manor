using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWhipController : MonoBehaviour
{
    public LayerMask enemyMask;
    public LayerMask breakableMask;
    private LayerMask layerMask;

    private PlayerController playerController;
    private Vector2 point;
    private Vector2 size;
    private ContactFilter2D contactFilter;
    private List<Collider2D> objectsHit = new List<Collider2D>(16);


    void Awake() {
        playerController = GetComponent<PlayerController>();

        size  = new Vector2(playerController.GetWhipLength(), playerController.GetWhipHeight());

        layerMask = enemyMask | breakableMask;
        contactFilter.useTriggers = false;
        contactFilter.SetLayerMask(layerMask);
        contactFilter.useLayerMask = true;
    }

    void FixedUpdate() {

        point = SetPoint();

        Physics2D.OverlapBox(point, size, 0f, contactFilter, objectsHit);

        foreach (var objectHit in objectsHit) {
            if (objectHit.tag == "Enemy") {
                objectHit.GetComponentInParent<EnemyController>().TakeDamage(playerController.GetWhipDamage());
            } else if (objectHit.tag == "Breakable") {
                Debug.Log("I hit a candle");
                objectHit.GetComponent<DestroyBreakable>().DestroyObject();
            }
        }
    }

    Vector2 SetPoint() {
        Vector2 newPoint;
        if (playerController.GetFacing() == PlayerController.FACING.right) {
            newPoint.x = transform.position.x + playerController.GetBufferZone() + (playerController.GetWhipLength() / 2f);
        } else {
            newPoint.x = transform.position.x - playerController.GetBufferZone() - (playerController.GetWhipLength() / 2f);
        }
        
        if (playerController.GetIsCrouching()) {
            newPoint.y =  transform.position.y - playerController.GetCrouchReduction() + (playerController.GetWhipLength() / 2f);
        } else {
            newPoint.y = transform.position.y + (playerController.GetWhipLength() / 2f);
        }
        return newPoint;
    }

    void DebugStuff() {
        Color rayColor;

        if (objectsHit.Count != 0) {
            rayColor = Color.green;
            Debug.Log(objectsHit.Count);
            foreach (var enemy in objectsHit) {
                Debug.Log(enemy.name);
            }
        } else {
            rayColor = Color.red;
        }
        if (playerController.GetFacing() == PlayerController.FACING.right) {
            if (playerController.GetIsCrouching()) {
                Debug.DrawRay(new Vector2(transform.position.x + playerController.GetBufferZone(), transform.position.y - playerController.GetCrouchReduction()), Vector2.right * playerController.GetWhipLength(), rayColor);
                Debug.DrawRay(new Vector2(transform.position.x + playerController. GetBufferZone(), transform.position.y + playerController.GetWhipHeight() - playerController.GetCrouchReduction()), Vector2.right * playerController.GetWhipLength(), rayColor);
                Debug.DrawRay(new Vector2(transform.position.x + playerController.GetBufferZone() + playerController.GetWhipLength(), transform.position.y - playerController.GetCrouchReduction()), Vector2.up * playerController.GetWhipHeight(), rayColor);
            } else {
                Debug.DrawRay(new Vector2(transform.position.x + playerController.GetBufferZone(), transform.position.y), Vector2.right * playerController.GetWhipLength(), rayColor);
                Debug.DrawRay(new Vector2(transform.position.x + playerController. GetBufferZone(), transform.position.y + playerController.GetWhipHeight()), Vector2.right * playerController.GetWhipLength(), rayColor);
                Debug.DrawRay(new Vector2(transform.position.x + playerController.GetBufferZone() + playerController.GetWhipLength(), transform.position.y), Vector2.up * playerController.GetWhipHeight(), rayColor);
                }
        } else {
            if (playerController.GetIsCrouching()) {
                Debug.DrawRay(new Vector2(transform.position.x - playerController.GetBufferZone(), transform.position.y - playerController.GetCrouchReduction()), Vector2.left * playerController.GetWhipLength(), rayColor);
                Debug.DrawRay(new Vector2(transform.position.x - playerController. GetBufferZone(), transform.position.y + playerController.GetWhipHeight() - playerController.GetCrouchReduction()), Vector2.left * playerController.GetWhipLength(), rayColor);
                Debug.DrawRay(new Vector2(transform.position.x - playerController.GetBufferZone() - playerController.GetWhipLength(), transform.position.y - playerController.GetCrouchReduction()), Vector2.up * playerController.GetWhipHeight(), rayColor);
            } else {
                Debug.DrawRay(new Vector2(transform.position.x - playerController.GetBufferZone(), transform.position.y), Vector2.left * playerController.GetWhipLength(), rayColor);
                Debug.DrawRay(new Vector2(transform.position.x - playerController. GetBufferZone(), transform.position.y + playerController.GetWhipHeight()), Vector2.left * playerController.GetWhipLength(), rayColor);
                Debug.DrawRay(new Vector2(transform.position.x - playerController.GetBufferZone() - playerController.GetWhipLength(), transform.position.y), Vector2.up * playerController.GetWhipHeight(), rayColor);
            }
        }
    }
}
