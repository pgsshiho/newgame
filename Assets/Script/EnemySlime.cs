using UnityEngine;

public class EnemySlime : MonoBehaviour
{
    public float speed = 2.0f; // ���� �̵� �ӵ�
    private Transform player;  // ���ΰ��� Transform

    void Start()
    {
        // ���ΰ� ������Ʈ�� ã���ϴ�
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        // ���� ���ΰ��� ���� �̵��ϵ��� ����
        Vector3 direction = player.position - transform.position;
        direction.Normalize();
        transform.position += direction * speed * Time.deltaTime;
    }
}
