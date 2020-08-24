using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenuAttribute(fileName = "ProjectileData", menuName = "Scriptables/Projectile/Generic", order=-1100)]
public class ProjectileData : ScriptableObject
{
    public int damage;
    public float projectileSpeed;
}

