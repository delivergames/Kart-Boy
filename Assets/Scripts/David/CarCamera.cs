using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarCamera : MonoBehaviour {

    public Car target;
    public Vector3 offset;

    BoxCollider carCollider;

    void Awake() {
        carCollider = target.GetComponent<BoxCollider>();
    }
	
	void LateUpdate () {
        transform.position = carCollider.bounds.center + offset;
        transform.LookAt(target.transform, Vector3.up);

    }
}
