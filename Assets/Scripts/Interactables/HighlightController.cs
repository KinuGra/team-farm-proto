using UnityEngine;

public class HighlightController : MonoBehaviour
{
    [Header("ハイライト設定")]
    public Color highlightColor = Color.yellow;

    private Renderer[] rends;
    private Color[] originalColors;

    void Start()
    {
        // ⭐ 子オブジェクト含め全部のRenderer取得
        rends = GetComponentsInChildren<Renderer>();

        originalColors = new Color[rends.Length];

        for (int i = 0; i < rends.Length; i++)
        {
            // ⭐ マテリアルを複製（超重要）
            rends[i].material = new Material(rends[i].material);

            // 元の色保存
            originalColors[i] = rends[i].material.color;
        }
    }

    public void SetHighlight(bool on)
    {
        for (int i = 0; i < rends.Length; i++)
        {
            if (on)
            {
                rends[i].material.color = highlightColor;

                // ⭐ 発光（オプション：消してもOK）
                rends[i].material.EnableKeyword("_EMISSION");
                rends[i].material.SetColor("_EmissionColor", highlightColor * 2f);
            }
            else
            {
                rends[i].material.color = originalColors[i];

                // 発光OFF
                rends[i].material.SetColor("_EmissionColor", Color.black);
            }
        }
    }
}
