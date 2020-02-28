using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class SnowMoundBehavior : MonoBehaviour
{
    private static SnowMoundBehavior snowMound;
    public float ScaleModifier;
    public static float availableSnow;
    public static bool canSpawnSnowball { get => Snowfall.activeSnowballs.Count == 0; }
    public float SnowLeft;
    public float MaxSize = 400;
    static List<Collider> CollidersInTrigger = new List<Collider>();
    public static event Action<bool> OnHandMotionDetected;
    public static event Action<GameObject> HandToSpawn;

    public void Start()
    {
        Snowfall.GameTimerDiminished += OnGameTimerDiminished;
        transform.localScale = new Vector3(100, 100, 0);
        availableSnow = 1;
        if (snowMound == null) { snowMound = this; } else { Destroy(this); }
    }

    private void Update()
    {
        SnowLeft = availableSnow;
        availableSnow += Time.deltaTime * Snowfall.RateOfIncrease;
        Vector3 s = transform.localScale + (Vector3.forward * availableSnow / 10);
        transform.localScale = s;
    }

    private void OnGameTimerDiminished()
    {
        availableSnow = 0f;
        //Destroy(GetComponent<Collider>());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!CollidersInTrigger.Contains(other) && ValidGameObject(other))
        {
            CollidersInTrigger.Add(other);
        }
        if (CollidersInTrigger.Count == 2)
        {
            Debug.Log("Here");
            OnHandMotionDetected(true);
            HandToSpawn(CollidersInTrigger[0].gameObject);
        }
    }

    bool ValidGameObject(Collider c)
    {
        return c.gameObject == HandManager.RightHand.gameObject || c.gameObject == HandManager.LeftHand.gameObject || c.tag =="Test";
    }

    private void OnTriggerExit(Collider other)
    {
        if (CollidersInTrigger.Contains(other))
        {
            CollidersInTrigger.Remove(other);
        }
        OnHandMotionDetected(false);
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
