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

    //TODO make this into editor read-only thing
    [Header("Info")]
    [SerializeField]
    Vector3 velocity;

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
        velocity = Vector3.zero;
        foreach (CelestialBody other in SpaceSimulation.current.celestialBodies)
        {
            if (this == other) continue;

            float m2 = other.rb.mass;
            float r = Vector3.Distance(transform.position, other.transform.position);

            float magnitudeV = Mathf.Sqrt(SpaceSimulation.G * m2 / r);
            Vector3 directionV = (other.transform.position - transform.position).normalized;
            directionV = Quaternion.AngleAxis(90, transform.up) * directionV;

            velocity += magnitudeV * directionV;
        }
    }

    /// <summary>
    /// Updates the velocity of the body by applying the gravity caused by all other CelestialObjects in the simulation
    /// </summary>
    public void UpdateVelocity()
    {
        foreach (CelestialBody other in SpaceSimulation.current.celestialBodies)
        {
            if (this == other) continue;

            float m2 = other.rb.mass;
            float sqrDistance = (other.transform.position - transform.position).sqrMagnitude;

            Vector3 dirAcceleration = (other.transform.position - transform.position).normalized;
            Vector3 acceleration = dirAcceleration * SpaceSimulation.G * m2 / sqrDistance;

            velocity += acceleration * SpaceSimulation.current.timeStep;
        }
    }

    public void UpdatePosition()
    {
        rb.MovePosition(rb.position + velocity * SpaceSimulation.current.timeStep);
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
