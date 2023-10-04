using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TimeController))]
public class TimeController_Inspector : Editor {
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        TimeController timeController = (TimeController) target;

        if (Application.isPlaying) {
            if (GUILayout.Button("Play")) {
                timeController.Play();
            }
            if (GUILayout.Button("Pause")) {
                timeController.Pause();
            }
        }
    }
}
