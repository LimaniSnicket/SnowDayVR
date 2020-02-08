using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SphereCollider))]
public class SnowballBehavior : MonoBehaviour
{
    public GameObject HitParticle;
    Rigidbody snowballBody { get => GetComponent<Rigidbody>(); }
    SphereCollider snowballCollider { get => GetComponent<SphereCollider>(); }
    float sizeFactor { get => Mathf.Floor(snowballBody.mass + snowballCollider.radius); }

    private void Start()
    {
        snowballBody.mass = 0.001f;
        transform.localScale = Vector3.one * 0.001f;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            ScaleUp();
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            Destroy(gameObject);
        }
    }

    void ScaleUp()
    {
        snowballBody.mass += Time.deltaTime;
        transform.localScale += Vector3.one * Time.deltaTime;
    }

    private void OnDestroy()
    {
        Instantiate(HitParticle, snowballBody.centerOfMass, new Quaternion(0,0,0,0));
        Debug.Log(sizeFactor);
    }
}
