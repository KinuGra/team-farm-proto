using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 釣り堀
/// </summary>
public class FishingSpot : MonoBehaviour
{
    [SerializeField] private FishingSpotData spotData;

    public FishingSpotData SpotData => spotData;

    /// 釣り堀にPlayerが入った時にFishingManagerに通知する
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player") || spotData == null)
            return;
        Debug.Log($"Playerが{spotData.SpotName}に入った");
        if (FishingManager.instance != null)
            FishingManager.instance.SetCurrentSpot(this);
    }

    /// <summary>
    /// 釣り堀からPlayerが出た時にFishingManagerに通知する
    /// </summary>
    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (FishingManager.instance != null)
            FishingManager.instance.ClearCurrentSpot(this);
    }


}
