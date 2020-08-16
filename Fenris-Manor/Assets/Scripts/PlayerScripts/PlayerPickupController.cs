using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPickupController : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other){
        if (other.tag == "Drop") {
            Destroy(other.gameObject);
        }
    }
}
