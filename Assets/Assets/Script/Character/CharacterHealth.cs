using System;
using UnityEngine;

public class CharacterHealth : MonoBehaviour, IDDamagable
{
    public CharacterConfig _characterConfig;
    private int currentHealth;

    private void Awake()
    {
        currentHealth = _characterConfig.playerMaxHealth;
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        Debug.Log("Took " + amount);
        
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Player died.");
        // Add respawn, animation, or game over logic
    }
}
