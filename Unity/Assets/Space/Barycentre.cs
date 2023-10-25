using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barycentre : MonoBehaviour
{
    [SerializeField]
    List<CelestialBody> vertices;


    public void SetPosition()
    {
        Vector3 pos = Vector3.zero;
        float totalMass = 0;
        foreach (CelestialBody cb in vertices)
        {
            pos += cb.transform.position * cb.mass;
            totalMass += cb.mass;
        }
        pos /= totalMass;
        transform.position = pos;
    }

    public BarycentreData ToData()
    {
        BarycentreData data = new()
        {
            name = gameObject.name,
            position = transform.position
        };
        return data;
    }
}

[System.Serializable]
public struct BarycentreData
{
    public string name;
    public Vector3 position;
}
