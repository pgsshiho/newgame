using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public float maxSpeed = 10.0f;
    public float jumpPower = 5.0f;
    public Vector2 inputVec;
    private Rigidbody2D rigid;
    private SpriteRenderer sr;
    private Animator animator;
    public bool isJump = false;
    public Slider HpBarSlider;
    public float curHealth = 100; //* 현재 체력
    public float maxHealth = 100; //* 최대 체력
    private bool isInvincible = false; // 무적 상태
    public float invincibilityDuration = 1.0f; // 무적 지속 시간
    public float flashDuration = 0.1f; // 깜빡임 간격
    public float AttackDamage = 10;
    public float attackRange = 5.0f; // 공격 범위
    EnemySlime ES;

    public void SetHp(float amount) //*Hp설정
    {
        maxHealth = amount;
        curHealth = maxHealth;
        CheckHp(); // 초기 HP 설정 시 체력바 갱신
    }

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

        if (Input.GetKeyDown(KeyCode.Space) && !isJump)
        {
            Jump();
        }
        if (Input.GetMouseButtonDown(0))
        {
            animator.SetTrigger("Attack");
            DetectAndAttackEnemy();
        }
        if (inputVec.x != 0)
        {
            animator.SetBool("Right_Move", true);
            sr.flipX = inputVec.x < 0;
        }
        else
        {
            animator.SetBool("Right_Move", false);
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

    public void CheckHp() //*HP 갱신
    {
        if (HpBarSlider != null)
            HpBarSlider.value = curHealth / maxHealth;
    }

    public void Damage(float damage)
    {
        if (isInvincible || maxHealth == 0 || curHealth <= 0)
            return;

        curHealth -= damage;
        CheckHp(); // 데미지를 입을 때 체력바 갱신

        if (curHealth <= 0)
        {
            //death
        }
        else
        {
            StartCoroutine(InvincibilityCoroutine());
        }
    }

    private IEnumerator InvincibilityCoroutine()
    {
        isInvincible = true;
        for (float i = 0; i < invincibilityDuration; i += flashDuration)
        {
            sr.enabled = !sr.enabled;
            yield return new WaitForSeconds(flashDuration);
        }
        sr.enabled = true;
        isInvincible = false;
    }

    public void Attack(Transform target)
    {
        // 공격 로직 추가 (타겟의 체력 감소 등)
        EnemySlime enemy = target.GetComponent<EnemySlime>();
        if (enemy != null)
        {
            enemy.TakeDamage(AttackDamage);
        }
    }

    public void DetectAndAttackEnemy()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, attackRange, LayerMask.GetMask("Enemy"));

        foreach (Collider2D enemy in hitEnemies)
        {
            if (Mathf.Abs(enemy.transform.position.x - transform.position.x) <= 3.0f)
            {
                Attack(enemy.transform);
                break;
            }
        }
    }

    public void Jump()
    {
        animator.SetBool("jumping", true);
        isJump = true;
        rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
        Debug.Log("점프 중");
        animator.SetBool("Right_Move", false);
    }

    // 애니메이션 이벤트를 통해 호출되는 메서드
    public void EndAttack()
    {
        animator.ResetTrigger("Attack");
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
