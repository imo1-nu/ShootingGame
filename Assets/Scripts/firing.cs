using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class firing : MonoBehaviour
{
    [Header("発射設定")]
    public GameObject bulletPrefab; // Inspector から設定する弾のプレハブ
    public float bulletSpeed = 10f; // 弾の初速度
    public float fireRate = 0.5f; // 発射間隔（秒）
    public KeyCode fireKey = KeyCode.Space; // 発射キー（キーボード用）
    public bool useJoycon = true; // Joyconを使用するかどうか
    
    public enum TeamColor { Red, Blue }
    public TeamColor selectedColor = TeamColor.Red; // Inspector で選択可能な色

    private float nextFireTime = 0f; // 次に発射可能な時間
    private List<Joycon> joycons; // Joyconリスト
    private JoyconDemo joyconDemo; // JoyconDemoスクリプトの参照
    private int jc_ind = -1; // 使用するJoyconのインデックス

    void Start()
    {
        // 同じオブジェクトのJoyconDemoスクリプトを取得
        joyconDemo = GetComponent<JoyconDemo>();
        if (joyconDemo == null)
        {
            Debug.LogWarning("JoyconDemoスクリプトが見つかりません。キーボード入力に切り替えます。");
            useJoycon = false;
        }

        // Joyconを使用する場合、JoyconManagerからJoyconリストを取得
        if (useJoycon)
        {
            joycons = JoyconManager.Instance.j;
            if (joycons.Count < GetJoyconIndex() + 1)
            {
                Debug.LogWarning("指定されたインデックスのJoyconが見つかりません。キーボード入力に切り替えます。");
                useJoycon = false;
            }
            else
            {
                jc_ind = GetJoyconIndex();
            }
        }

        SetObjectColor(gameObject, selectedColor);
    }

    void Update()
    {
        bool shouldFire = false;
        Joycon j = joycons[jc_ind];
        // SHOULDER_1ボタンが押された時に発射
        if (j.GetButtonDown(Joycon.Button.SHOULDER_2))
        {
            shouldFire = true;
        }

        // キーボード入力をチェック（バックアップまたはJoycon無効時）
        if (!useJoycon && Input.GetKeyDown(fireKey))
        {
            shouldFire = true;
        }

        // 発射条件が満たされ、発射間隔が経過している場合
        if (shouldFire && Time.time >= nextFireTime)
        {
            j.SetRumble(200f, 400f, 0.4f, 150);
            FireBullet();
            nextFireTime = Time.time + fireRate;
        }
    }

    // JoyconDemoスクリプトからjc_indを取得するヘルパーメソッド
    private int GetJoyconIndex()
    {
        return joyconDemo != null ? joyconDemo.jc_ind : 0;
    }

    void FireBullet()
    {
        if (bulletPrefab == null)
        {
            Debug.LogWarning("弾のプレハブが設定されていません！");
            return;
        }

        // オブジェクトの下側（ローカル座標でのy負方向）の位置を計算
        Vector3 firePosition = transform.position + transform.TransformDirection(Vector3.down);

        // 弾を生成
        GameObject bullet = Instantiate(bulletPrefab, firePosition, transform.rotation);
        SetObjectColor(bullet, selectedColor);
        bullet.GetComponent<bullet>().jc_ind = jc_ind;

        // 弾にRigidbodyがあるかチェック
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            // オブジェクトの回転に応じた下方向に初速度を与える
            Vector3 fireDirection = transform.TransformDirection(Vector3.down);
            rb.velocity = fireDirection * bulletSpeed;
        }
        else
        {
            Debug.LogWarning("弾のプレハブにRigidbodyコンポーネントがありません！");
        }

        // 弾を一定時間後に自動削除（メモリリーク防止）
        Destroy(bullet, 5f);
    }
    
    private void SetObjectColor(GameObject obj, TeamColor color)
    {
        // オブジェクトのRendererコンポーネントを取得
        Renderer renderer = obj.GetComponent<Renderer>();
        
        if (renderer != null)
        {
            // 色に応じてマテリアルの色を変更
            Color targetColor = color == TeamColor.Red ? Color.red : Color.blue;
            renderer.material.color = targetColor;
        }
        
        // 子オブジェクトも含めて色を変更
        Renderer[] childRenderers = obj.GetComponentsInChildren<Renderer>();
        foreach (Renderer childRenderer in childRenderers)
        {
            Color targetColor = color == TeamColor.Red ? Color.red : Color.blue;
            childRenderer.material.color = targetColor;
        }
    }
}
