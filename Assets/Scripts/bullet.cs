using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    [Header("弾の設定")]
    public float lifetime = 5f;
    public int jc_ind = -1;
    private ScoreManager scoreManager;

    void Start()
    {
        scoreManager = FindObjectOfType<ScoreManager>();
        if (scoreManager == null)
        {
            Debug.LogError("ScoreManagerが見つかりません。");
        }
        
        // 2つのColliderを設定
        SetupDualColliders();
        
        Destroy(gameObject, lifetime);
    }

    void Update()
    {

    }

    private void SetupDualColliders()
    {
        // 1. 物理衝突用のCollider（Is Trigger = false）
        SphereCollider physicsCollider = gameObject.AddComponent<SphereCollider>();
        physicsCollider.isTrigger = false;
        physicsCollider.radius = 0.5f; // 適切な半径を設定
        // 2. トリガー検知用のCollider（Is Trigger = true）
        SphereCollider triggerCollider = gameObject.AddComponent<SphereCollider>();
        triggerCollider.isTrigger = true;
        triggerCollider.radius = 0.5f; // 物理衝突用と同じ半径を設定
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Target"))
        {
            if (scoreManager != null)
            {
                scoreManager.AddScore(jc_ind, 100);
            }
            
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}