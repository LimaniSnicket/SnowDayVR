using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowballSpawner : MonoBehaviour
{
    public GameObject SnowballPrefab;
    public Vector3 spawnPosition;

    private void Update()
    {
        spawnPosition = transform.position + GetComponent<MeshFilter>().mesh.bounds.max;
        if (Input.GetKeyDown(KeyCode.A))
        {
            Instantiate(SnowballPrefab, spawnPosition, new Quaternion(0, 0, 0, 0));
        }
    }


}
