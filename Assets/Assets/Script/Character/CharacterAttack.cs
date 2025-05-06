using System;
using System.Collections;
using UnityEngine;

public class CharacterAttack : MonoBehaviour
{
    private bool m_isAttacking = false;
    private Animator m_animator;
    private int comboIndex = 0;
    private float lastAttackTime = 0f;
    private bool canReceiveCombo = false;

    [Header("Combo Timing")]
    public float comboResetTime = 1f;
    
    public CharacterConfig characterConfig;
    public Transform attackPoint;

    private void Awake()
    {
        m_animator = GetComponent<Animator>();
    }

    private void Update()
    {
        PlayAttackAnim();
    }

    public void PlayAttackAnim()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            HandleComboInput();
        }
        else if (Time.time - lastAttackTime > comboResetTime)
        {
            ResetCombo();
        }
    }

    void HandleComboInput()
    {
        if (!canReceiveCombo && comboIndex > 0) return;

        comboIndex++;
        comboIndex = Mathf.Clamp(comboIndex, 1, 4);

        lastAttackTime = Time.time;
        
        m_animator.SetInteger("ComboIndex", comboIndex);
        m_animator.SetTrigger("Attack");
        canReceiveCombo = false;
    }

    void ApplyDamage()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(
            attackPoint.position,
            characterConfig.attackRange,
            characterConfig.enemyLayer);

        foreach (Collider2D enemy in hitEnemies)
        {
            IDDamagable target = enemy.GetComponent<IDDamagable>();
            if (target != null && comboIndex != 4)
            {
                target.TakeDamage(characterConfig.baseDamage);
            }
            else if (target != null && comboIndex == 4)
            {
                target.TakeDamage(characterConfig.heavyAttackDamage);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, characterConfig.attackRange);
    }

    public void EnableComboWindow()
    {
        canReceiveCombo = true;
    }

    public void EndCombo()
    {
        ResetCombo();
    }

    private void ResetCombo()
    {
        comboIndex = 0;
        m_animator.SetInteger("ComboIndex", 0);
        canReceiveCombo = false;
    }
}
