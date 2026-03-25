using UnityEngine;

public class Campfire : MonoBehaviour
{
    public float interactDistance = 2f;

    [Header("ハイライト")]
    public Color highlightColor = Color.yellow;

    private Renderer rend;
    private Color originalColor;
    private Transform player;

    void Start()
    {
        rend = GetComponentInChildren<Renderer>();
        rend.material = new Material(rend.material);
        originalColor = rend.material.color;

        GameObject p = GameObject.FindWithTag("Player");
        if (p != null)
        {
            player = p.transform;
        }
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        // ⭐ 距離＋アイテムチェック
        bool isNear = distance <= interactDistance;
        bool hasRice = Inventory.instance.GetItemCount("Rice") > 0;

        bool canCook = isNear && hasRice;

        if (canCook)
        {
            rend.material.color = highlightColor;

            // ⭐ 絶対ここに入れる！！
            if (Input.GetKeyDown(KeyCode.F))
            {
                TryCook();
            }
        }
        else
        {
            rend.material.color = originalColor;
        }
    }

    void TryCook()
    {
        if (Inventory.instance.UseItem("Rice", 1))
        {
            Inventory.instance.AddItem("CookedRice");
            Debug.Log("ご飯を作った！");
        }
    }
}
