using UnityEngine;

/// <summary>
/// 釣り時間を計算する責務。
/// </summary>
public interface IFishingTimeCalculator
{
    float Calculate(FishingSpotData spotData);
}

public sealed class FishingTimeCalculator : IFishingTimeCalculator
{
    public float Calculate(FishingSpotData spotData)
    {
        if (spotData == null)
            return 0.1f;

        float mean = Mathf.Max(0.1f, spotData.FishingTimeMean);
        float variance = Mathf.Max(0f, spotData.FishingTimeVariance);
        float standardDeviation = Mathf.Sqrt(variance);

        if (standardDeviation <= 0f)
            return mean;

        float u1 = Random.Range(0.000001f, 1f);
        float u2 = Random.Range(0.000001f, 1f);
        float offset = standardDeviation * Mathf.Sqrt(-2f * Mathf.Log(u1)) * Mathf.Cos(2f * Mathf.PI * u2);
        return Mathf.Max(0.1f, mean + offset);
    }
}

/// <summary>
/// 釣りスポットから釣果を選択する責務。
/// </summary>
public interface IFishSelector
{
    FishData Select(FishingSpotData spotData);
}

public sealed class WeightedFishSelector : IFishSelector
{
    public FishData Select(FishingSpotData spotData)
    {
        if (spotData == null || spotData.LocalFish == null)
            return null;

        int totalWeight = 0;
        foreach (LocalFishData localFish in spotData.LocalFish)
        {
            if (IsSelectable(localFish))
                totalWeight += localFish.spawnWeight;
        }

        if (totalWeight <= 0)
            return null;

        int randomWeight = Random.Range(1, totalWeight + 1);
        foreach (LocalFishData localFish in spotData.LocalFish)
        {
            if (!IsSelectable(localFish))
                continue;

            randomWeight -= localFish.spawnWeight;
            if (randomWeight <= 0)
                return localFish.fishData;
        }

        return null;
    }

    private static bool IsSelectable(LocalFishData localFish)
    {
        return localFish != null && localFish.fishData != null && localFish.catchRate > 0 && localFish.spawnWeight > 0;
    }
}
