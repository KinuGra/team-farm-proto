using UnityEngine;

public class throwItem : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private ItemData throwItemData;
    [SerializeField] private GameObject throwItemPrefab;
    [SerializeField] private float frontVelocity = 10.0f;
    [SerializeField] private float upVelocity = 10.0f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
         if (Input.GetKeyDown(KeyCode.F))
        {   
            if(InventoryManager.instance != null && throwItemData != null && throwItemPrefab != null)
            {
                ThrowItem();
            }
        }
    }

    void ThrowItem()
    {
        if(InventoryManager.instance.CanRemoveItem(throwItemData))
        {
            Debug.Log("アイテムを投げた: " + throwItemData.name);
            Vector3 pos = transform.position + transform.forward * 2.0f + transform.up*2.0f;
            GameObject throwItem = DynamicObjectTracker.Spawn(throwItemPrefab, pos, Quaternion.identity);
            Rigidbody rb = throwItem.GetComponent<Rigidbody>();
            rb.linearVelocity = transform.forward * frontVelocity + transform.up * upVelocity; // 投げる方向と速度を設定
            InventoryManager.instance.RemoveItem(throwItemData);
        }
        else
        {
            Debug.Log("インベントリにアイテムがない: " + throwItemData.name);
        }
    }
}
