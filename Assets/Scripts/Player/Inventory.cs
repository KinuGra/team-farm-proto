using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    // シングルトン：どこからでも Inventory.instance でアクセスできる
    public static Inventory instance;
    
    // アイテム名と個数の辞書
    private Dictionary<string, int> items = new Dictionary<string, int>();

    void Awake()
    {
        // シングルトンの判定
        if (instance == null) {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    // アイテムを追加
    public void AddItem(string itemName)
    {
        if (items.ContainsKey(itemName))
        {
            items[itemName]++;
        }
        else
        {
            items[itemName] = 1;
        }
        
        Debug.Log("[インベントリ] " + itemName + " x" + items[itemName]);
    }
    
    // アイテムを消費
    public bool UseItem(string itemName, int amount = 1)
    {
        if (items.ContainsKey(itemName) && items[itemName] >= amount) {
            items[itemName] -= amount;
            
            if (items[itemName] <= 0)
            {
                items.Remove(itemName);
            }
            
            return true;
        }

        return false;
    }
    
    // アイテムの個数を取得
    public int GetItemCount(string itemName)
    {
        if (items.ContainsKey(itemName))
        {
            return items[itemName];
        }
        return 0;
    }
    
    // 全アイテムを取得（UI表示用）
    public Dictionary<string, int> GetAllItems()
    {
        return items;
    }
}