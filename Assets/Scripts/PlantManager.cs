using UnityEngine;

public class PlantManager : MonoBehaviour
{
    public GameObject cropPrefab;
    public float plantHeight = 0.5f;
    public float rayDistance = 3f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            // 足元からRay
            Ray ray = new Ray(transform.position + Vector3.down * 1f, Vector3.down);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, rayDistance))
            {
                Debug.Log("当たった: " + hit.collider.name);

                if (hit.collider.CompareTag("Field"))
                {
                    GameObject field = hit.collider.gameObject;

                    // すでに植えてあるか
                    if (field.transform.childCount > 0)
                    {
                        Debug.Log("すでに植えてある");
                        return;
                    }

                    // 植える位置
                    Vector3 plantPos = field.transform.position;
                    plantPos.y += plantHeight;

                    // 作物生成
                    GameObject crop = Instantiate(cropPrefab, plantPos, Quaternion.identity);

                    // 畑の子にする
                    crop.transform.parent = field.transform;

                    Debug.Log("植えた！");
                }
                else
                {
                    Debug.Log("畑じゃない：" + hit.collider.name);
                }
            }
            else
            {
                Debug.Log("何も当たってない");
            }
        }
    }
}