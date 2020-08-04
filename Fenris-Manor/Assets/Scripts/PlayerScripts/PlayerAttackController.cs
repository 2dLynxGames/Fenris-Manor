using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackController : MonoBehaviour
{
    private PlayerController playerController;
    private Animator animator;

    private bool canAttack;


    // Start is called before the first frame update
    void Start()
    {
        playerController = GetComponent<PlayerController>();
        animator = GetComponent<Animator>();
        animator.SetInteger("whipLevel", 1);
        canAttack = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Attack") && !playerController.GetIsAttacking() && !playerController.GetInDoor() && canAttack) {
            StartCoroutine(PlayerAttack());
            StartCoroutine(ResetAttack());
        }
    }

    Collider2D CreateWhipHitbox() {

        var playerWhipHitbox = Instantiate(playerController.GetWhipHitbox(), transform.position, Quaternion.identity);
        if (playerController.GetFacing() == PlayerController.FACING.right) {
            if (playerController.GetIsCrouching()) {
                playerWhipHitbox.transform.position -= new Vector3(0, 0.4f, 0);
            }
        } else {
            playerWhipHitbox.transform.localScale = new Vector3(-1, 1, 1);
            if (playerController.GetIsCrouching()) {
                playerWhipHitbox.transform.position -= new Vector3(0, 0.4f, 0);
            }
        }

        return playerWhipHitbox;

    }

    IEnumerator PlayerAttack() {
        playerController.SetIsAttacking(true);
        Collider2D playerWhipHitbox;
        animator.SetBool("attack", true);
        
        yield return new WaitForSecondsRealtime(0.125f);
        if (!playerController.GetIsHurt()) {
            playerController.whipSound.Play();
            playerWhipHitbox = CreateWhipHitbox();

            
            yield return new WaitForSecondsRealtime(0.08f);

            Destroy(playerWhipHitbox.gameObject);
        }
        
        playerController.SetIsAttacking(false);

        animator.SetBool("attack", false);
    }

    IEnumerator ResetAttack() {
        canAttack = false;

        yield return new WaitForSecondsRealtime(0.4f);

        canAttack = true;
    }
}
