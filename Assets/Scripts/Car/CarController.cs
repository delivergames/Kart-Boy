using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour {
	SimpleCarController wd;
    Shooter shoots;
    Vector3 lugarDisparo;
    float velocidadAuto;
    Rigidbody rb;
    Vector3 impulse;
    CAR car;
  

    // Use this for initialization
    void Start () {
		wd = GetComponent<SimpleCarController> ();
        shoots = GetComponent<Shooter>();
        rb = GetComponent<Rigidbody>();
        impulse =new Vector3(0, 10, 1);
        car = GetComponent<CAR>();
    }
	
	// Update is called once per frame
	void Update () {
        //wd.SetAxes(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        if (Input.GetKeyDown(KeyCode.Space)) {
            shoots.MissileShoot(transform);
        }
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            shoots.mineSpawn(transform);
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            shoots.NitroImpulse(transform,GetComponent<WheelDrive>().maxTorque);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "RoadDot" && car.lastDot != other.transform)
        {
            car.lastDot = other.transform;
        }
    }

}

