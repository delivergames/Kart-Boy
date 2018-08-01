using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour {

    Car car;

    private void Awake() {
        car = GetComponent<Car>();
    }
	
	void Update () {

        if (Input.GetKey(KeyCode.X)) {
            car.Accelerate();
        }

        if (Input.GetKey(KeyCode.C)) {
            car.Brake();
        }

        car.Steer(Input.GetAxisRaw("Horizontal"));

    }
}
