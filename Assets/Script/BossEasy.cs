using System.Collections;
using UnityEngine;

public class BossEasy : MonoBehaviour
{
    [SerializeField]
    public float speed = 2.0f; // 보스의 이동 속도
    public float damage = 30.0f; // 플레이어에게 입힐 데미지
    public float maxHealth = 300.0f; // 보스의 최대 체력
    public float curHealth; // 보스의 현재 체력
    private Transform player;  // 주인공의 Transform
    private SpriteRenderer sr; // 보스의 SpriteRenderer
    private Rigidbody2D rb; // 보스의 Rigidbody2D
    private bool isPaused = false; // 보스가 멈춘 상태인지 여부
    private StageManager stageManager; // StageManager 인스턴스
    private BossManager bossManager; // BossManager 인스턴스
    private bool isInvincible = false; // 무적 상태

    public float invincibilityDuration = 1.0f; // 무적 지속 시간
    public float flashDuration = 0.1f; // 깜빡임 간격

    void Start()
    {
        // 주인공 오브젝트를 찾습니다
        player = GameObject.FindGameObjectWithTag("Player").transform;
        curHealth = maxHealth; // 초기 체력을 최대 체력으로 설정
        sr = GetComponent<SpriteRenderer>(); // SpriteRenderer 컴포넌트를 가져옵니다
        rb = GetComponent<Rigidbody2D>(); // Rigidbody2D 컴포넌트를 가져옵니다

        // 보스가 중력의 영향을 받지 않도록 설정
        rb.gravityScale = 0;
        rb.isKinematic = true; // 보스를 물리적인 반응 없이 움직이게 설정

        // StageManager 인스턴스를 가져옵니다
        stageManager = StageManager.Instance;
        if (stageManager == null)
        {
            Debug.LogError("StageManager instance not found!");
        }

        // BossManager 인스턴스를 가져옵니다
        bossManager = FindObjectOfType<BossManager>();
        if (bossManager == null)
        {
            Debug.LogError("BossManager instance not found!");
        }
    }

    void Update()
    {
        if (isPaused) return; // 멈춘 상태에서는 이동하지 않음

        // 보스가 주인공을 향해 이동하도록 설정
        MoveTowardsPlayer();
    }

    private void MoveTowardsPlayer()
    {
        Vector3 direction = player.position - transform.position;
        direction.Normalize();

        // 이동 방향에 따라 스프라이트 뒤집기
        sr.flipX = direction.x > 0;

        // 보스의 이동 처리
        transform.position += direction * speed * Time.deltaTime;

        // Y 값이 0 이하로 내려가지 않도록 제한
        if (transform.position.y < 0)
        {
            transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // 플레이어에게 데미지를 입힙니다
            Player playerScript = collision.gameObject.GetComponent<Player>();
            if (playerScript != null)
            {
                playerScript.Damage(damage);

                // 충돌 후에도 계속 플레이어를 추적합니다. 밀리지 않도록 설정.
                StartCoroutine(AttackAndContinueChase());
            }
        }
    }

    private IEnumerator AttackAndContinueChase()
    {
        isPaused = true; // 공격 중에는 잠시 멈춤
        yield return new WaitForSeconds(0.5f); // 0.5초 후 다시 이동
        isPaused = false;
    }

    public void TakeDamage(float damage)
    {
        if (isInvincible) return; // 무적 상태에서는 데미지를 받지 않음

        curHealth -= damage;

        // 보스 체력이 변경될 때마다 BossManager의 UI를 업데이트
        if (bossManager != null)
        {
            bossManager.UpdateBossHealthUI();
        }

        if (curHealth <= 0)
        {
            if (stageManager != null)
            {
                stageManager.dia += 70;
                stageManager.UpdateDiaCount(); // 다이아몬드 수 업데이트
            }
            else
            {
                Debug.LogWarning("StageManager 객체가 null입니다.");
            }
            Destroy(gameObject);
        }
        else
        {
            // 데미지를 받을 때 무적 상태로 전환하고 깜빡임 및 멈춤 처리
            StartCoroutine(InvincibilityCoroutine());
        }
    }

    private IEnumerator InvincibilityCoroutine()
    {
        isInvincible = true;
        isPaused = true; // 무적 상태일 때 이동 멈춤

        for (float i = 0; i < invincibilityDuration; i += flashDuration)
        {
            sr.enabled = !sr.enabled; // 깜빡임 처리
            yield return new WaitForSeconds(flashDuration);
        }

        sr.enabled = true; // 깜빡임 종료 후 원래 상태로 복원
        isPaused = false; // 다시 이동 가능
        isInvincible = false; // 무적 상태 해제
    }
}
