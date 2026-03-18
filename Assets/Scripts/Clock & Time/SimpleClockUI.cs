using UnityEngine;

public class SimpleClockUI : MonoBehaviour
{
    public RectTransform hand;

    void Update()
    {
        float timeRate = TimeManager.instance.GetTimeRate();

        // 1日で1回転
        float rotation = timeRate * 360f;

        // 上を0時にする
        hand.localRotation = Quaternion.Euler(0, 0, -rotation + 90f);
    }
}
