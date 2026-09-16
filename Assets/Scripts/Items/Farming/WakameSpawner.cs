using UnityEngine;

public class WakameSpawner : MonoBehaviour
{
    [SerializeField] private GameObject wakamePrefab;
    [SerializeField] private int wakameCount = 30;
    [SerializeField] private float radius = 30f;
    

    [SerializeField] private float minY = -5f;  // 浅瀬の下限（深さ）
    [SerializeField] private float maxY = 6.46f;   // 水面（0くらい想定）

    void Start()
    {
        SpawnWakame(wakameCount);
    }

    private void SpawnWakame(int count = 1)
    {
        int spawned = 0;
        int maxAttempts = count * 20;

        for (int i = 0; i < maxAttempts; i++)
        { 
            if (spawned >= count) break;

            // 位置抽選
            if (TryGetSpawnPosition(out Vector3 spawnPos))
            {
                // 生成
                SpawnWakameAt(spawnPos);
                spawned++;
                Debug.Log($"ワカメを植えました！{spawned}");
            }
        }
    }

    private bool TryGetSpawnPosition(out Vector3 spawnPosition)
    {
        spawnPosition = Vector3.zero;

        Vector2 rand = Random.insideUnitCircle * radius;
        Vector3 rayPos = new Vector3(rand.x, 20f, rand.y);

        if (Physics.Raycast(rayPos, Vector3.down, out RaycastHit hit, 50f))
        {
            // Groundに当たって、かつ高さが範囲内なら成功
            if (hit.collider.CompareTag("Ground") &&
                hit.point.y >= minY &&
                hit.point.y <= maxY)
            {
                spawnPosition = hit.point;
                return true;
            }
        }

        return false;
    }

    private void SpawnWakameAt(Vector3 position)
    {
        GameObject obj = Instantiate(wakamePrefab, position, Quaternion.identity, transform);

        if (obj.TryGetComponent<CollectWakame>(out var wakame))
        {
            wakame.OnHarvested += OnWakameHarvested;
        }
    }

    private void OnWakameHarvested(CollectWakame harvestedWakame)
    {
        Debug.Log($"ワカメを収穫しました！");
        SpawnWakame(1);
    }
}
