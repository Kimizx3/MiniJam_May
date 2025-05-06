using System;
using UnityEngine;

public abstract class BaseEnemy : MonoBehaviour
{
    public abstract void Die();
    public abstract void Move();
    
    public abstract void Attack();
    public abstract void UseAbility();
    public abstract void TakeDamage(int damage);
}
