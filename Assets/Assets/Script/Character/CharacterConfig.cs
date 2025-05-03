using UnityEngine;

[CreateAssetMenu(fileName = "PlayerConfigData", menuName = "PlayerConfig", order = 1)]
public class CharacterConfig : ScriptableObject
{
    public float playerMoveSpeed = 1.0f;
    public float jumpForce = 5.0f;
    public float groundCheckRadius = 0.15f;
    public float gravityForce = 40f;
    public float terminalVelocity = 20f;
    public float attackAnimDuration = 0.5f;
    public LayerMask groundLayer;
}
