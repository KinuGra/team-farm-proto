using UnityEngine;

public class DynamicObjectTracker : MonoBehaviour
{
    public GameObject prefab;

    // ゲーム内のどこから生成する場合も、Instantiate の代わりにこれを使う
    public static GameObject Spawn(GameObject prefabAsset, Vector3 position, Quaternion rotation)
    {
        GameObject instance = Instantiate(prefabAsset, position, rotation);
        
        // Unityが (Clone) に書き換えてしまった参照を、元のプレハブアセットに強制的に戻す
        if (instance.TryGetComponent<DynamicObjectTracker>(out var tracker))
        {
            tracker.prefab = prefabAsset;
        }

        return instance;
    }
}