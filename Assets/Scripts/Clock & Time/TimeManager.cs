using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager instance;

    public float dayLength = 1200f; // 20分 = 1200秒
    private float currentTime = 0f;

    void Awake()
    {
        instance = this;
    }

    void Update()
    {
        currentTime += Time.deltaTime;

        if (currentTime >= dayLength)
        {
            currentTime = 0f;
        }
    }

    public float GetTimeRate()
    {
        return currentTime / dayLength;
    }

    public string GetTimeText()
    {
        float totalMinutes = GetTimeRate() * 24f * 60f;

        int hour = (int)(totalMinutes / 60f);
        int minute = (int)(totalMinutes % 60f);

        return string.Format("{0:00}:{1:00}", hour, minute);
    }
}
