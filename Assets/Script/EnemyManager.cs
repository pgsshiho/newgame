using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyManager : MonoBehaviour
{
    public GameObject enemyPrefab; // 적 프리팹
    public Transform leftSpawnPoint; // 적이 왼쪽에서 생성될 위치
    public Transform rightSpawnPoint; // 적이 오른쪽에서 생성될 위치
    public float spawnInterval = 8.0f; // 적이 생성되는 간격
    public TextMeshProUGUI enemyCountText; // 남은 적 수를 표시할 텍스트
    public GameObject[] shops; // 여러 Shop 오브젝트를 배열로 관리

    private Timer timer; // Timer 스크립트 참조
    private List<GameObject> enemies = new List<GameObject>(); // 생성된 적을 추적하는 리스트

    void Start()
    {
        // 참조가 올바르게 설정되었는지 확인
        if (enemyPrefab == null)
        {
            Debug.LogError("enemyPrefab이 할당되지 않았습니다.");
        }
        if (leftSpawnPoint == null)
        {
            Debug.LogError("leftSpawnPoint가 할당되지 않았습니다.");
        }
        if (rightSpawnPoint == null)
        {
            Debug.LogError("rightSpawnPoint가 할당되지 않았습니다.");
        }
        if (enemyCountText == null)
        {
            Debug.LogError("enemyCountText가 할당되지 않았습니다.");
        }
        if (shops == null || shops.Length == 0)
        {
            Debug.LogError("Shop 오브젝트가 할당되지 않았습니다.");
        }

        timer = FindObjectOfType<Timer>();
        if (timer == null)
        {
            Debug.LogError("Timer 스크립트를 찾을 수 없습니다. 씬에 Timer가 있는지 확인하세요.");
        }
        else
        {
            // 타이머가 종료될 때 코루틴을 재시작하도록 이벤트 등록
            timer.OnTimerEnd += StartSpawningEnemies;

            // 적을 생성하는 코루틴 시작
            StartCoroutine(SpawnEnemyCoroutine());
        }

        UpdateEnemyCountDisplay(); // 시작 시 남은 적 수 업데이트
    }

    // 새로운 적을 생성하는 메소드
    public void SpawnEnemy(Transform spawnPoint, bool isLeft)
    {
        if (AllShopsInactive())
        {
            if (timer != null && !timer.TimerEnded)
            {
                if (enemyPrefab != null && spawnPoint != null)
                {
                    GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
                    enemies.Add(enemy); // 생성된 적을 리스트에 추가
                    EnemySlime enemySlime = enemy.GetComponent<EnemySlime>();
                    if (enemySlime != null)
                    {
                        enemySlime.isLeft = isLeft; // 왼쪽에서 생성된 경우 true, 오른쪽에서 생성된 경우 false
                        enemySlime.enemyManager = this; // EnemyManager에 대한 참조를 설정
                    }

                    UpdateEnemyCountDisplay(); // 적이 생성될 때 남은 적 수 업데이트
                }
                else
                {
                    Debug.LogError("enemyPrefab 또는 spawnPoint가 null입니다.");
                }
            }
            else
            {
                Debug.Log("타이머가 끝나 더 이상 적을 생성하지 않습니다.");
            }
        }
        else
        {
            Debug.Log("하나 이상의 Shop이 활성화되어 있어 적을 생성하지 않습니다.");
        }
    }

    private bool AllShopsInactive()
    {
        foreach (GameObject shop in shops)
        {
            if (shop.activeSelf)
            {
                Debug.Log("Shop 활성화됨: " + shop.name);
                return false; // Shop 중 하나라도 활성화되어 있으면 false 반환
            }
        }
        return true; // 모든 Shop이 비활성화 상태이면 true 반환
    }

    private IEnumerator SpawnEnemyCoroutine()
    {
        while (timer != null && !timer.TimerEnded)
        {
            SpawnEnemy(leftSpawnPoint, true); // 왼쪽에서 생성
            SpawnEnemy(rightSpawnPoint, false); // 오른쪽에서 생성
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    // 적이 제거될 때 리스트에서 삭제
    public void RemoveEnemy(GameObject enemy)
    {
        if (enemies.Contains(enemy))
        {
            enemies.Remove(enemy);
            Destroy(enemy);
            UpdateEnemyCountDisplay(); // 적이 제거된 후 남은 적 수 업데이트
            Debug.Log($"적이 제거되었습니다: {enemy.name}");
        }
        else
        {
            Debug.LogWarning("적을 리스트에서 찾을 수 없습니다.");
        }
    }

    // 모든 적 제거 메소드
    public void RemoveAllEnemies()
    {
        StartCoroutine(RemoveAllEnemiesCoroutine());
    }

    private IEnumerator RemoveAllEnemiesCoroutine()
    {
        foreach (GameObject enemy in enemies)
        {
            if (enemy != null)
            {
                Destroy(enemy);
            }
        }

        enemies.Clear();
        UpdateEnemyCountDisplay(); // 적이 제거된 후 남은 적 수 업데이트
        Debug.Log("모든 적이 제거되었습니다.");

        // 약간의 지연 후 적 생성 재시작
        yield return new WaitForSeconds(0.5f);

        if (timer != null && !timer.TimerEnded)
        {
            StartCoroutine(SpawnEnemyCoroutine());
        }
    }

    // 남은 적 수를 업데이트하고 화면에 표시하는 메서드
    private void UpdateEnemyCountDisplay()
    {
        if (enemyCountText != null)
        {
            enemyCountText.text = $"{enemies.Count}";
        }
    }

    // 적이 남아있는지 확인하는 메소드
    public bool AreAllEnemiesRemoved()
    {
        return enemies.Count == 0;
    }

    // 타이머가 재시작될 때 적 스폰을 다시 시작하는 메서드
    private void StartSpawningEnemies()
    {
        if (timer != null && !timer.TimerEnded)
        {
            StartCoroutine(SpawnEnemyCoroutine());
        }
    }

    // 적이 생성될 때 리스트에 추가
    public void AddEnemy(GameObject enemy)
    {
        if (!enemies.Contains(enemy))
        {
            enemies.Add(enemy);
            UpdateEnemyCountDisplay(); // 적이 생성될 때 남은 적 수 업데이트
        }
    }
}
