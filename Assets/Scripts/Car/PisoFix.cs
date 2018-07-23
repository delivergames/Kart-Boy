using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PisoFix : MonoBehaviour {
    SimpleCarController sc;
	// Use this for initialization
	void Start () {
		sc = GetComponentInParent<SimpleCarController>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Piso")
        {
            sc.tocoPiso = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Piso")
        {
            sc.tocoPiso = false;
        }
    }

}
