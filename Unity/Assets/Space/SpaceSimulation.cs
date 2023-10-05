using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[RequireComponent(typeof(TimeController))]
public class SpaceSimulation : MonoBehaviour {
    public static SpaceSimulation current;
    public static float G = 100f;
    [Header("Data")]
    [SerializeField]
    private string simulationName;

    private string filePath {
        get {
            string safeName = simulationName
                .Replace(' ', '_')
                .Replace(Path.DirectorySeparatorChar, '_')
                .Replace(Path.AltDirectorySeparatorChar, '_')
                .Replace('\\', '_')
                .Replace(':', '_');
#if UNITY_EDITOR
            string path = Application.dataPath + Path.DirectorySeparatorChar + ".." + Path.DirectorySeparatorChar + "..";
#else
            string path = Application.persistentDataPath;
#endif
            return path + Path.DirectorySeparatorChar + "Data" + Path.DirectorySeparatorChar + safeName + ".json";
        }
    }

    private StreamWriter streamWriter;


    [Header("Simulation")]
    [SerializeField]
    private float runUntil = 10;
    public float timeStep = 0.02f;

    [SerializeField]
    private float time;

    public bool isStarted { get; private set; }
    private bool isInitialized;
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
        isInitialized = false;
        isStarted = false;
    }

    public void StartSimulation() {
        if (!isInitialized) {
            Debug.Log("Initialization not finished. Please wait.");
            return;
        }
        isStarted = true;
        TimeController.current.Play();
    }

    private void Start() {
        InitializeSimulation();
        InitializeDataLog();
        isInitialized = true;
    }

    private void OnDestroy() {
        FinalizeDataLog();
    }

    private void InitializeDataLog() {
        Debug.Log("Initializing Data Log...");
        Debug.Log("... Checking Data Directory");
        Directory.CreateDirectory(Path.GetDirectoryName(filePath));

        Debug.Log("... Creating File at: " + filePath);
        streamWriter = new StreamWriter(File.Create(filePath));
        Debug.Log("... Writing Initial Conditions");
        streamWriter.Write('[');
        streamWriter.Write(JsonUtility.ToJson(ToData()));
        Debug.Log("Initialized Data Log...");
    }

    private void FinalizeDataLog() {
        Debug.Log("Finalizing Data Log...");
        Debug.Log("... Writing Closing Character");
        streamWriter.Write(']');
        Debug.Log("... Closing File");
        streamWriter.Close();
        streamWriter = null;
        Debug.Log("Finalized Data Log...");
    }

    private void InitializeSimulation() {
        Debug.Log("Initializing Space Simulation...");
        TimeController.current.Pause();
        Debug.Log("... Setting Positions");
        SetInitialPositionsCircular();
        Debug.Log("... Calculating Velocities");
        SetInitialVelocities();
        Debug.Log("... Applying Offsets");
        ApplyInitialOffsets();
        Debug.Log("... Visualizing Lagrange Points");
        SetLagrangePointPositions();
        Debug.Log("Initialized Space Simulation");
    }

    private void FixedUpdate() {
        if (!isStarted) return;
        UpdateVelocities();
        UpdatePositions();
        SetLagrangePointPositions();
        time += timeStep;

        streamWriter.Write(',');
        streamWriter.Write(JsonUtility.ToJson(ToData()));

        if (time >= runUntil) TimeController.current.Pause();
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

    public SpaceSimulationData ToData() {
        List<CelestialBodyData> celestialBodyData = new();
        foreach (CelestialBody cb in celestialBodies) {
            celestialBodyData.Add(cb.ToData());
        }
        List<LagrangePointData> lagrangePointData = new();
        foreach (LagrangePoint lp in lagrangePoints) {
            lagrangePointData.Add(lp.ToData());
        }

        SpaceSimulationData data = new() {
            time = time,
            celestialBodyData = celestialBodyData.ToArray(),
            lagrangePointData = lagrangePointData.ToArray()
        };
        return data;
    }

}

[System.Serializable]
public struct SpaceSimulationData {
    public float time;
    public CelestialBodyData[] celestialBodyData;
    public LagrangePointData[] lagrangePointData;
}