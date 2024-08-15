using System.Collections;
using UnityEngine;

public class WormEasy : MonoBehaviour
{
    [SerializeField]
    public float speed = 1.0f; // 적의 이동 속도
    public float damage = 7.0f; // 플레이어에게 입힐 데미지
    public float maxHealth = 15.0f; // 적의 최대 체력
    public float curHealth; // 적의 현재 체력
    private Transform player;  // 주인공의 Transform
    private SpriteRenderer sr; // 적의 SpriteRenderer
    private Rigidbody2D rb; // 적의 Rigidbody2D
    private bool isKnockedBack = false; // 넉백 상태
    public bool isLeft; // 적이 왼쪽에서 생성된 경우 true, 오른쪽에서 생성된 경우 false
    private StageManager stageManager; // StageManager 인스턴스

    void Start()
    {
        // 주인공 오브젝트를 찾습니다
        player = GameObject.FindGameObjectWithTag("Player").transform;
        curHealth = maxHealth; // 초기 체력을 최대 체력으로 설정
        sr = GetComponent<SpriteRenderer>(); // SpriteRenderer 컴포넌트를 가져옵니다
        rb = GetComponent<Rigidbody2D>(); // Rigidbody2D 컴포넌트를 가져옵니다

        // StageManager 인스턴스를 가져옵니다
        stageManager = StageManager.Instance;
        if (stageManager == null)
        {
            Debug.LogError("StageManager instance not found!");
        }
    }

    void Update()
    {
        if (isKnockedBack) return; // 넉백 중에는 이동하지 않습니다

        // 적이 주인공을 향해 이동하도록 설정
        Vector3 direction = player.position - transform.position;
        direction.Normalize();

        // 이동 방향에 따라 스프라이트 뒤집기
        if (direction.x > 0) // 오른쪽으로 이동 중
        {
            sr.flipX = true;
        }
        else if (direction.x < 0) // 왼쪽으로 이동 중
        {
            sr.flipX = false;
        }

        transform.position += direction * speed * Time.deltaTime;
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
            }
        }
    }

    public void TakeDamage(float damage)
    {
        curHealth -= damage;

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
            StartCoroutine(InvincibilityCoroutine());
        }
    }

    private IEnumerator InvincibilityCoroutine()
    {
        sr.color = Color.red; // 빨간색으로 변경
        isKnockedBack = true;

        // 넉백 처리
        Vector2 knockbackDirection = (transform.position - player.position).normalized;
        rb.AddForce(knockbackDirection * 3f, ForceMode2D.Impulse);

        yield return new WaitForSeconds(1.5f);

        sr.color = Color.white; // 원래 색으로 복원
        isKnockedBack = false;
    }
}
