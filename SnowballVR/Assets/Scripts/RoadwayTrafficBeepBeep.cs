using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RoadwayTrafficBeepBeep : MonoBehaviour
{
    private static RoadwayTrafficBeepBeep beepBeep;
    public static TrafficSpawner LeftLaneTraffic, RightLaneTraffic;
    public GameObject LeftLane, RightLane;
    public GameObject DebugVehicle;

    private void Start()
    {
        if (beepBeep == null) { beepBeep = this; } else { Destroy(beepBeep); }
        LeftLaneTraffic = new TrafficSpawner(LeftLane.transform.GetChild(0).gameObject, LeftLane.transform.GetChild(1).gameObject);
        RightLaneTraffic = new TrafficSpawner(RightLane.transform.GetChild(0).gameObject, RightLane.transform.GetChild(1).gameObject);
    }

    private void Update()
    {
        LeftLaneTraffic.ManageTraffic(DebugVehicle);
        RightLaneTraffic.ManageTraffic(DebugVehicle);
    }

    public static void VehicleDestroyed(VehicleBehavior vb)
    {
        if (LeftLaneTraffic.CarsSpawned.Contains(vb))
        {
            Debug.Log("Destroying a vehicle in the Left Lane");
            LeftLaneTraffic.CarsSpawned.Remove(vb);
        }

        if (RightLaneTraffic.CarsSpawned.Contains(vb))
        {
            Debug.Log("Destroying a vehicle in the Right Lane");
            RightLaneTraffic.CarsSpawned.Remove(vb);
        }
    }
}

[Serializable]
public class TrafficSpawner
{
    public Transform SpawnPosition, Destination;
    public List<VehicleBehavior> CarsSpawned;
    private float TimeTillSpawn, currentSpawnTimer;
    public TrafficSpawner() { SpawnPosition = null; Destination = null; CarsSpawned = new List<VehicleBehavior>(); }
    public TrafficSpawner(GameObject spawnAt, GameObject goTo)
    {
        SpawnPosition = spawnAt.transform;
        Destination = goTo.transform;
        CarsSpawned = new List<VehicleBehavior>();
        TimeTillSpawn = UnityEngine.Random.Range(10f, 15f);
    }

    public void ManageTraffic(GameObject vehiclePrefab)
    {
        currentSpawnTimer += Time.deltaTime;
        if (currentSpawnTimer >= TimeTillSpawn)
        {
            currentSpawnTimer = 0;
            TimeTillSpawn = UnityEngine.Random.Range(10f, 15f);
            VehicleBehavior newVehicle = GameObject.Instantiate(vehiclePrefab).GetComponent<VehicleBehavior>();
            newVehicle.transform.position = SpawnPosition.position;
            newVehicle.transform.forward = SpawnPosition.forward;
            newVehicle.SetDestination(Destination.position);
            CarsSpawned.Add(newVehicle);
        }
    }

    public void DebugTimer()
    {
        Debug.Log(currentSpawnTimer);
    }
}
