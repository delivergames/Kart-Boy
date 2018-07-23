using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dot : MonoBehaviour {
    public Vector3 margenMin;
    public Vector3 margenMax;
    [Range(0, 2)]
    public float frenadaMin;
    [Range(0, 2)]
    public float frenadaMax;

    // Use this for initialization
    void Start () {
		
	}
	
	public Vector3 getRandomPos()
    {
        return transform.position + new Vector3(Random.Range(margenMin.x, margenMax.x), Random.Range(margenMin.y, margenMax.y), Random.Range(margenMin.z, margenMax.z));
    }

    public float frenada()
    {
        return Random.Range(frenadaMin,frenadaMax);
    }
}
