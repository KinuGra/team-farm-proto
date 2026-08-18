using UnityEngine;

public class Crop : MonoBehaviour
{
    public GameObject[] growthStages;
    public float growTime = 5f;

    private float timer = 0f;
    private int stage = 0;

    void Start()
    {
        stage = 0;
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

    // 👇 追加
    public bool IsGrown()
    {
        return stage >= growthStages.Length - 1;
    }
}
