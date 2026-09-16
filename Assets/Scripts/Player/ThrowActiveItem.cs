using UnityEngine;

public class throwActiveItem : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is create
    private GameObject throwItemPrefab;
    [SerializeField] private ItemSlots itemSlots;
    [SerializeField] private float frontVelocity = 10.0f;
    [SerializeField] private float upVelocity = 10.0f;

    // Update is called once per frame
    void Update()
    {
         if (Input.GetKeyDown(KeyCode.Q))
        {   
            ItemData throwItemData = itemSlots.GetActiveItemData();
            if(InventoryManager.instance != null && throwItemData != null)
            {
                ThrowItem(throwItemData);
            }
        }
    }

    void ThrowItem(ItemData throwItemData)
    {
        if(InventoryManager.instance.CanRemoveItem(throwItemData))
        {
            Debug.Log("アイテムを投げた: " + throwItemData.name);
            GameObject throwItemPrefab = throwItemData.Model;
            if(throwItemPrefab != null)
            {
                
                Vector3 pos = transform.position + transform.forward * 2.0f + transform.up*2.0f;
                Quaternion rot = transform.rotation;
                GameObject throwItem = DynamicObjectTracker.Spawn(throwItemPrefab, pos, rot);
                Rigidbody rb = throwItem.GetComponent<Rigidbody>();
                rb.linearVelocity = transform.forward * frontVelocity + transform.up * upVelocity; // 投げる方向と速度を設定
            }
            else
            {
                Debug.Log("モデルが指定されていません");
            }
            InventoryManager.instance.RemoveItem(throwItemData);
        }
        else
        {
            Debug.Log("インベントリにアイテムがない: " + throwItemData.name);
        }
    }
}
