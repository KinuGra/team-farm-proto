using UnityEngine;

public class PlantManager : MonoBehaviour
{
    public GameObject kome0Prefab; // 植えるPrefab
    public float plantHeight = 0.5f; // 植える高さ

    private bool isInField = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Field"))
        {
            isInField = true;
            Debug.Log("[畑に入った]");
        }
    }
    
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Field"))
        {
            isInField = false;
            Debug.Log("[畑から出た]");
        }
    }
    
    void Update()
    {
        if (isInField && Input.GetKeyDown(KeyCode.F))
        {
            // プレイヤーの足元にkome0を生成
            Vector3 plantPos = transform.position + transform.forward * 1f;
            plantPos.y = plantHeight;

            Instantiate(kome0Prefab, plantPos, Quaternion.identity);
            Debug.Log("[米を植えた] 位置： " + plantPos);
        }
    }
}