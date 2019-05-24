using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    public GameObject followCamera;
    public GameObject target;
    Vector3 offset;

    // Start is called before the first frame update
    void Start() {
        if (target != null)
            offset = followCamera.transform.position - target.transform.position;
    }

    // Update is called once per frame
    void Update() {
        if (target != null)
            followCamera.transform.position = target.transform.position + offset;
    }
}
