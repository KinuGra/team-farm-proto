using UnityEngine;

public class PlantManager : MonoBehaviour
{
    public GameObject cropPrefab;
    public float plantHeight = 0.1f;
    public float rayDistance = 10f;

    // 🌟 ハイライト用
    private GameObject currentField;
    private Renderer currentRenderer;
    private Color originalColor;

    public Color canPlantColor = Color.green;   // 植えられる
    public Color cannotPlantColor = Color.red;  // 植えられない

    void Update()
    {
        HighlightField();

        if (Input.GetKeyDown(KeyCode.F))
        {
            TryPlant();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            TryHarvest();
        }
    }

    // 🌱 植える
    void TryPlant()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, rayDistance))
        {
            if (hit.collider.CompareTag("Field"))
            {
                GameObject field = hit.collider.gameObject;

                if (field.GetComponentInChildren<Crop>() != null)
                {
                    Debug.Log("すでに植えてある");
                    return;
                }

                Vector3 pos = field.transform.position;
                pos.y += plantHeight;

                GameObject crop = Instantiate(cropPrefab, pos, Quaternion.identity);

                // 👇 これが超重要
                crop.transform.SetParent(field.transform);

                Debug.Log("植えた！");
            }
        }
    }

    // 🌾 収穫
    void TryHarvest()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, rayDistance))
        {
            if (hit.collider.CompareTag("Field"))
            {
                GameObject field = hit.collider.gameObject;

                Crop crop = field.GetComponentInChildren<Crop>();

                if (crop != null)
                {
                    if (crop.IsGrown())
                    {
                        Inventory.instance.AddItem("米"); // ←ここ追加
                        Destroy(crop.gameObject);
                        Debug.Log("収穫した！");
                    }
                    else
                    {
                        Debug.Log("まだ育ってない");
                    }
                }
                else
                {
                    Debug.Log("何も植えてない");
                }
            }
        }
    }

    // 🌟 マスを光らせる
    void HighlightField()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, Vector3.down, out hit, rayDistance))
        {
            if (hit.collider.CompareTag("Field"))
            {
                GameObject field = hit.collider.gameObject;

                if (currentField != field)
                {
                    ResetHighlight();

                    currentField = field;
                    currentRenderer = field.GetComponent<Renderer>();

                    if (currentRenderer != null)
                    {
                        originalColor = currentRenderer.material.color;

                        // 🌱 状態で色変更
                        Crop crop = field.GetComponentInChildren<Crop>();

                        if (crop == null)
                        {
                            currentRenderer.material.color = canPlantColor; // 緑
                        }
                        else if (crop.IsGrown())
                        {
                            currentRenderer.material.color = Color.yellow; // 収穫OK
                        }
                        else
                        {
                            currentRenderer.material.color = cannotPlantColor; // 赤
                        }
                    }
                }
            }
            else
            {
                ResetHighlight();
            }
        }
        else
        {
            ResetHighlight();
        }
    }

    // 🌟 色を元に戻す
    void ResetHighlight()
    {
        if (currentRenderer != null)
        {
            currentRenderer.material.color = originalColor;
        }

        currentField = null;
        currentRenderer = null;
    }
}