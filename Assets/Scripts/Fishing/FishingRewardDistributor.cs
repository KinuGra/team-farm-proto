using UnityEngine;

/// <summary>
/// アイテムを受け取る側の最小契約。
/// </summary>
public interface IItemReceiver
{
    bool CanReceive(ItemData itemData, int quantity);
    bool Receive(ItemData itemData, int quantity);
}

/// <summary>
/// 釣果をインベントリへ渡す責務。
/// </summary>
public sealed class FishingRewardDistributor
{
    private readonly IItemReceiver receiver;

    public FishingRewardDistributor(IItemReceiver receiver)
    {
        this.receiver = receiver;
    }

    public bool Distribute(FishData fishData, int quantity = 1)
    {
        if (receiver == null || fishData == null || fishData.Item == null || quantity <= 0)
            return false;

        if (!receiver.CanReceive(fishData.Item, quantity))
            return false;

        return receiver.Receive(fishData.Item, quantity);
    }
}
