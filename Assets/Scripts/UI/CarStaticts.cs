using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarStaticts : MonoBehaviour {
    public Rigidbody car;
    public Text speedText;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        speedText.text = ((int)(car.velocity.magnitude * 10f)).ToString();
	}
}
