using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    static ScoreManager scoreManager;
    public static int CurrentScore;
    public TextMeshPro HighScoreTextMesh;

    public int Score;

    private void Awake()
    {
        if (scoreManager == null) { scoreManager = this; } else { Destroy(this); }
       VehicleBehavior.SnowballHit += UpdateScore;
    }

    private void Update()
    {
        Score = CurrentScore;
        HighScoreTextMesh.text = "Score: " + CurrentScore + '\n' + "Best: " + PlayerPrefs.GetInt("Highscore");
    }

    void UpdateScore(float snowballSize, int carModifier)
    {
        CurrentScore += Mathf.FloorToInt(snowballSize * carModifier);
        if (CurrentScore > PlayerPrefs.GetInt("Highscore", 0))
        {
            PlayerPrefs.SetInt("Highscore", CurrentScore);
        }
    }

    private void OnDestroy()
    {
        VehicleBehavior.SnowballHit -= UpdateScore;
    }
}
