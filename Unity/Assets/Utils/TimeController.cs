using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TimeController : MonoBehaviour {
    public static TimeController current;

    public List<float> timeScales = new List<float> { 1 };

    [SerializeField]
    int _timeScaleIndex;
    // The timescale of the game when it is actually playing. Only sets when Time.timeScale is not 0.
    public int timeScaleIndex {
        get => _timeScaleIndex;
        set {
            _timeScaleIndex = Mathf.Clamp(value, 0, timeScales.Count - 1);
            if (Time.timeScale == 0) return;
            Play();
        }
    }

    private void Awake() {
        if (current != null) {
            Destroy(this);
            return;
        }
        current = this;
        timeScaleIndex = 0;
    }

    public void Play() {
        Time.timeScale = timeScales[timeScaleIndex];
    }

    public void Pause() {
        Time.timeScale = 0;
    }

#if UNITY_EDITOR
    private void OnValidate() {
        timeScaleIndex = _timeScaleIndex;
    }
#endif
}
