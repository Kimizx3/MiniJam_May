using System;
using System.Collections;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    // Public Section
    public CharacterConfig characterConfig;
    public Transform feetPos;
    public Transform hitPoint;
    
    // Private Section
    private Rigidbody2D m_rb;
    private CapsuleCollider2D m_body;
    private Animator m_animator;
    private SpriteRenderer m_spriteRenderer;
    private bool m_jumpRequested = false;
    private bool m_isGrounded = true;
    private float m_verticalVelocity = 0f;

    private bool m_isDashing = false;
    private bool m_canDash = true;
    private float m_dashTimer = 0f;

    private Vector3 preHitPoint;

    private CharacterAttack _characterAttack;

    private void Awake()
    {
        m_rb = GetComponent<Rigidbody2D>();
        m_body = GetComponent<CapsuleCollider2D>();
        m_animator = GetComponent<Animator>();
        m_spriteRenderer = GetComponent<SpriteRenderer>();
        _characterAttack = GetComponent<CharacterAttack>();
        preHitPoint = hitPoint.localPosition;
        if (feetPos == null)
        {
            feetPos = transform.Find("Feet");
        }
    }

    private void Update()
    {
        // Update() function for input update (not relevant with physics)
        JumpInput();
        Dash();
    }

    private void FixedUpdate()
    {
        // FixedUpdate() function for physics related functions 
        SimulateGravity();
        HorizontalMove();
        FlipCharacter();
        CheckGround();
        Jump();
    }

    float MovementInput()
    {
        float input = 0;
        if (Input.GetKey(KeyCode.D)) input += 1f;
        if (Input.GetKey(KeyCode.A)) input -= 1f;
        return input;
    }
    
    void HorizontalMove()
    {
        Vector2 move = new Vector2(MovementInput() * characterConfig.playerMoveSpeed, m_verticalVelocity);
        m_rb.linearVelocity = move;
        PlayRunAnimation();
    }

    void JumpInput()
    {
        if (m_isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            m_jumpRequested = true;
            m_animator.SetBool("Jump", true);
        }
    }
    
    void Jump()
    {
        if (m_jumpRequested)
        {
            m_verticalVelocity = characterConfig.jumpForce;
            m_jumpRequested = false;
        }
    }
    
    public void CheckGround()
    {
        bool m_wasGrounded = m_isGrounded;
        
        m_isGrounded = Physics2D.OverlapCircle(
            feetPos.position,
            characterConfig.groundCheckRadius,
            characterConfig.groundLayer);

        if (!m_wasGrounded && m_isGrounded)
        {
            m_animator.SetBool("Jump", false);
        }

    }

    void SimulateGravity()
    {
        if (m_isDashing) return;
        if (!m_isGrounded)
        {
            m_verticalVelocity -= characterConfig.gravityForce * Time.fixedDeltaTime;
            m_verticalVelocity = Mathf.Max(m_verticalVelocity, -characterConfig.terminalVelocity);
        }
        else if (m_verticalVelocity < 0f)
        {
            m_verticalVelocity = 0f;
        }
    }

    void FlipCharacter()
    {
        if (MovementInput() > 0)
            m_spriteRenderer.flipX = false;
        else if (MovementInput() < 0)
            m_spriteRenderer.flipX = true;
        FlipAttackPoint(m_spriteRenderer.flipX);
    }

    public void PlayRunAnimation()
    {
        m_animator.SetFloat("Speed", Mathf.Abs(MovementInput()));
    }

    public void Dash()
    {
        if (Input.GetKeyDown(KeyCode.Q) && m_canDash)
        {
            StartCoroutine(PerformDash());
        }
        if (m_isDashing && Input.GetKeyDown(KeyCode.Space))
        {
            CancelDashToJump();
        }
        if (m_isDashing && Input.GetKeyDown(KeyCode.E))
        {
            CancelDashToAttack();
        }
    }

    IEnumerator PerformDash()
    {
        m_isDashing = true;
        m_canDash = false;

        float originalGravity = m_rb.gravityScale;
        m_rb.gravityScale = 0f;

        Vector2 dashDir = m_spriteRenderer.flipX ? Vector2.left : Vector2.right;
        Vector2 origin = (Vector2)transform.position;

        float dashLength = characterConfig.dashDistance;

        RaycastHit2D hit = Physics2D.Raycast(origin, dashDir, dashLength,
            characterConfig.dashObstacleMask);
        if (hit.collider != null)
        {
            dashLength = hit.distance - 0.1f;
        }
        Vector2 target = origin + dashDir * dashLength;

        transform.position = target;
        
        m_animator.SetTrigger("Dash");

        yield return new WaitForSeconds(characterConfig.dashDuration);

        m_isDashing = false;
        m_rb.gravityScale = originalGravity;
        m_rb.linearVelocity = Vector2.zero;

        yield return new WaitForSeconds(characterConfig.dashCooldown);
        m_canDash = true;
    }

    void CancelDashToJump()
    {
        m_isDashing = false;
        m_rb.gravityScale = characterConfig.gravityForce;
        m_verticalVelocity = characterConfig.jumpForce;
        m_animator.SetBool("Jump", true);
    }

    void CancelDashToAttack()
    {
        m_isDashing = false;
        m_rb.gravityScale = characterConfig.gravityForce;
        _characterAttack.PlayAttackAnim();
    }
    
    private void FlipAttackPoint(bool facingLeft)
    {
        hitPoint.localPosition = new Vector3(
            facingLeft ? -Mathf.Abs(preHitPoint.x) : Mathf.Abs(preHitPoint.x),
            preHitPoint.y,
            preHitPoint.z);
    }
}
