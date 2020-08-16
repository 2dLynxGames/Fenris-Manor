using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoints : SpawnItem
{
    protected int points;
    public PointDropData pointData;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = pointData.sprite;
        points = pointData.points;
    }
}
