using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private int[] scores = new int[2]; // 0: Player1, 1: Player2
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    // スコアを加算するメソッド
    public void AddScore(int playerIndex, int score)
    {
        if (playerIndex < 0 || playerIndex >= scores.Length)
        {
            Debug.LogError("Invalid player index: " + playerIndex);
            return;
        }
        scores[playerIndex] += score;
        Debug.Log("Player " + playerIndex + " score: " + scores[playerIndex]);
    }
}
