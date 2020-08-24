using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CultistController : EnemyController
{
    public CultistData cultistData;

    private Animator cultistAnimator;
    
    protected override void Awake()
    {
        base.Awake();
        cultistAnimator = GetComponent<Animator>();
        enemyData = cultistData;

        currentHealth = cultistData.health;
    }

    void Start()
    {
        SetMovement();
    }
    
    protected override void ComputeVelocity(){
        if (isAwake) {
            targetVelocity = move * cultistData.moveSpeed;
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
