using System;
using System.Collections;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    // Public Section
    public CharacterConfig characterConfig;
    public Transform feetPos;
    
    // Private Section
    private Rigidbody2D m_rb;
    private CapsuleCollider2D m_body;
    private Animator m_animator;
    private SpriteRenderer m_spriteRenderer;
    private bool m_jumpRequested = false;
    private bool m_isGrounded = true;
    private float m_verticalVelocity = 0f;

    private void Awake()
    {
        m_rb = GetComponent<Rigidbody2D>();
        m_body = GetComponent<CapsuleCollider2D>();
        m_animator = GetComponent<Animator>();
        m_spriteRenderer = GetComponent<SpriteRenderer>();
        if (feetPos == null)
        {
            feetPos = transform.Find("Feet");
        }
    }

    private void Update()
    {
        // Update() function for input update (not relevant with physics)
        JumpInput();
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
    }

    public void PlayRunAnimation()
    {
        m_animator.SetFloat("Speed", Mathf.Abs(MovementInput()));
    }
}
