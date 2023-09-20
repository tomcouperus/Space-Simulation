using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TCoup.Toolbox {
    public class TimeController : MonoBehaviour {
        public static TimeController current;

        [SerializeField]
        [Range(0, 1)]
        private float slowSpeed = 0.5f;

        [SerializeField]
        [Range(1, 100)]
        private float fastSpeed = 2;

        private void Awake() {
            if (current != null) {
                Destroy(this);
                return;
            }
            current = this;
        }

        public void Play() {
            Time.timeScale = 1;
        }

        public void PlaySlow() {
            Time.timeScale = slowSpeed;
        }

        public void PlayFast() {
            Time.timeScale = fastSpeed;
        }

        public void Pause() {
            Time.timeScale = 0;
        }
    }
}