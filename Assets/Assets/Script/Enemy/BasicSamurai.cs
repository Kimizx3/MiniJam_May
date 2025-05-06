using System;
using System.Collections;
using UnityEngine;

public class BasicSamurai : BaseEnemy, IDDamagable
{
    public EnemyConfig enemyConfig;
    public Transform pointA;
    public Transform pointB;
    public GameObject playerTarget;
    public Transform attackPoint;
    public float attackRadius = 0.5f;
    public LayerMask playerLayer;
    
    private int m_currentHealth;
    private Transform target;
    private Rigidbody2D m_rb;
    private SpriteRenderer _spriteRenderer;
    private Animator m_enemyAnimator;
    private bool isWaiting = false;
    private float waitCounter = 0f;
    private bool isAttacking = false;
    private bool isChasing = false;
    private float lastAttackTime = -999f;
    private Vector3 preAttackPoint;
    private void Awake()
    {
        m_currentHealth = enemyConfig.maxHealth;
        m_rb = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        m_enemyAnimator = GetComponent<Animator>();
        target = pointB;
        preAttackPoint = attackPoint.localPosition;
    }

    private void Update()
    {
        Move();
        Attack();
    }

    public override void Die()
    {
        Debug.Log("Enemy die.");
        //Destroy(gameObject);
    }

    public override void Move()
    {
        if ( isWaiting || isAttacking) { return; }
        
        Vector2 dir = (target.position - transform.position).normalized;

        if (target == playerTarget.transform)
        {
            float distanceToPlayer = Mathf.Abs(transform.position.x - playerTarget.transform.position.x);
            if (distanceToPlayer <= enemyConfig.attackRange * 0.95f)
            {
                m_rb.linearVelocity = new Vector2(0, m_rb.linearVelocityY);
                m_enemyAnimator.SetFloat("Speed", 0);
                StartCoroutine(WaitForAttack());
                return;
            }
        }
        
        m_rb.linearVelocity = new Vector2(dir.x * enemyConfig.patrolSpeed, m_rb.linearVelocityY);
        
        m_enemyAnimator.SetFloat("Speed", Mathf.Abs(m_rb.linearVelocity.x));
        _spriteRenderer.flipX = dir.x < 0;
        FlipAttackPoint(_spriteRenderer.flipX);

        if ((dir.x > 0 && transform.position.x >= target.position.x - 0.1f) ||
            (dir.x < 0 && transform.position.x <= target.position.x + 0.1f))
        {
            StartCoroutine(PatrolPause());
        }
    }

    private void SwitchPatrolPoint()
    {
        target = target == pointA ? pointB : pointA;
    }

    IEnumerator PatrolPause()
    {
        isWaiting = true;
        m_rb.linearVelocity = Vector2.zero;
        m_enemyAnimator.SetFloat("Speed", 0);

        yield return new WaitForSeconds(enemyConfig.waitTime);
        
        SwitchPatrolPoint();
        isWaiting = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        IDDamagable damagable = collision.gameObject.GetComponent<IDDamagable>();
        if (damagable != null)
        {
            damagable.TakeDamage(enemyConfig.damage);
        }
    }

    public override void UseAbility()
    {
        
    }

    public override void TakeDamage(int damage)
    {
        m_currentHealth -= damage;
        Debug.Log("Enemy Took " + damage);
        
        if (m_currentHealth <= 0)
        {
            Die();
        }
    }

    public override void Attack()
    {
        float distanceToPlayer = Mathf.Abs(transform.position.x - playerTarget.transform.position.x);
        if (distanceToPlayer <= enemyConfig.attackRange)
        {
            isChasing = true;
            target = playerTarget.transform;
            if (Time.time - lastAttackTime > enemyConfig.attackCooldown)
            {
                m_enemyAnimator.SetTrigger("Attack");
                lastAttackTime = Time.time;
                StartCoroutine(AttackPause());
            }
        }
        else
        {
            if (isChasing)
            {
                target = Mathf.Abs(transform.position.x - pointA.position.x) <
                         Mathf.Abs(transform.position.x - pointB.position.x)
                    ? pointA
                    : pointB;
                
                isChasing = false;
            }
            
        }
    }

    public void DealDamage()
    {
        Vector2 toPlayer = (playerTarget.transform.position - transform.position).normalized;
        float facingDir = _spriteRenderer.flipX ? -1 : 1;

        if (Mathf.Sign(toPlayer.x) != facingDir)
        {
            return;
        }

        Collider2D hit = Physics2D.OverlapCircle(attackPoint.position, attackRadius, playerLayer);
        
        float distanceToPlayer = Mathf.Abs(transform.position.x - playerTarget.transform.position.x);
        if (distanceToPlayer <= enemyConfig.attackRange && hit != null)
        {
            IDDamagable player = playerTarget.GetComponent<IDDamagable>();
            if (player != null)
            {
                player.TakeDamage(enemyConfig.damage);
            }
        }
    }
    
    IEnumerator AttackPause()
    {
        isAttacking = true;
        isWaiting = true;
        m_rb.linearVelocity = Vector2.zero;
        m_enemyAnimator.SetFloat("Speed", 0);

        yield return new WaitForSeconds(enemyConfig.attackCooldown);

        isAttacking = false;
        isWaiting = false;
    }

    IEnumerator WaitForAttack()
    {
        yield return new WaitForSeconds(2);
    }

    public void OnAttackEnd()
    {
        isAttacking = false;
    }

    private void FlipAttackPoint(bool facingLeft)
    {
        attackPoint.localPosition = new Vector3(
            facingLeft ? -Mathf.Abs(preAttackPoint.x) : Mathf.Abs(preAttackPoint.x),
            preAttackPoint.y,
            preAttackPoint.z);
    }
    
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
    }
}
