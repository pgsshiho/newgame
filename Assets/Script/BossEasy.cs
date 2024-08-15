using System.Collections;
using UnityEngine;

public class BossEasy : MonoBehaviour
{
    [SerializeField]
    public float speed = 2.0f; // ������ �̵� �ӵ�
    public float damage = 30.0f; // �÷��̾�� ���� ������
    public float maxHealth = 300.0f; // ������ �ִ� ü��
    public float curHealth; // ������ ���� ü��
    private Transform player;  // ���ΰ��� Transform
    private SpriteRenderer sr; // ������ SpriteRenderer
    private Rigidbody2D rb; // ������ Rigidbody2D
    private bool isPaused = false; // ������ ���� �������� ����
    private StageManager stageManager; // StageManager �ν��Ͻ�
    private BossManager bossManager; // BossManager �ν��Ͻ�
    private bool isInvincible = false; // ���� ����

    public float invincibilityDuration = 1.0f; // ���� ���� �ð�
    public float flashDuration = 0.1f; // ������ ����

    void Start()
    {
        // ���ΰ� ������Ʈ�� ã���ϴ�
        player = GameObject.FindGameObjectWithTag("Player").transform;
        curHealth = maxHealth; // �ʱ� ü���� �ִ� ü������ ����
        sr = GetComponent<SpriteRenderer>(); // SpriteRenderer ������Ʈ�� �����ɴϴ�
        rb = GetComponent<Rigidbody2D>(); // Rigidbody2D ������Ʈ�� �����ɴϴ�

        // ������ �߷��� ������ ���� �ʵ��� ����
        rb.gravityScale = 0;
        rb.isKinematic = true; // ������ �������� ���� ���� �����̰� ����

        // StageManager �ν��Ͻ��� �����ɴϴ�
        stageManager = StageManager.Instance;
        if (stageManager == null)
        {
            Debug.LogError("StageManager instance not found!");
        }

        // BossManager �ν��Ͻ��� �����ɴϴ�
        bossManager = FindObjectOfType<BossManager>();
        if (bossManager == null)
        {
            Debug.LogError("BossManager instance not found!");
        }
    }

    void Update()
    {
        if (isPaused) return; // ���� ���¿����� �̵����� ����

        // ������ ���ΰ��� ���� �̵��ϵ��� ����
        MoveTowardsPlayer();
    }

    private void MoveTowardsPlayer()
    {
        Vector3 direction = player.position - transform.position;
        direction.Normalize();

        // �̵� ���⿡ ���� ��������Ʈ ������
        sr.flipX = direction.x > 0;

        // ������ �̵� ó��
        transform.position += direction * speed * Time.deltaTime;

        // Y ���� 0 ���Ϸ� �������� �ʵ��� ����
        if (transform.position.y < 0)
        {
            transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // �÷��̾�� �������� �����ϴ�
            Player playerScript = collision.gameObject.GetComponent<Player>();
            if (playerScript != null)
            {
                playerScript.Damage(damage);

                // �浹 �Ŀ��� ��� �÷��̾ �����մϴ�. �и��� �ʵ��� ����.
                StartCoroutine(AttackAndContinueChase());
            }
        }
    }

    private IEnumerator AttackAndContinueChase()
    {
        isPaused = true; // ���� �߿��� ��� ����
        yield return new WaitForSeconds(0.5f); // 0.5�� �� �ٽ� �̵�
        isPaused = false;
    }

    public void TakeDamage(float damage)
    {
        if (isInvincible) return; // ���� ���¿����� �������� ���� ����

        curHealth -= damage;

        // ���� ü���� ����� ������ BossManager�� UI�� ������Ʈ
        if (bossManager != null)
        {
            bossManager.UpdateBossHealthUI();
        }

        if (curHealth <= 0)
        {
            if (stageManager != null)
            {
                stageManager.dia += 70;
                stageManager.UpdateDiaCount(); // ���̾Ƹ�� �� ������Ʈ
            }
            else
            {
                Debug.LogWarning("StageManager ��ü�� null�Դϴ�.");
            }
            Destroy(gameObject);
        }
        else
        {
            // �������� ���� �� ���� ���·� ��ȯ�ϰ� ������ �� ���� ó��
            StartCoroutine(InvincibilityCoroutine());
        }
    }

    private IEnumerator InvincibilityCoroutine()
    {
        isInvincible = true;
        isPaused = true; // ���� ������ �� �̵� ����

        for (float i = 0; i < invincibilityDuration; i += flashDuration)
        {
            sr.enabled = !sr.enabled; // ������ ó��
            yield return new WaitForSeconds(flashDuration);
        }

        sr.enabled = true; // ������ ���� �� ���� ���·� ����
        isPaused = false; // �ٽ� �̵� ����
        isInvincible = false; // ���� ���� ����
    }
}
