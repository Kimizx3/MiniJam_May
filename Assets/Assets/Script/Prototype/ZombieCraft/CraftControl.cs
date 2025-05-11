using System;
using UnityEngine;

public class CraftControl : MonoBehaviour
{
    public Transform movePos;
    public float moveSpeed = 5f;
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        movePos.parent = null;
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, movePos.position, moveSpeed * Time.deltaTime);
        if (Vector3.Distance(transform.position, movePos.position) <= 0.05f) 
        {
            if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) == 1f)
            {
                movePos.position += new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f);
            }
            else if (Mathf.Abs(Input.GetAxisRaw("Vertical")) == 1f)
            {
                movePos.position += new Vector3(0f, Input.GetAxisRaw("Vertical"), 0f);
            }
        }
        
    }
}
