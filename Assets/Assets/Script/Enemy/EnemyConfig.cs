using UnityEngine;
using UnityEngine.Serialization;

public enum EnemyType { BasicSamurai, DemonSamurai, Boss}

[CreateAssetMenu(fileName = "EnemyConfig", menuName = "Enemy/Config", order = 0)]
public class EnemyConfig : ScriptableObject
{
    [Header("Basic Setting")] 
    public string enemyName;
    public EnemyType type;
    public float waitTime = 1f;
    
    [Header("Movement Setting")] 
    public float patrolSpeed = 20f;

    [Header("Combat Setting")] 
    public int maxHealth = 100;
    public int damage = 5;
    public float attackRange = 1f;
    public float attackCooldown = 1.5f;
}
