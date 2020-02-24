using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(OVRGrabbable))]
public class SnowballBehavior : MonoBehaviour
{
    public GameObject HitParticle;
    Rigidbody snowballBody { get => GetComponent<Rigidbody>(); }
    SphereCollider snowballCollider { get => GetComponent<SphereCollider>(); }
    OVRGrabbable snowballGrabbable { get => GetComponent<OVRGrabbable>(); }
    public OVRGrabber followHand;
    float sizeFactor { get => Mathf.Ceil(snowballBody.mass + snowballCollider.radius); }
    public static event Action<Collider, float> SnowballCollision;
    public List<string> IgnoreTags;

    bool ungrabbed = true;

    private void Start()
    {
        if (Snowfall.activeSnowballs != null && !Snowfall.activeSnowballs.Contains(this))
        {
            Snowfall.activeSnowballs.Add(this);
        }
        snowballBody.mass = 0.001f;
        transform.localScale = Vector3.one * 0.001f;
        snowballBody.isKinematic = true;
        IgnoreTags.Add("Untagged");
    }

    float addForce = 0;

    private void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            ScaleUp(Time.deltaTime/5f);
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            Destroy(gameObject);
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            addForce += Time.deltaTime * 1.5f;
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            snowballBody.isKinematic = false;
            snowballBody.AddForce((Vector3.forward + Vector3.up) * addForce, ForceMode.Impulse);
            addForce = 0;
        }

        if (snowballGrabbable.grabbedBy != null)
        {
            ungrabbed = false;
            followHand = null;
        }

        if (followHand != null && ungrabbed)
        {
            transform.position = followHand.transform.position;
            Debug.Log("Still follow hand");
        }

        if (transform.position.y < -200)
        {
            Destroy(gameObject);
        }

       // snowballBody.isKinematic = !ungrabbed;

    }

    public void SetOVRGrabberFollow(OVRGrabber newFollowHand)
    {
        followHand = newFollowHand;
    }

    void Drop()
    {
        followHand = null;
    }

    void ScaleUp()
    {
        snowballBody.mass += Time.deltaTime/10f;
        transform.localScale += Vector3.one * Time.deltaTime/5f;
    }

    public void ScaleUp(float massToAdd)
    {
        if (SnowMoundBehavior.availableSnow >= massToAdd)
        {
            snowballBody.mass += massToAdd;
            transform.localScale += Vector3.one * massToAdd / 2;
            SnowMoundBehavior.availableSnow -= massToAdd / 2;
        }
    }


    void BroadcastSnowballHit(Collider c)
    {
        if (IgnoreTags != null && IgnoreTags.Contains(c.tag))
        {
            Physics.IgnoreCollision(snowballCollider, c);
            return;
        }
        SnowballCollision(c, sizeFactor);
        Debug.Log(c.name + ": " + sizeFactor);
        Destroy(gameObject);
    }

    Queue<Collider> Hits = new Queue<Collider>();
    private void OnCollisionEnter(Collision collision)
    {
        Hits.Enqueue(collision.collider);
        BroadcastSnowballHit(Hits.Peek());
    }

    private void OnDestroy()
    {
        Instantiate(HitParticle, transform.position, new Quaternion(0,0,0,0));
        if (Snowfall.activeSnowballs.Contains(this))
        {
            Snowfall.activeSnowballs.Remove(this);
        }
    }
}
