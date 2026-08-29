using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 釣り堀の魚の情報
/// </summary>
[System.Serializable]
public class LocalFishData
{
    public FishData fishData;
    [Range(0, 100)]
    public int catchRate = 100; // 釣れる確率（0〜100）
    public int spawnWeight = 1; // 釣れる魚の出現比重 
}

/// <summary>
/// 釣り堀の情報
/// </summary>
[CreateAssetMenu(menuName = "Fishing/FishSpot")]
public class FishingSpotData : ScriptableObject
{
    [SerializeField] private int spotid; // 釣り堀のID

    [SerializeField] private string spotName; // 釣り堀の名前
    [SerializeField] private List<LocalFishData> localFish; // 釣り堀で釣れる魚の情報
    [SerializeField] private float fishingTimeMean = 5f; // 平均釣り時間（秒）
    [SerializeField] private float fishingTimeVariance = 2f; // 釣り時間の分散（秒）

    [SerializeField] private Vector3 direction;


    public int SpotId => spotid;
    public string SpotName => spotName;
    public List<LocalFishData> LocalFish => localFish;
    public Vector3 Direction => direction;
    public float FishingTimeMean => fishingTimeMean;
    public float FishingTimeVariance => fishingTimeVariance;

    private void OnValidate()
    {
        if (localFish == null)
            return;

        foreach (var fish in localFish)
        {
            if (fish == null)
                continue;

            fish.catchRate = Mathf.Clamp(fish.catchRate, 0, 100);
            fish.spawnWeight = Mathf.Max(1, fish.spawnWeight);
        }
    }
}
