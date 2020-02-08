using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    static ScoreManager scoreManager;
    public static int CurrentScore;

    private void Awake()
    {
        if (scoreManager == null) { scoreManager = this; } else { Destroy(this); }
        SnowballBehavior.SnowballHit += UpdateScore;
    }

    private void Update()
    {
        if (CurrentScore > PlayerPrefs.GetInt("Highscore", 0))
        {
            PlayerPrefs.SetInt("Highscore", CurrentScore);
        }
    }

    void UpdateScore(float snowballSize, float carModifier)
    {
        CurrentScore += Mathf.FloorToInt(snowballSize * carModifier);
    }

    private void OnDestroy()
    {
        SnowballBehavior.SnowballHit -= UpdateScore;
    }
}
