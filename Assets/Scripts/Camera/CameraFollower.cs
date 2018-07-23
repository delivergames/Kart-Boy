using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollower : MonoBehaviour {
    public Transform target;
    public Vector3 offset;
    Vector3 difference;
    public Vector3[] angulos;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        offset = Vector3.MoveTowards(offset, smoothAngles(), 0.1f);
        transform.position = (new Vector3(target.position.x + offset.x, transform.position.y + offset.y, target.position.z + offset.z))   ;
	}

    Vector3 smoothAngles()
    {
        float angle = target.rotation.eulerAngles.y;
        if (between(angle,0,45))
        {
            //Debug.Log("ACTIVE 0");
            return angulos[0];
            
        }else if (between(angle, 46, 135))
        {
            //Debug.Log("ACTIVE 1");
            return angulos[1];
        }else if (between(angle, 136, 225))
        {
           // Debug.Log("ACTIVE 2");
            return angulos[2];
        }
        else if(between(angle, 315, 360))
        {
            //Debug.Log("ACTIVE 0.1");
            return angulos[0];
        }
        else 
        {
           // Debug.Log("ACTIVE 3 " + angle);
            return angulos[3];
        }

    }

    bool between(float value,float min,float max)
    {
        return value >= min && value <= max;
    }
}
