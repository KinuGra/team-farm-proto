using UnityEngine;
using System;
/// <summary>
/// フィールドに配置されているアイテムを表す
/// プレイヤーが拾うとインベントリに追加される
/// </summary>
public class CollectWakame : CollectableItem
{
    public System.Action<CollectWakame> OnHarvested;

    protected override void OnCollected()
    {
        base.OnCollected(); 

        OnHarvested?.Invoke(this);

    }

}