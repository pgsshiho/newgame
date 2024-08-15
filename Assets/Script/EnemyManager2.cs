using System.Collections;
using System.Collections.Generic;
using TMPro; // TextMeshProUGUI�� ����ϱ� ���� �߰�
using UnityEngine;

public class EnemyManager2 : MonoBehaviour
{
    public GameObject enemyPrefab; // �� ������
    public Transform leftSpawnPoint; // ���� ���ʿ��� ������ ��ġ
    public Transform rightSpawnPoint; // ���� �����ʿ��� ������ ��ġ
    public float spawnInterval = 8.0f; // ���� �����Ǵ� ����
    public TextMeshProUGUI enemyCountText; // ���� �� ���� ǥ���� �ؽ�Ʈ
    public GameObject[] shops; // ���� Shop ������Ʈ�� �迭�� ����

    private Timer timer; // Timer ��ũ��Ʈ ����
    private List<GameObject> enemies = new List<GameObject>(); // ������ ���� �����ϴ� ����Ʈ
    private Coroutine spawnCoroutine;

    void Start()
    {
        // ������ �ùٸ��� �����Ǿ����� Ȯ��
        if (enemyPrefab == null)
        {
            Debug.LogError("enemyPrefab�� �Ҵ���� �ʾҽ��ϴ�.");
        }
        if (leftSpawnPoint == null)
        {
            Debug.LogError("leftSpawnPoint�� �Ҵ���� �ʾҽ��ϴ�.");
        }
        if (rightSpawnPoint == null)
        {
            Debug.LogError("rightSpawnPoint�� �Ҵ���� �ʾҽ��ϴ�.");
        }
        if (enemyCountText == null)
        {
            Debug.LogError("enemyCountText�� �Ҵ���� �ʾҽ��ϴ�.");
        }
        if (shops == null || shops.Length == 0)
        {
            Debug.LogError("Shop ������Ʈ�� �Ҵ���� �ʾҽ��ϴ�.");
        }

        timer = FindObjectOfType<Timer>();
        if (timer == null)
        {
            Debug.LogError("Timer ��ũ��Ʈ�� ã�� �� �����ϴ�. ���� Timer�� �ִ��� Ȯ���ϼ���.");
        }
        else
        {
            // Ÿ�̸Ӱ� ����� �� �� ������ �����ϵ��� �̺�Ʈ ���
            timer.OnTimerEnd += StopSpawningEnemies;

            // ������Ʈ Ȱ��ȭ �� �� ������ �����ϵ��� ����
            StartSpawningEnemies();
        }

        UpdateEnemyCountDisplay(); // ���� �� ���� �� �� ������Ʈ
    }

    void OnEnable()
    {
        // ������Ʈ�� Ȱ��ȭ�� �� �� ������ ����
        StartSpawningEnemies();
    }

    void OnDisable()
    {
        // ������Ʈ�� ��Ȱ��ȭ�� �� �� ������ ����
        StopSpawningEnemies();
    }

    // ���ο� ���� �����ϴ� �޼ҵ�
    public void SpawnEnemy(Transform spawnPoint, bool isLeft)
    {
        if (AllShopsInactive())
        {
            if (timer != null && !timer.TimerEnded)
            {
                if (enemyPrefab != null && spawnPoint != null)
                {
                    GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
                    enemies.Add(enemy); // ������ ���� ����Ʈ�� �߰�

                    // �� ���� ����
                    WormEasy worm = enemy.GetComponent<WormEasy>();
                    if (worm != null)
                    {
                        worm.isLeft = isLeft;
                    }

                    UpdateEnemyCountDisplay(); // �� ���� �� ���� �� �� ������Ʈ
                }
                else
                {
                    Debug.LogError("enemyPrefab �Ǵ� spawnPoint�� null�Դϴ�.");
                }
            }
            else
            {
                Debug.Log("Ÿ�̸Ӱ� ���� �� �̻� ���� �������� �ʽ��ϴ�.");
            }
        }
        else
        {
            Debug.Log("�ϳ� �̻��� Shop�� Ȱ��ȭ�Ǿ� �־� ���� �������� �ʽ��ϴ�.");
        }
    }

    private bool AllShopsInactive()
    {
        foreach (GameObject shop in shops)
        {
            if (shop.activeSelf)
            {
                Debug.Log("Shop Ȱ��ȭ��: " + shop.name);
                return false; // Shop �� �ϳ��� Ȱ��ȭ�Ǿ� ������ false ��ȯ
            }
        }
        return true; // ��� Shop�� ��Ȱ��ȭ �����̸� true ��ȯ
    }

    private IEnumerator SpawnEnemyCoroutine()
    {
        while (timer != null && !timer.TimerEnded)
        {
            SpawnEnemy(leftSpawnPoint, true); // ���ʿ��� ����
            SpawnEnemy(rightSpawnPoint, false); // �����ʿ��� ����
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    // ���� ���ŵ� �� ����Ʈ���� ����
    public void RemoveEnemy(GameObject enemy)
    {
        if (enemies.Contains(enemy))
        {
            enemies.Remove(enemy);
            Destroy(enemy);
            UpdateEnemyCountDisplay(); // ���� ���ŵ� �� ���� �� �� ������Ʈ
            Debug.Log($"���� ���ŵǾ����ϴ�: {enemy.name}");
        }
        else
        {
            Debug.LogWarning("���� ����Ʈ���� ã�� �� �����ϴ�.");
        }
    }

    // ��� �� ���� �޼ҵ�
    public void RemoveAllEnemies()
    {
        foreach (GameObject enemy in enemies)
        {
            if (enemy != null && enemy.layer == LayerMask.NameToLayer("Enemy"))
            {
                Destroy(enemy);
            }
        }
        enemies.Clear();
        UpdateEnemyCountDisplay(); // ���� ���ŵ� �� ���� �� �� ������Ʈ
        Debug.Log("Enemy ���̾ ���� ��� ���� ���ŵǾ����ϴ�.");
    }

    // ���� �� ���� ������Ʈ�ϰ� ȭ�鿡 ǥ���ϴ� �޼���
    private void UpdateEnemyCountDisplay()
    {
        if (enemyCountText != null)
        {
            enemyCountText.text = $"{enemies.Count}";
        }
    }

    // ������Ʈ�� Ȱ��ȭ�� �� �� ������ �����ϴ� �޼���
    public void StartSpawningEnemies()
    {
        if (timer != null && !timer.TimerEnded)
        {
            if (spawnCoroutine == null)
            {
                spawnCoroutine = StartCoroutine(SpawnEnemyCoroutine());
            }
        }
    }

    // Ÿ�̸Ӱ� ����ǰų� ������Ʈ�� ��Ȱ��ȭ�� �� �� ������ �����ϴ� �޼���
    public void StopSpawningEnemies()
    {
        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
            spawnCoroutine = null;
        }
    }

    // ���� ������ �� ����Ʈ�� �߰�
    public void AddEnemy(GameObject enemy)
    {
        if (!enemies.Contains(enemy))
        {
            enemies.Add(enemy);
            UpdateEnemyCountDisplay(); // ���� ������ �� ���� �� �� ������Ʈ
        }
    }
}
