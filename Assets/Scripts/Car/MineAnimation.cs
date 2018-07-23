using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineAnimation : MonoBehaviour {
    public float impulse=10f;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    void OnTriggerEnter(Collider collision)
    {

        if (collision.transform.GetComponentInParent<Shooter>() != null){
            Debug.Log("PUMM");
            collision.GetComponentInParent<Rigidbody>().AddForce(Vector3.up * impulse, ForceMode.Impulse);
            collision.GetComponentInParent<Rigidbody>().AddTorque(new Vector3(0, Random.Range(1.8f,2), Random.Range(0.8f, 1)) * impulse,ForceMode.Impulse);
            collision.GetComponentInParent<CAR>().health--;
            if (gameObject != null)
            Destroy(gameObject);

        }
    }
}
