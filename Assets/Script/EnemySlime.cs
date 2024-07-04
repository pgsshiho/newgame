using UnityEngine;

public class EnemySlime : MonoBehaviour
{
    public float speed = 2.0f; // 적의 이동 속도
    private Transform player;  // 주인공의 Transform

    void Start()
    {
        // 주인공 오브젝트를 찾습니다
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        // 적이 주인공을 향해 이동하도록 설정
        Vector3 direction = player.position - transform.position;
        direction.Normalize();
        transform.position += direction * speed * Time.deltaTime;
    }
}
