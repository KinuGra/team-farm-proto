using UnityEngine;
using System.Collections.Generic;

public class Campfire : MonoBehaviour
{
    public float interactDistance = 2f;

    [Header("ハイライト")]
    public Color highlightColor = Color.yellow;

    private Renderer rend;
    private Color originalColor;
    private Transform player;

    // ⭐ レシピ（入力 → 出力）
    private Dictionary<string, string> recipes = new Dictionary<string, string>()
    {
        { "Rice", "CookedRice" },
        { "Wakame", "CookedWakame" }
    };

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
        bool isNear = distance <= interactDistance;

        bool hasAnyItem = HasAnyCookableItem();

        bool canCook = isNear && hasAnyItem;

        if (canCook)
        {
            rend.material.color = highlightColor;

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

    bool HasAnyCookableItem()
    {
        foreach (var recipe in recipes)
        {
            if (Inventory.instance.GetItemCount(recipe.Key) > 0)
                return true;
        }
        return false;
    }

    void TryCook()
    {
        foreach (var recipe in recipes)
        {
            string input = recipe.Key;
            string output = recipe.Value;

            if (Inventory.instance.UseItem(input, 1))
            {
                Inventory.instance.AddItem(output);
                Debug.Log(input + " → " + output + " に調理！");
                return; // 1回で1つだけ
            }
        }

        Debug.Log("調理できる素材がない！");
    }
}
