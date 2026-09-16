using UnityEngine;

/// <summary>
/// プレイヤーのインベントリを管理する（シングルトン）
/// </summary>
/// 
public class InventoryManager : ItemContainer
{
    // シングルトン
    public static InventoryManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
