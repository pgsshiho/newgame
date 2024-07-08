using UnityEngine;

public class EnemySlime : MonoBehaviour
{
    [SerializeField]
    public float speed = 2.0f; // 적의 이동 속도
    public float damage = 5.0f; // 플레이어에게 입힐 데미지
    public float maxHealth = 10.0f; // 적의 최대 체력
    public float curHealth; // 적의 현재 체력
    private Transform player;  // 주인공의 Transform

    void Start()
    {
        // 주인공 오브젝트를 찾습니다
        player = GameObject.FindGameObjectWithTag("Player").transform;
        curHealth = maxHealth; // 초기 체력을 최대 체력으로 설정
    }

    void Update()
    {
        // 적이 주인공을 향해 이동하도록 설정
        Vector3 direction = player.position - transform.position;
        direction.Normalize();
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
            // 적이 죽었을 때의 로직
            Destroy(gameObject);
        }
    }
}
