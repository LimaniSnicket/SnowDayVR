using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class Snowfall : MonoBehaviour
{
    private static Snowfall snowfall;
    public static ParticleSystem snowFallParticle;
    public GameObject DirectionalLight;

    public static List<SnowballBehavior> activeSnowballs;

    public float TimeRemaining;
    [Header("X Rotation of the directional light:")]
    public float StartingDirectionalLightAngle, FinalDirectionalLightAngle;
    public delegate void OnTimeOut();
    public static event OnTimeOut GameTimerDiminished;

    public static float RateOfIncrease;

    private void Awake()
    {
        if (snowfall == null) { snowfall = this; } else { Destroy(this); }
        activeSnowballs = new List<SnowballBehavior>();
        TimeRemaining = 250f;
        snowFallParticle = GetComponentInChildren<ParticleSystem>();
        Debug.Log("Obtained reference to Snowfall Particle System");
        RateOfIncrease = 0.1f;
        activeSnowballs.Capacity = 1;
    }

    private void Update()
    {
        TimeRemaining -= Time.deltaTime;
        if (Mathf.Approximately(TimeRemaining, 0))
        {
            GameTimerDiminished();
        }
        DirectionalLight.transform.Rotate(Vector3.right * Time.deltaTime * DirectionalLightIncrement());
    }

    private float DirectionalLightIncrement()
    {
        return Mathf.Abs((FinalDirectionalLightAngle - StartingDirectionalLightAngle)/250f);
    }

    public static bool Snowing()
    {
        return snowfall.TimeRemaining > 0;
    }

    public static SnowballBehavior ActiveSnowball()
    {
        if(activeSnowballs == null) { return null; }
        return activeSnowballs[0];
    }

}
