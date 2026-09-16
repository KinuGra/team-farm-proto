using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DynamicObjectManager : MonoBehaviour
{
    public static DynamicObjectManager Instance { get; private set; }

    // シーン名ごとにデータを保存する辞書
    private Dictionary<string, List<ObjectData>> sceneDataDict = new Dictionary<string, List<ObjectData>>();

    [System.Serializable]
    public struct ObjectData
    {
        public GameObject prefab;
        public Vector3 position;
        public Quaternion rotation;
    }

    private void Awake()
    {
        // 二重生成防止（すでに存在していれば自分を消す）
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // シーン移動しても削除しない
    }

    // SceneManager.sceneLoadedへのシーンロード時の処理登録は最初の一回のみ
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // シーン遷移を呼ぶ直前に外から明示的に呼ぶ保存メソッド
    public void SaveCurrentSceneObjects()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        List<ObjectData> currentList = new List<ObjectData>();

        DynamicObjectTracker[] trackers = FindObjectsByType<DynamicObjectTracker>(FindObjectsSortMode.None);
        foreach (var t in trackers)
        {
            if (t.prefab != null)
            {
                currentList.Add(new ObjectData
                {
                    prefab = t.prefab,
                    position = t.transform.position,
                    rotation = t.transform.rotation
                });
            }
        }

        sceneDataDict[currentScene] = currentList;
        Debug.Log($"[{currentScene}] のデータを保存しました: {currentList.Count}個");
        foreach (var cl in currentList)
        {
            if (cl.prefab != null)
            {
                Debug.Log($"{cl.prefab.name}: {cl.position}, {cl.rotation}");
            }
        }
    }

    // シーンが読み込まれた後に自動で復元する
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (sceneDataDict.TryGetValue(scene.name, out var savedList))
        {
            foreach (var data in savedList)
            {
                if (data.prefab != null)
                {
                    GameObject obj = DynamicObjectTracker.Spawn(data.prefab, data.position, data.rotation);
                    if (obj.TryGetComponent<Rigidbody>(out var rb))
                    {
                        rb.WakeUp();
                    }
                }
            }
            Debug.Log($"[{scene.name}] のデータを復元しました: {savedList.Count}個");
        }
    }
}