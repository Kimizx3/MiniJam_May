using UnityEngine;

[CreateAssetMenu(fileName = "PlayerConfigData", menuName = "PlayerConfig", order = 1)]
public class CharacterConfig : ScriptableObject
{
    [Header("Movement Setting")]
    public float playerMoveSpeed = 1.0f;
    public float jumpForce = 5.0f;
    public float groundCheckRadius = 0.15f;
    public float gravityForce = 40f;
    public float terminalVelocity = 20f;
    public LayerMask groundLayer;
    
    [Header("Combat Setting")]
    public float attackAnimDuration = 0.5f;
    public int playerMaxHealth = 100;
    public float attackRange = 0.5f;
    public int baseDamage = 5;
    public int heavyAttackDamage = 10;
    public LayerMask enemyLayer;
    
    [Header("Dash Setting")]
    public float dashSpeed = 40f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 0.5f;
    public float dashDistance = 5f;
    public LayerMask dashObstacleMask;
}
