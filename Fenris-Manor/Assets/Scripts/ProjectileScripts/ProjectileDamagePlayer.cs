using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileDamagePlayer : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player") {
            this.GetComponentInParent<ProjectileController>().DealDamage();
            Destroy(this.transform.parent.gameObject);
        }
    }
}
