using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TCoup.Toolbox.Space {
    [RequireComponent(typeof(TimeController))]
    public class SpaceSimulation : MonoBehaviour {
        public static SpaceSimulation current;
        public static float G = 100f;

        public bool isInitialized { get; private set; }
        public CelestialBody[] celestialBodies;

        private void Awake() {
            if (current != null) {
                Destroy(this);
                return;
            }
            current = this;
            celestialBodies = FindObjectsOfType<CelestialBody>();
            isInitialized = false;
        }

        private void Start() {
            Debug.Log("Initializing");
            TimeController.current.Pause();
            foreach (CelestialBody cb in celestialBodies) {
                cb.SetInitialPositionCircular();
            }
            foreach (CelestialBody cb in celestialBodies) {
                if (cb.isStationary) continue;
                cb.SetInitialVelocity();
            }
            isInitialized = true;
            Debug.Log("Initialized Space Simulation");
        }

        private void FixedUpdate() {
            if (!isInitialized) return;
            foreach (CelestialBody cb in celestialBodies) {
                if (cb.isStationary) continue;
                cb.ApplyGravity();
            }
        }
    }
}