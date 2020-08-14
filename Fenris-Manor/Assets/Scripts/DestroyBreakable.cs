using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyBreakable : MonoBehaviour
{
    private bool isBroken;

    public void DestroyObject() {
        if (!isBroken) {
            isBroken = true;
            Destroy(this.gameObject);
        }
    }
}
