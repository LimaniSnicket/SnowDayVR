using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class Snowfall : MonoBehaviour
{
    private static Snowfall snowfall;
    public GameObject snowFallPrefab;

    public static List<SnowballBehavior> activeSnowballs;

    public float TimeRemaining;
    public delegate void OnTimeOut();
    public static event OnTimeOut GameTimerDiminished;

    public static float RateOfIncrease;

    private void Awake()
    {
        if (snowfall == null) { snowfall = this; } else { Destroy(this); }
        activeSnowballs = new List<SnowballBehavior>();
    }

    private void Update()
    {
        TimeRemaining -= Time.deltaTime;
        if (Mathf.Approximately(TimeRemaining, 0))
        {
            GameTimerDiminished();
        }
    }
}
