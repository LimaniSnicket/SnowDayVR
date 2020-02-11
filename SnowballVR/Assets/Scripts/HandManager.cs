using UnityEngine;
using System.Collections;

public class HandManager : MonoBehaviour
{
    private static HandManager vrHands;
    public static OVRGrabber LeftHand, RightHand;

    private void Awake()
    {
        if (vrHands == null) { vrHands = this; } else { Destroy(this); }
    }

    public static Vector3 HandPosition(bool isLeft)
    {
        OVRGrabber g = isLeft ? LeftHand : RightHand;
        if(g == null) { return Vector3.zero; }
        return g.transform.position;
    }
}
