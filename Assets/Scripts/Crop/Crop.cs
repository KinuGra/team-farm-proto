using UnityEngine;

public class Crop : MonoBehaviour
{
    public GameObject[] growthStages; // 成長段階（見た目）
    public float growTime = 5f; // 次の段階までの時間

    private float timer = 0f;
    private int stage = 0;

    void Start()
    {
        UpdateStage();
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= growTime)
        {
            timer = 0f;

            if (stage < growthStages.Length - 1)
            {
                stage++;
                UpdateStage();
            }
        }
    }

    void UpdateStage()
    {
        for (int i = 0; i < growthStages.Length; i++)
        {
            growthStages[i].SetActive(i == stage);
        }
    }
}
