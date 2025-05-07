using System;
using System.Collections;
using UnityEngine;
using Random = System.Random;

public enum EnemyState
{
    Idle,
    Patrol,
    Chase,
    Attack,
    Hurt,
    Dead
}

public class BasicSamurai : BaseEnemy, IDDamagable
{
    // private
    private EnemyState m_currentState;
    private GameObject m_currentTarget;
    private SpriteRenderer m_spriteRenderer;
    private Rigidbody2D m_rb;
    private Animator m_animator;
    private Transform m_preAttackPoint;
    private float m_lastAttackTime = -999f;
    
    // public
    [Header("Movement")]
    public GameObject playerTarget;
    public Transform attackPoint;
    public GameObject[] pointList;

    [Header("Config")] 
    public EnemyConfig enemyConfig;
    
    
    private void Awake()
    {
        m_currentState = EnemyState.Patrol;
        m_currentTarget = pointList[0];
        m_spriteRenderer = GetComponent<SpriteRenderer>();
        m_animator = GetComponent<Animator>();
        m_rb = GetComponent<Rigidbody2D>();
        m_preAttackPoint = attackPoint;
    }

    private void Update()
    {
        HandleStates();
    }

    private void HandleStates()
    {
        switch (m_currentState)
        {
            case EnemyState.Idle:
                HandleIdle();
                break;
            case EnemyState.Patrol:
                Move();
                break;
            case EnemyState.Chase:
                // HandleIdle()
                break;
            case EnemyState.Attack:
                // HandleIdle()
                break;
            case EnemyState.Hurt:
                // HandleIdle()
                break;
            case EnemyState.Dead:
                // HandleIdle()
                break;
            default:
                break;
        }
    }

    public override void Die()
    {
        
    }

    private void HandleIdle()
    {
        m_currentState = EnemyState.Idle;
        m_rb.linearVelocity = Vector2.Lerp(m_rb.linearVelocity, Vector2.zero, Time.deltaTime * 5f);

        if (m_rb.linearVelocity.magnitude < 0.01f)
        {
            m_rb.linearVelocity = Vector2.zero;
        }
        
        m_animator.SetFloat("Speed", 0);
    }

    public override void Move()
    {
        m_currentState = EnemyState.Patrol;
        
        float dir = Mathf.Sign(m_currentTarget.transform.position.x - transform.position.x);
        
        // Flip with direction
        m_spriteRenderer.flipX = dir < 0;
        
        // Move Speed
        m_rb.linearVelocity = new Vector2(dir * enemyConfig.patrolSpeed, m_rb.linearVelocityY);
        
        // Handle Move Animation
        m_animator.SetFloat("Speed", 1);
        
        // Flip attack point
        FlipAttackPoint(m_spriteRenderer.flipX);
        
        // Stop within distance
        bool isInDistance = (Mathf.Abs(transform.position.x - m_currentTarget.transform.position.x) <=
                            enemyConfig.attackRange);

        Collider2D hit = Physics2D.OverlapCircle(
            transform.position, 
            enemyConfig.detectRadius, 
            enemyConfig.playerLayer);

        float distanceToPlayer = Mathf.Abs(transform.position.x - playerTarget.transform.position.x);
        
        if ( distanceToPlayer <= enemyConfig.detectRadius && hit != null)
        {
            Attack();
        }
        else if ( m_currentState == EnemyState.Patrol && Mathf.Abs(transform.position.x - m_currentTarget.transform.position.x) < 0.5f)
        {
            m_currentState = EnemyState.Idle;
            StartCoroutine(PatrolPause());
        }
    }

    IEnumerator PatrolPause()
    {
        HandleIdle();
        yield return new WaitForSeconds(enemyConfig.waitTime);
        SwitchPatrolPoint();
        m_currentState = EnemyState.Patrol;
    }
    
    private void SwitchPatrolPoint()
    {
        if (pointList.Length == 1) { return; }
        int randIndex;
        do
        {
            randIndex = UnityEngine.Random.Range(0, pointList.Length);
        } while (pointList[randIndex] == m_currentTarget);
        
        m_currentTarget = pointList[randIndex];
    }

    private void FlipAttackPoint(bool facingLeft)
    {
        Vector3 preAtk = m_preAttackPoint.position;
        attackPoint.localPosition = new Vector3(
            facingLeft ? -Mathf.Abs(preAtk.x) : Mathf.Abs(preAtk.x),
            preAtk.y,
            preAtk.z);
    }

    public override void Attack()
    {
        m_currentState = EnemyState.Attack;
        m_currentTarget = playerTarget;
        m_rb.linearVelocity = Vector2.zero;
        float dir = Mathf.Sign(m_currentTarget.transform.position.x - transform.position.x);
        m_spriteRenderer.flipX = dir < 0;
        
        if (Time.time - m_lastAttackTime > enemyConfig.attackCooldown)
        {
            m_animator.SetTrigger("Attack");
            m_lastAttackTime = Time.time;
            StartCoroutine(AttackPause());
        }
    }

    IEnumerator AttackPause()
    {
        yield return new WaitForSeconds(enemyConfig.attackCooldown);
    }

    private void HandleChase()
    {
        
    }

    public override void UseAbility()
    {
        
    }

    public override void TakeDamage(int damage)
    {
        
    }
}
