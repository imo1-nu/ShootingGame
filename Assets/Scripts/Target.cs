using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField] private int scoreValue = 100; // このターゲットのスコア値
    
    // ターゲットが破壊された時のスコア値を取得
    public int GetScoreValue()
    {
        return scoreValue;
    }
    
    // ターゲットが破壊された時の処理
    public void OnDestroyed()
    {
        // エフェクトやサウンド再生などの処理
        Destroy(gameObject);
    }
}