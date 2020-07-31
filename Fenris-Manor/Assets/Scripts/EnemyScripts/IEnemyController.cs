using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IEnemyController
{
    void TakeDamage(int damageToTake);
    void DealDamage();
    bool GetIsDead();
    void SetIsDead(bool dead);
}
