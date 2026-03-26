using UnityEngine;

public class WakameSpawner : MonoBehaviour
{
    public GameObject wakamePrefab;
    public int spawnCount = 50;
    public float radius = 30f;

    public float minY = -3f;  // 浅瀬の下限（深さ）
    public float maxY = 6.46f;   // 水面（0くらい想定）

    void Start()
    {
        for (int i = 0; i < spawnCount; i++)
        {
            // ランダムな位置（上空からRay飛ばす）
            Vector2 rand = Random.insideUnitCircle * radius;
            Vector3 rayPos = new Vector3(rand.x, 20f, rand.y);

            RaycastHit hit;
            if (Physics.Raycast(rayPos, Vector3.down, out hit, 50f))
            {
                // Groundに当たって、かつ高さが範囲内なら生成
                if (hit.collider.CompareTag("Ground") &&
                    hit.point.y >= minY &&
                    hit.point.y <= maxY)
                {
                    Instantiate(wakamePrefab, hit.point, Quaternion.identity);
                }
            }
        }
    }
}
