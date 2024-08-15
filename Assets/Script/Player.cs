using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float maxSpeed = 10.0f;
    public float jumpPower = 5.0f;
    public Vector2 inputVec;
    private Rigidbody2D rigid;
    private SpriteRenderer sr;
    private Animator animator;
    public bool isJump = false;
    public Slider HpBarSlider;
    [SerializeField]
    public float curHealth = 100; // 현재 체력
    public float maxHealth = 100; // 최대 체력
    private bool isInvincible = false; // 무적 상태
    public float invincibilityDuration = 1.0f; // 무적 지속 시간
    public float flashDuration = 0.1f; // 깜빡임 간격
    [SerializeField]
    public float AttackDamage = 5;
    public float attackRange = 10.0f; // 공격 범위
    [SerializeField]
    private int test = 0;
    StageManager stageManager; // StageManager 인스턴스
    Shop shop;

    public float sliderLerpSpeed = 2.0f; // 슬라이더 변화 속도

    private bool canAttack = true; // 공격 가능 여부
    public float attackCooldown = 1.0f; // 공격 쿨타임 시간
    public bool canMove = true; // 움직임 가능 여부

    private void Awake()
    {
        Time.timeScale = 1;
        rigid = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        rigid.freezeRotation = true;

        // StageManager 인스턴스를 가져옴
        stageManager = FindObjectOfType<StageManager>();

        // Shop 인스턴스를 가져옴
        shop = FindObjectOfType<Shop>();

        // 플레이어 초기화
        InitializePlayer();
    }

    // 플레이어 초기화 메서드
    private void InitializePlayer()
    {
        maxHealth = 100;
        curHealth = maxHealth;
        AttackDamage = 5;
        CheckHp(); // HP 슬라이더 업데이트
    }

    public void SetHp(float amount) // Hp 설정
    {
        maxHealth = amount;
        curHealth = maxHealth;
        CheckHp();
    }

    public void Update()
    {
        // Shop의 static 변수에 클래스 이름을 통해 접근
        AttackDamage = Shop.swordCount * 5;
        maxHealth = Shop.clothCount * 10;

        if (!canMove) return; // 움직임이 불가능하면 입력 무시

        inputVec.x = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space) && !isJump)
        {
            Jump();
        }
        if (Input.GetMouseButtonDown(0) && canAttack)
        {
            animator.SetTrigger("Attack");

            DetectAndAttackEnemy();
            StartCoroutine(AttackCooldownCoroutine());
        }
        if (inputVec.x != 0)
        {
            animator.SetBool("Right_Move", true);
            animator.SetBool("Idle", false);
            sr.flipX = inputVec.x < 0;
        }
        else
        {
            animator.SetBool("Right_Move", false);
            animator.SetBool("Idle", true);
        }
        if (SceneManager.GetActiveScene().name == "Easy")
        {
            test = 0;
        }
        if (SceneManager.GetActiveScene().name == "Nomal")
        {
            test = 1;
        }
        if (SceneManager.GetActiveScene().name == "Hard")
        {
            test = 2;
        }
    }

    public void FixedUpdate()
    {
        if (!canMove) return; // 움직임이 불가능하면 물리 업데이트 무시

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

    public void CheckHp() // HP 갱신
    {
        if (HpBarSlider != null)
        {
            StartCoroutine(LerpHpBar(curHealth)); // 코루틴을 사용하여 슬라이더 값을 부드럽게 변경
        }
    }

    private IEnumerator LerpHpBar(float targetHealth)
    {
        float startValue = HpBarSlider.value;
        float endValue = targetHealth;
        float elapsedTime = 0f;

        while (elapsedTime < 1f / sliderLerpSpeed)
        {
            elapsedTime += Time.deltaTime * sliderLerpSpeed;
            HpBarSlider.value = Mathf.Lerp(startValue, endValue, elapsedTime);
            yield return null;
        }

        HpBarSlider.value = endValue; // 마지막 값을 정확하게 설정
    }

    public void Damage(float damage)
    {
        if (isInvincible || curHealth <= 0)
            return;

        curHealth -= damage; // 현재 체력 감소
        CheckHp(); // 데미지를 입을 때 체력바 갱신

        if (curHealth <= 0)
        {
            // 죽을 때 초기화 메서드 호출
            InitializePlayer();

            // GAME OVER 씬으로 전환
            SceneManager.LoadScene("GAME OVER");
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
        if (target == null)
        {
            Debug.LogError("Attack target is null.");
            return;
        }

        // 공격 로직 추가 (타겟의 체력 감소 등)
        var enemySlime = target.GetComponent<EnemySlime>();
        var enemySlimeHard = target.GetComponent<EnemySlimeHard>();
        var enemySlimeNomal = target.GetComponent<EnemySlimeNomal>();
        var wormEasy = target.GetComponent<WormEasy>();
        var boss = target.GetComponent<BossEasy>(); // Boss 컴포넌트 접근

        if (enemySlime != null)
        {
            enemySlime.TakeDamage(AttackDamage);
        }
        else if (enemySlimeHard != null)
        {
            enemySlimeHard.TakeDamage(AttackDamage);
        }
        else if (enemySlimeNomal != null)
        {
            enemySlimeNomal.TakeDamage(AttackDamage);
        }
        else if (wormEasy != null)
        {
            wormEasy.TakeDamage(AttackDamage);
        }
        else if (boss != null) // Boss 공격 처리 추가
        {
            boss.TakeDamage(AttackDamage);
        }
        else
        {
            Debug.LogError("EnemySlime, EnemySlimeHard, EnemySlimeNomal, WormEasy, or Boss component not found on target.");
        }
    }

    public void DetectAndAttackEnemy()
    {
        // 적 레이어에 속한 모든 적 공격
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, attackRange, LayerMask.GetMask("Enemy"));

        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy != null && enemy.transform != null)
            {
                if (Mathf.Abs(enemy.transform.position.x - transform.position.x) <= attackRange)
                {
                    Attack(enemy.transform);
                    break;
                }
            }
        }

        // Boss 태그를 가진 오브젝트도 공격 가능하게 처리
        Collider2D[] hitBosses = Physics2D.OverlapCircleAll(transform.position, attackRange);

        foreach (Collider2D boss in hitBosses)
        {
            if (boss.CompareTag("Boss"))
            {
                if (Mathf.Abs(boss.transform.position.x - transform.position.x) <= attackRange)
                {
                    Attack(boss.transform);
                    break;
                }
            }
        }
    }

    private IEnumerator AttackCooldownCoroutine()
    {
        canAttack = false;
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    public void Jump()
    {
        animator.SetBool("jumping", true);
        isJump = true;
        rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
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
