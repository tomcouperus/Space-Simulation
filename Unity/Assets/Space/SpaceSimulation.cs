using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TimeController))]
public class SpaceSimulation : MonoBehaviour {
    public static SpaceSimulation current;
    public static float G = 100f;

    public float timeStep = 0.02f;
    public bool isStarted { get; private set; }
    public CelestialBody[] celestialBodies;
    public LagrangePoint[] lagrangePoints;

    private void Awake() {
        if (current != null) {
            Destroy(this);
            return;
        }
        current = this;
        celestialBodies = FindObjectsOfType<CelestialBody>();
        lagrangePoints = FindObjectsOfType<LagrangePoint>();
        isStarted = false;
    }

    private void Start() {
        Debug.Log("Initializing Space Simulation...");
        TimeController.current.Pause();
        Debug.Log("... Setting Positions");
        SetInitialPositionsCircular();
        Debug.Log("... Calculating Velocities");
        SetInitialVelocities();
        Debug.Log("... Applying Offsets");
        ApplyInitialOffsets();
        Debug.Log("... Lagrange Points");
        SetLagrangePointPositions();
        Debug.Log("Initialized Space Simulation");
    }

    private void ApplyInitialOffsets() {
        foreach (CelestialBody cb in celestialBodies) {
            if (!cb.gameObject.activeSelf) continue;
            cb.SetInitialOffsets();
        }
    }

    private void SetInitialPositionsCircular() {
        foreach (CelestialBody cb in celestialBodies) {
            if (!cb.gameObject.activeSelf) continue;
            cb.SetInitialPositionCircular();
        }
    }

    private void SetInitialVelocities() {
        foreach (CelestialBody cb in celestialBodies) {
            if (!cb.gameObject.activeSelf) continue;
            cb.SetInitialVelocity();
        }
    }

    private void SetLagrangePointPositions() {
        foreach (LagrangePoint lp in lagrangePoints) {
            if (!lp.gameObject.activeSelf) continue;
            lp.SetPosition();
        }
    }

    private void UpdateVelocities() {
        foreach (CelestialBody cb in celestialBodies) {
            if (!cb.gameObject.activeSelf) continue;
            cb.UpdateVelocity();
        }
    }

    private void UpdatePositions() {
        foreach (CelestialBody cb in celestialBodies) {
            if (!cb.gameObject.activeSelf) continue;
            cb.UpdatePosition();
        }
    }

    public void StartSimulation() {
        isStarted = true;
        TimeController.current.Play();
    }

    private void FixedUpdate() {
        if (!isStarted) return;
        UpdateVelocities();
        UpdatePositions();
        SetLagrangePointPositions();
    }

}