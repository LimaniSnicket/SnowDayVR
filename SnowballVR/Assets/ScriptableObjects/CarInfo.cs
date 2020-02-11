using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName= "New Car", menuName ="Car" )]
public class CarInfo : ScriptableObject
{
    public string CarID;
    public int OnHitCarBonus;
    public float MaxSpeed;
    public GameObject CarModel;
    public Texture[] AvailableTextures;
    public float SpawnFrequency;
    public OnHitEffect OnHitDo;

    public VehicleBehavior GenerateCar(Transform spawnAt, bool add = false)
    {
        GameObject g = Instantiate(CarModel);
        g.transform.position = spawnAt.position;
        g.transform.forward = spawnAt.right;
        VehicleBehavior v = add ? g.AddComponent<VehicleBehavior>() : g.GetComponent<VehicleBehavior>(); 
        v.CarInformation = this;
        return v;
    }
}

public enum OnHitEffect
{
    NONE = 0,
    PENALTY = 1
}
