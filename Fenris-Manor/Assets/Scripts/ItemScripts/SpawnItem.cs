using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SpawnItem : MonoBehaviour
{
    protected SpriteRenderer spriteRenderer;

    public virtual void ItemSpawn() {}
}
