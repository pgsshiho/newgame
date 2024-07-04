using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float maxSpeed = 10.0f;
    public float jumpPower = 5.0f;
    public Vector2 inputVec;
    private Rigidbody2D rigid;
    private SpriteRenderer sr;
    private Animator animator;
    public bool isJump = false;

    public void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        rigid.freezeRotation = true;
    }

    public void Update()
    {
        inputVec.x = Input.GetAxisRaw("Horizontal");

        if (inputVec.x != 0)
        {
            animator.SetBool("Right_Move", true);
            sr.flipX = inputVec.x < 0;
        }
        else
        {
            animator.SetBool("Right_Move", false);
        }

        if (Input.GetButtonDown("Jump") && !isJump)
        {
            animator.SetBool("jumping", true);
            isJump = true;
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            Debug.Log("Á¡ÇÁ Áß");
        }
    }

    public void FixedUpdate()
    {
        CheckGround();
        Vector2 nextVec = inputVec.normalized * maxSpeed;
        rigid.velocity = new Vector2(nextVec.x, rigid.velocity.y);
    }

    private void CheckGround()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.1f, LayerMask.GetMask("Ground"));

        if (hit.collider != null)
        {
            isJump = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.contacts[0].normal.y > 0.5f)
        {
            isJump = false;
            animator.SetBool("jumping", false);
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, Vector3.down); 
    }
}
