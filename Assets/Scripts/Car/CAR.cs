using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CAR : MonoBehaviour {
   
    GamaManager gm;
    [HideInInspector]
    public int vueltas = 1;
    [HideInInspector]
    public int cp = 0;
    [HideInInspector]
    public string lastCpName;
    public byte health = 100;
    [HideInInspector]
   public  Transform lastDot;
    Rigidbody rb;
    // Use this for initialization
    void Start () {
        gm = GameObject.FindObjectOfType<GamaManager>();
        rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        AtascoFixer();
	}

    void OnTriggerEnter(Collider collida)
    {
        if (collida.transform.tag == "CP")
        {
            if (collida.transform.name != lastCpName)
            {
                cp++;
                lastCpName = collida.transform.name;
            }
            
        }
        else if (collida.transform.tag == "Meta"){
            if (cp == 2)
            {
                cp = 0;
                lastCpName = "";
                vueltas++;
                gm.informoVuelta(this);
            }
        }
     
    }
    float atascoTime;
    void AtascoFixer()
    {
        if (rb.velocity.magnitude < 0.8f)
        {
            atascoTime +=  Time.deltaTime;
        }
        else
        {
            atascoTime = 0;
        }
        if (atascoTime >= 3)
        {
            if (lastDot != null)
            {
                transform.position = lastDot.position;
                transform.rotation = lastDot.rotation;
      
                Debug.Log("VOLTIE CARRO");
            }
        }
    }
}
