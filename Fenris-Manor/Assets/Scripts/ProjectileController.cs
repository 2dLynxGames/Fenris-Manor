using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : EnemyController
{
    public ProjectileData projectileData;

    protected override void Awake() {
        base.Awake();

        enemyData = projectileData;
    }
}
