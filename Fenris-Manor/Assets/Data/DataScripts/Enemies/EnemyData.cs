using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenuAttribute()]
public class EnemyData : ScriptableObject
{
    public float moveSpeed;
    public GameObject death;
    public int health;
    public float gravityModifier = 1f;
    public int damageToDo;
}
