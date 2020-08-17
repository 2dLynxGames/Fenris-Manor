using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyBreakable : MonoBehaviour
{
    public GameObject itemToSpawn;
    private bool isBroken;

    void Start()
    {

    }

    public void DestroyObject() {
        if (!isBroken) {
            isBroken = true;
            ItemSpawn(itemToSpawn);
            Destroy(this.gameObject);
        }
    }

    public void ItemSpawn(GameObject itemToSpawn) {
        Debug.Log("Spawning item");
        var drop = Instantiate(itemToSpawn, transform.position, transform.rotation);

        Debug.Log(drop.name + " spawned");
    }
}
