using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CultistController : EnemyController
{
    private bool isAwake = false;

    private Animator cultistAnimator;
    
    protected override void Awake()
    {
        base.Awake();
        cultistAnimator = GetComponent<Animator>();
    }

    void Start()
    {
        SetMovement();
    }
    
    protected override void ComputeVelocity(){
        if (isAwake) {
            targetVelocity = move * moveSpeed;
        }
    }

    protected override void AnimateActor(){
        if (isAwake) {
            cultistAnimator.SetBool("isAwake", true);
        }
    }

    void OnEnable()
	{
        isAwake = false;
        cultistAnimator.SetBool("isAwake", false);
	}

    void OnBecameVisible()
    {
        isAwake = true;
        cultistAnimator.SetBool("isAwake", true);
    }
}
