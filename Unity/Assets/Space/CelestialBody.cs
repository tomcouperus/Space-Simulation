using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CelestialBody : MonoBehaviour {
    Rigidbody rb;

    [Header("Orbit")]
    [SerializeField]
    Transform _orbitFocus;
    public Transform orbitFocus { get => _orbitFocus; }

    [Tooltip("In e6 km")]
    [SerializeField]
    float orbitRadius;
    [SerializeField]
    [Range(-180, 180)]
    float _angleInOrbit;
    public float angleInOrbit { get => _angleInOrbit; }

    [Header("Planet")]
    [Tooltip("In e24 kg")]
    [SerializeField]
    float mass;
    [Tooltip("In e4 km")]
    [SerializeField]
    float radius;

    [Header("Simulation")]
    public bool isStationary;

    //TODO make this into editor read-only thing
    [Header("Info")]
    [SerializeField]
    Vector3 velocity;

    
    [SerializeField]
    float velocityMagnitude;

    void Awake() {
        rb = GetComponent<Rigidbody>();
        rb.mass = mass;
        rb.useGravity = false;
    }

    /// <summary>
    /// Sets the initial velocity for the CelestialObject (m1) relative to all other objects (m2) in the star system, 
    /// by using the formula:
    /// v = sqrt(G * m2 / r)
    /// </summary>
    public void SetInitialVelocity() {
        velocity = Vector3.zero;
        foreach (CelestialBody other in SpaceSimulation.current.celestialBodies) {
            if (this == other) continue;

            float m2 = other.rb.mass;
            float r = Vector3.Distance(transform.position, other.transform.position);

            float magnitudeV = Mathf.Sqrt(SpaceSimulation.G * m2 / r);
            Vector3 directionV = (other.transform.position - transform.position).normalized;
            directionV = Quaternion.AngleAxis(90, transform.up) * directionV;

            velocity += magnitudeV * directionV;
        }
        velocityMagnitude = velocity.magnitude;
    }

    /// <summary>
    /// Updates the velocity of the body by applying the gravity caused by all other CelestialObjects in the simulation
    /// </summary>
    public void UpdateVelocity() {
        foreach (CelestialBody other in SpaceSimulation.current.celestialBodies) {
            if (this == other) continue;

            float m2 = other.rb.mass;
            float sqrDistance = (other.transform.position - transform.position).sqrMagnitude;

            Vector3 dirAcceleration = (other.transform.position - transform.position).normalized;
            Vector3 acceleration = dirAcceleration * SpaceSimulation.G * m2 / sqrDistance;

            velocity += acceleration * SpaceSimulation.current.timeStep;
            velocityMagnitude = velocity.magnitude;
        }
    }

    public void UpdatePosition() {
        rb.MovePosition(rb.position + velocity * SpaceSimulation.current.timeStep);

        float dstX = rb.position.x - _orbitFocus.position.x;
        float dstZ = rb.position.z - _orbitFocus.position.z;
        _angleInOrbit = Mathf.Rad2Deg * Mathf.Atan2(dstZ, dstX);
    }

    public void SetInitialPositionCircular() {
        if (_orbitFocus == null) return;
        float angle = _angleInOrbit * Mathf.Deg2Rad;
        float x = Mathf.Cos(angle) * orbitRadius;
        float z = Mathf.Sin(angle) * orbitRadius;
        transform.position = _orbitFocus.transform.position + new Vector3(x, 0, z);
    }

#if UNITY_EDITOR
    private void OnValidate() {
        transform.localScale = Vector3.one * radius;
        SetInitialPositionCircular();
    }
#endif
}
