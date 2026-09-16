using UnityEngine;

/// <summary>
/// 簡単な住民の例 
/// 取引表を基に取引をして、アイテムを生成する
/// 取引後にメッセージを表示する
/// </summary>
public class Resident : Workstation
{
    // 取引後の処理
    protected override void OnTransactionExecuted(Transaction transaction)
    {
        base.OnTransactionExecuted(transaction);
        
        // 取引後のメッセージ
        Debug.Log($"取引完了: {transaction.TransactionName} - {transaction.GetResultDescription()}");
    }

    protected override void OnTransactionUnavailable()
    {
        base.OnTransactionUnavailable();
        
        // 取引できない場合のメッセージ
        Debug.Log($"取引可能なアイテムがありません");
    }
}
