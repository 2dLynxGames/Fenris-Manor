using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyEnemy : MonoBehaviour
{
    //public GameObject deathAnimation;

    private PlayerController playerController;

    private int damageToDo;


    private
    // Start is called before the first frame update
    void Awake()
    {
        playerController = transform.GetComponentInParent<PlayerController>();
    }

    void Start() {
        damageToDo = playerController.GetWhipDamage();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other.tag);
        if (other.tag == "Enemy") {
            other.GetComponent<iEnemyController>().TakeDamage(damageToDo);
        }
    }


}
