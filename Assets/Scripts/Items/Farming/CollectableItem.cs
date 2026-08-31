using UnityEngine;

/// <summary>
/// フィールドに配置されているアイテムを表す
/// プレイヤーが拾うとインベントリに追加される
/// </summary>
public class CollectableItem : MonoBehaviour
{
    [SerializeField] private ItemData itemData;
    [SerializeField] private int quantity = 1;

    public ItemData ItemData => itemData;
    public int Quantity => quantity;

    private void OnValidate()
    {
        if (quantity < 1) quantity = 1;
    }

    /// <summary>
    /// このアイテムを集める
    /// </summary>
    public bool Collect(InventoryManager inventory)
    {
        if (itemData == null || inventory == null) return false;

        // 全て追加できるか判定
        if (!inventory.CanAddItem(itemData, quantity)) return false;

        // 全て追加できる場合のみ追加してオブジェクト削除
        inventory.AddItem(itemData, quantity);

        OnCollected();
        Destroy(gameObject);
        return true;
    }

    /// <summary>
    /// 収集時の特殊処理
    /// </summary>
    protected virtual void OnCollected()
    {
        
    }

}