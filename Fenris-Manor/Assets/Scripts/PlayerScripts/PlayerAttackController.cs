using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackController : PlayerController
{
    private PlayerController playerController;
    private Animator animator;

    private bool canAttack;


    // Start is called before the first frame update
    void Start()
    {
        playerController = player.GetComponent<PlayerController>();
        animator = player.GetComponent<Animator>();
        animator.SetInteger("whipLevel", 1);
        canAttack = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Attack") && !playerController.GetIsAttacking() && canAttack) {
            StartCoroutine(PlayerAttack());
            StartCoroutine(ResetAttack());
        }
    }

    IEnumerator PlayerAttack() {
        playerController.SetIsAttacking(true);
        animator.SetBool("attack", true);
        
        yield return new WaitForSecondsRealtime(0.25f);
        
        playerController.SetIsAttacking(false);
        animator.SetBool("attack", false);
    }

    IEnumerator ResetAttack() {
        canAttack = false;

        yield return new WaitForSecondsRealtime(0.4f);

        canAttack = true;
    }
}
