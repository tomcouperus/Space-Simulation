using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LagrangePoint : MonoBehaviour {
    enum PointType {
        // TODO L1, L2, L3, 
        L4, L5
    }

    [SerializeField]
    CelestialBody parent;

    [SerializeField]
    PointType type;

    public void SetPosition() {
        switch (type) {
            case PointType.L4:
                SetPositionL4();
                break;
            case PointType.L5:
                SetPositionL5();
                break;
        }
    }

    private void SetPositionL4() {
        float angle = (parent.angleInOrbit + 60) * Mathf.Deg2Rad;
        float dst = (parent.orbitFocus.transform.position - parent.transform.position).magnitude;
        float x = Mathf.Cos(angle) * dst;
        float z = Mathf.Sin(angle) * dst;

        transform.position = parent.orbitFocus.transform.position + new Vector3(x, 0, z);
    }
    private void SetPositionL5() {
        float angle = (parent.angleInOrbit - 60) * Mathf.Deg2Rad;
        float dst = (parent.orbitFocus.transform.position - parent.transform.position).magnitude;
        float x = Mathf.Cos(angle) * dst;
        float z = Mathf.Sin(angle) * dst;

        transform.position = parent.orbitFocus.transform.position + new Vector3(x, 0, z);
    }

    public LagrangePointData ToData() {
        LagrangePointData data = new() {
            name = gameObject.name,
            position = transform.position
        };
        return data;
    }

#if UNITY_EDITOR
    private void OnValidate() {
        SetPosition();
    }
#endif
}

[System.Serializable]
public struct LagrangePointData {
    public string name;
    public Vector3 position;
}
