using UnityEngine;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// 全アイテムを一元管理するシングルトン
/// ItemDataをID、名前で検索できる
/// </summary>
public class ItemDatabase : MonoBehaviour
{
    public static ItemDatabase instance;

    [SerializeField] private List<ItemData> items = new List<ItemData>();

    private Dictionary<int, ItemData> itemsById = new Dictionary<int, ItemData>();
    private Dictionary<string, ItemData> itemsByName = new Dictionary<string, ItemData>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeDatabase();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// データベースを初期化（IDと名前でインデックスを作成）
    /// </summary>
    private void InitializeDatabase()
    {
        itemsById.Clear();
        itemsByName.Clear();

        foreach (var item in items)
        {
            if (item == null) continue;

            // IDでインデックス
            if (!itemsById.ContainsKey(item.ItemId))
            {
                itemsById[item.ItemId] = item;
            }
            else
            {
                Debug.LogWarning($"重複したItemId: {item.ItemId}");
            }

            // 名前でインデックス
            if (!itemsByName.ContainsKey(item.ItemName))
            {
                itemsByName[item.ItemName] = item;
            }
            else
            {
                Debug.LogWarning($"重複したアイテム名: {item.ItemName}");
            }
        }

        Debug.Log($"ItemDatabase初期化完了: {items.Count}個のアイテムを読み込み");
    }

    /// <summary>
    /// IDでアイテムを取得
    /// </summary>
    public ItemData GetItemById(int id)
    {
        if (itemsById.TryGetValue(id, out ItemData item))
        {
            return item;
        }
        Debug.LogWarning($"ItemId {id} が見つかりません");
        return null;
    }

    /// <summary>
    /// 名前でアイテムを取得
    /// </summary>
    public ItemData GetItemByName(string name)
    {
        if (itemsByName.TryGetValue(name, out ItemData item))
        {
            return item;
        }
        Debug.LogWarning($"アイテム名 '{name}' が見つかりません");
        return null;
    }

    /// <summary>
    /// すべてのアイテムを取得
    /// </summary>
    public IReadOnlyList<ItemData> GetAllItems()
    {
        return items.AsReadOnly();
    }

    /// <summary>
    /// アイテム総数
    /// </summary>
    public int ItemCount => items.Count;

#if UNITY_EDITOR
    /// <summary>
    /// エディタで自動的にScriptableObjectを検出して登録する
    /// </summary>
    [ContextMenu("Auto-Detect Items")]
    public void AutoDetectItems()
    {
        items.Clear();
        
        // Assets/Scripts/Items/ 配下のすべてのItemDataを検索
       
        List<string> guids = new List<string>(UnityEditor.AssetDatabase.FindAssets("t:ItemData"));
        foreach (string guid in guids)
        {
            string path = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);
            ItemData item = UnityEditor.AssetDatabase.LoadAssetAtPath<ItemData>(path);
            if (item != null && !items.Contains(item))
            {
                items.Add(item);
            }
        }

        InitializeDatabase();
        Debug.Log($"自動検出完了: {items.Count}個のItemDataが登録されました");
    }
#endif
}
