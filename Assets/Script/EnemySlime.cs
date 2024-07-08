using UnityEngine;

public class EnemySlime : MonoBehaviour
{
    [SerializeField]
    public float speed = 2.0f; // ���� �̵� �ӵ�
    public float damage = 5.0f; // �÷��̾�� ���� ������
    public float maxHealth = 10.0f; // ���� �ִ� ü��
    public float curHealth; // ���� ���� ü��
    private Transform player;  // ���ΰ��� Transform

    void Start()
    {
        // ���ΰ� ������Ʈ�� ã���ϴ�
        player = GameObject.FindGameObjectWithTag("Player").transform;
        curHealth = maxHealth; // �ʱ� ü���� �ִ� ü������ ����
    }

    void Update()
    {
        // ���� ���ΰ��� ���� �̵��ϵ��� ����
        Vector3 direction = player.position - transform.position;
        direction.Normalize();
        transform.position += direction * speed * Time.deltaTime;
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
            }
        }
    }

    public void TakeDamage(float damage)
    {
        curHealth -= damage;
        if (curHealth <= 0)
        {
            // ���� �׾��� ���� ����
            Destroy(gameObject);
        }
    }
}
