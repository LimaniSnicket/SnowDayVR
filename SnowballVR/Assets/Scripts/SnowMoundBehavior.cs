﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SnowMoundBehavior : MonoBehaviour
{
    private static SnowMoundBehavior snowMound;
    public static float availableSnow;
    public float SnowLeft;
    static List<Collider> CollidersInTrigger = new List<Collider>();

    public void Start()
    {
        Snowfall.GameTimerDiminished += OnGameTimerDiminished;
        transform.localScale = new Vector3(2, 0, 2);
        StartCoroutine(AccumulateSnow());
        if (snowMound == null) { snowMound = this; } else { Destroy(this); }
    }

    private void Update()
    {
        SnowLeft = availableSnow;
    }

    private IEnumerator AccumulateSnow()
    {
        yield return new WaitForSecondsRealtime(.1f);
        while (Snowfall.Snowing())
        {
            availableSnow += Snowfall.RateOfIncrease;
            Vector3 lerpTo = transform.localScale + Vector3.up * availableSnow/4;
            while(!transform.localScale.Approximately(lerpTo))
            {
                transform.localScale = Vector3.Lerp(transform.localScale, lerpTo, Time.deltaTime);
                yield return null;
            }
            yield return new WaitForSecondsRealtime(1);
        }
        yield return null;
    }

    private void OnGameTimerDiminished()
    {
        availableSnow = 0f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!CollidersInTrigger.Contains(other) && ValidTag(other))
        {
            CollidersInTrigger.Add(other);
        }
        if (CollidersInTrigger.Count == 2)
        {
            StartCoroutine(HandManager.SufficientHandMovement(1, 3));
        }
    }

    bool ValidTag(Collider c)
    {
        return c.tag == "RightHand" || c.tag == "LeftHand";
    }

    private void OnTriggerExit(Collider other)
    {
        if (CollidersInTrigger.Contains(other))
        {
            CollidersInTrigger.Remove(other);
        }
        StopCoroutine(HandManager.SufficientHandMovement(1,3));
    }

    private void OnDestroy()
    {
        Snowfall.GameTimerDiminished -= OnGameTimerDiminished;
    }
}

public static class HelperFunctions
{
    public static bool Approximately(this Vector3 v, Vector3 comp, float threshold = 0.01f)
    {
        return Mathf.Abs(Vector3.Distance(v, comp)) <= threshold;
    }
}
