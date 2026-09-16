using UnityEngine;

/// <summary>
/// 簡単なキャンプファイヤーの例 
/// レシピを基に調理をして、アイテムを生成する
/// 調理後にメッセージを表示する
/// </summary>
public class Campfire : Workstation
{
    // 調理後の処理
    protected override void OnTransactionExecuted(Transaction transaction)
    {
        base.OnTransactionExecuted(transaction);
        
        // 調理後のメッセージ
        Debug.Log($"調理成功: {transaction.TransactionName} - {transaction.GetResultDescription()}");
    }

    protected override void OnTransactionUnavailable()
    {
        base.OnTransactionUnavailable();
        
        // 調理できない場合のメッセージ
        Debug.Log($"調理可能なレシピがありません");
    }
}
