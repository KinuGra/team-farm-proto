using UnityEngine;

public class CookingTable : MonoBehaviour
{
    public float interactDistance = 3f;

    private Transform player;

    void Start()
    {
        GameObject p = GameObject.FindWithTag("Player");
        if (p != null)
        {
            player = p.transform;
            Debug.Log("プレイヤー見つけた");
        }
        else
        {
            Debug.LogError("Playerタグが見つからない！");
        }
    }

    void Update()
    {
        if (player == null) return;

        float dist = Vector3.Distance(transform.position, player.position);

        // 距離チェックログ
        Debug.Log("距離: " + dist);

        if (dist <= interactDistance)
        {
            Debug.Log("範囲内にいる");

            if (Input.GetKeyDown(KeyCode.F))
            {
                Debug.Log("F押された！");
                TryCraftOnigiri();
            }
        }
    }

    void TryCraftOnigiri()
    {
        Debug.Log("調理処理スタート");

        bool hasRice = Inventory.instance.GetItemCount("CookedRice") > 0;
        bool hasWakame = Inventory.instance.GetItemCount("CookedWakame") > 0;

        Debug.Log("CookedRice: " + hasRice);
        Debug.Log("CookedWakame: " + hasWakame);

        if (hasRice && hasWakame)
        {
            Inventory.instance.UseItem("CookedRice", 1);
            Inventory.instance.UseItem("CookedWakame", 1);

            Inventory.instance.AddItem("Onigiri", 1);

            Debug.Log("🍙 おにぎりを作った！");
        }
        else
        {
            Debug.Log("❌ 材料が足りない！");
        }
    }
}
