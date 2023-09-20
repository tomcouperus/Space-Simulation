using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// https://en.wikipedia.org/wiki/Spherical_coordinate_system
public class SphericalPosition : MonoBehaviour
{
    [Min(0)]
    public float radius;
    [Range(-180, 180)]
    public float inclination;
    [Range(0, 360)]
    public float azimuth;

    public Vector3 ToCartesian()
    {
        float _inclination = inclination * Mathf.Deg2Rad;
        float _azimuth = azimuth * Mathf.Deg2Rad;
        return ToCartesian(radius, _inclination, _azimuth);
    }

    public static Vector3 ToCartesian(float radius, float inclination, float azimuth)
    {
        Vector3 position = Vector3.zero;
        inclination = -inclination + Mathf.PI / 2f;
        position.x = radius * Mathf.Sin(inclination) * Mathf.Cos(azimuth);
        position.z = radius * Mathf.Sin(inclination) * Mathf.Sin(azimuth);
        position.y = radius * Mathf.Cos(inclination);
        return position;
    }

#if UNITY_EDITOR
    protected virtual void OnValidate()
    {
        transform.localPosition = ToCartesian();
    }
#endif
}
