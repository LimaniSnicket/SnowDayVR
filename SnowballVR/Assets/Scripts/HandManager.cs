using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HandManager : MonoBehaviour
{
    private static HandManager vrHands;
    public static OVRGrabber LeftHand, RightHand;
    public GameObject SnowballPrefab;
    private static Dictionary<bool, OVRGrabber> ovrPairs;

    private void Awake()
    {
        if (vrHands == null) { vrHands = this; } else { Destroy(this); }
        ovrPairs.Add(true, LeftHand); ovrPairs.Add(false, RightHand);
    }

    public static OVRGrabber GetOVRGrabber(bool isLeft)
    {
        return ovrPairs[isLeft];
    }

    public static Vector3 HandPosition(bool isLeft)
    {
        OVRGrabber g = ovrPairs[isLeft];
        if(g == null) { return Vector3.zero; }
        return g.transform.position;
    }
}
