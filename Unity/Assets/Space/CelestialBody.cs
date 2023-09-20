using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CelestialBody : MonoBehaviour
{

    Rigidbody rb;

    [Header("Orbit")]
    [SerializeField]
    CelestialBody orbitFocus;
    [SerializeField]
    float apoapsis;
    [SerializeField]
    float periapsis;
    [SerializeField]
    [Tooltip("// TODO")]
    float eccentricity;

    float semiMajorAxis { get => (apoapsis + periapsis) / 2; }
    float semiMinorAxis { get => semiMajorAxis * Mathf.Sqrt(1 - eccentricity * eccentricity); }

    [SerializeField]
    [Range(0, 180)]
    [Tooltip("// TODO")]
    float inclination;

    [SerializeField]
    [Range(-180, 180)]
    [Tooltip("Place where the orbit ascends through its plane of reference, with respect to the positive x-axis.")]
    float longitudeAscendingNode;

    [Header("Planet")]
    [SerializeField]
    private float mass;
    [SerializeField]
    private float radius;

    [Header("Simulation")]
    public bool isStationary;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    /// <summary>
    /// Sets the initial velocity for the CelestialObject (m1) relative to all other objects (m2) in the star system, 
    /// by using the formula:
    /// v = sqrt(G * m2 / r)
    /// </summary>
    public void SetInitialVelocity()
    {
        foreach (CelestialBody other in SpaceSimulation.current.celestialBodies)
        {
            if (this == other) continue;

            float m2 = other.rb.mass;
            float r = Vector3.Distance(transform.position, other.transform.position);

            float magnitudeV = Mathf.Sqrt(SpaceSimulation.G * m2 / r);
            Vector3 directionV = (other.transform.position - transform.position).normalized;
            directionV = Quaternion.AngleAxis(90, transform.up) * directionV;
            Vector3 velocity = magnitudeV * directionV;

            rb.velocity += velocity;
        }
    }

    /// <summary>
    /// Applies the standard physics formula for gravity:
    /// Fg = G * m1 * m2 / r^2
    /// to each CelestialObject in the system
    /// </summary>
    public void ApplyGravity()
    {
        foreach (CelestialBody other in SpaceSimulation.current.celestialBodies)
        {
            if (this == other) continue;

            float m1 = rb.mass;
            float m2 = other.rb.mass;
            float sqrDistance = (other.transform.position - transform.position).sqrMagnitude;

            Vector3 directionFg = (other.transform.position - transform.position).normalized;
            float magnitudeFg = SpaceSimulation.G * m1 * m2 / sqrDistance;
            Vector3 Fg = magnitudeFg * directionFg;

            rb.AddForce(Fg);
        }
    }

    public void SetInitialPositionCircular()
    {
        if (orbitFocus == null) return;
        float loanRad = longitudeAscendingNode * Mathf.Deg2Rad;
        float x = Mathf.Cos(loanRad) * periapsis;
        float z = Mathf.Sin(loanRad) * periapsis;
        transform.position = orbitFocus.transform.position + new Vector3(x, 0, z);
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (rb != null) rb.mass = mass;
        transform.localScale = Vector3.one * radius;
        SetInitialPositionCircular();
    }
#endif
}
