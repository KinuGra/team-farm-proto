using UnityEngine;
using System.Collections;

public class FishingManager : MonoBehaviour
{
    public static FishingManager instance { get; private set; }

    private FishingSpot currentSpot;
    private bool isFishing = false;
    private readonly IFishSelector fishSelector = new WeightedFishSelector();
    private readonly IFishingTimeCalculator fishingTimeCalculator = new FishingTimeCalculator();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetCurrentSpot(FishingSpot spot)
    {
        currentSpot = spot;
    }

    public void ClearCurrentSpot(FishingSpot spot)
    {
        if (currentSpot == spot)
            currentSpot = null;
    }

    public void TryStartFishing()
    {
        if (currentSpot == null)
            return;

        StartFishing(currentSpot);
    }

    private void StartFishing(FishingSpot spot)
    {
        if (isFishing || spot == null || spot.SpotData == null) return;
        isFishing = true;
        Debug.Log($"{spot.SpotData.SpotName}で釣り開始");
        StartCoroutine(WaitForFish(spot));
    }

    private IEnumerator WaitForFish(FishingSpot spot)
    {
        // ✡釣りを途中で抜け出した場合の処理はいつかやる
        float waitTime = fishingTimeCalculator.Calculate(spot.SpotData);
        yield return new WaitForSeconds(waitTime);

        EndFish(spot);
        isFishing = false;
    }

    private void EndFish(FishingSpot spot)
    {
        FishData caughtFish = fishSelector.Select(spot != null ? spot.SpotData : null);
        InventoryManager inventory = InventoryManager.instance;

        if (caughtFish == null)
        {
            Debug.Log("魚は釣れなかった");
            return;
        }

        if (inventory == null)
        {
            Debug.LogWarning("インベントリが見つからないため、釣果を受け取れません");
            return;
        }

        FishingRewardDistributor rewardDistributor = new FishingRewardDistributor(inventory);
        if (rewardDistributor.Distribute(caughtFish))
        {
            Debug.Log($"釣れた魚: {caughtFish.FishName}");
        }
        else
        {
            Debug.Log($"{caughtFish.FishName}を釣ったが、インベントリに空きがありません");
        }
    }
}