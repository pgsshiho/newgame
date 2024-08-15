using System.Collections;
using System.Collections.Generic;
using TMPro; // TextMeshProUGUI를 사용하기 위해 추가
using UnityEngine;

public class EnemyManager2 : MonoBehaviour
{
    public GameObject enemyPrefab; // 적 프리팹
    public Transform leftSpawnPoint; // 적이 왼쪽에서 생성될 위치
    public Transform rightSpawnPoint; // 적이 오른쪽에서 생성될 위치
    public float spawnInterval = 8.0f; // 적이 생성되는 간격
    public TextMeshProUGUI enemyCountText; // 남은 적 수를 표시할 텍스트
    public GameObject[] shops; // 여러 Shop 오브젝트를 배열로 관리

    private Timer timer; // Timer 스크립트 참조
    private List<GameObject> enemies = new List<GameObject>(); // 생성된 적을 추적하는 리스트
    private Coroutine spawnCoroutine;

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
            // 타이머가 종료될 때 적 생성을 중지하도록 이벤트 등록
            timer.OnTimerEnd += StopSpawningEnemies;

            // 오브젝트 활성화 시 적 생성을 시작하도록 설정
            StartSpawningEnemies();
        }

        UpdateEnemyCountDisplay(); // 시작 시 남은 적 수 업데이트
    }

    void OnEnable()
    {
        // 오브젝트가 활성화될 때 적 생성을 시작
        StartSpawningEnemies();
    }

    void OnDisable()
    {
        // 오브젝트가 비활성화될 때 적 생성을 중지
        StopSpawningEnemies();
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

                    // 적 방향 설정
                    WormEasy worm = enemy.GetComponent<WormEasy>();
                    if (worm != null)
                    {
                        worm.isLeft = isLeft;
                    }

                    UpdateEnemyCountDisplay(); // 적 생성 후 남은 적 수 업데이트
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
        foreach (GameObject enemy in enemies)
        {
            if (enemy != null && enemy.layer == LayerMask.NameToLayer("Enemy"))
            {
                Destroy(enemy);
            }
        }
        enemies.Clear();
        UpdateEnemyCountDisplay(); // 적이 제거된 후 남은 적 수 업데이트
        Debug.Log("Enemy 레이어에 속한 모든 적이 제거되었습니다.");
    }

    // 남은 적 수를 업데이트하고 화면에 표시하는 메서드
    private void UpdateEnemyCountDisplay()
    {
        if (enemyCountText != null)
        {
            enemyCountText.text = $"{enemies.Count}";
        }
    }

    // 오브젝트가 활성화될 때 적 스폰을 시작하는 메서드
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

    // 타이머가 종료되거나 오브젝트가 비활성화될 때 적 스폰을 중지하는 메서드
    public void StopSpawningEnemies()
    {
        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
            spawnCoroutine = null;
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
