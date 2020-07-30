using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface iEnemyController
{
    void TakeDamage(int damageToTake);
    void DealDamage(int damageToDo);
    bool GetIsDead();
    void SetIsDead(bool dead);
}
