using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SpaceSimulation))]
public class SpaceSimulation_Inspector : Editor {
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        SpaceSimulation spaceSimulation = (SpaceSimulation) target;

        if (Application.isPlaying) {
            if (GUILayout.Button("Start")) {
                spaceSimulation.StartSimulation();
            }
        }
    }
}
