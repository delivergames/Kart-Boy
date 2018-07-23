using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamaManager : MonoBehaviour {
    [HideInInspector]
    public CAR[] cars;

    Text textoMedio;

    [HideInInspector]
    public string ganador;

    public int cantidadDeVueltas;
	// Use this for initialization
	void Start () {
        cars = GameObject.FindObjectsOfType<CAR>();
        textoMedio = GameObject.Find("TextoMedio").GetComponent<Text>();
        StartCoroutine("StartRace");
	}

    IEnumerator StartRace()
    {

        //apago los motores
        estadoCarros(cars, false);
        //cuenta regresiva
        for (int i = 3; i > 0; i--)
        {
            textoMedio.text = i.ToString();
            yield return new WaitForSeconds(1);
        }
        //texto de ya
        textoMedio.text = "YA!";
        yield return new WaitForSeconds(1);
        textoMedio.text = "";
        //activo los motores
        estadoCarros(cars, true);

        yield break;
    }

    void estadoCarros(CAR[] carros, bool estado)
    {
        foreach (CAR car in carros)
        {
            estadoCarro(car, estado);
        }
  

    }

    void estadoCarro(CAR carro, bool estado)
    {
		carro.GetComponent<Rigidbody> ().isKinematic = !estado;
    }

    void ApagoMotor(WheelDrive carro)
    {
        carro.Detener(carro.GetComponent<Rigidbody>());
    }

    public void informoVuelta(CAR car)
    {
        if (car.vueltas > cantidadDeVueltas)
        {
            ApagoMotor(car.GetComponent<WheelDrive>());
            if (ganador == "")
            {
                ganador = car.transform.name;
            }
        }
        car.GetComponent<Shooter>().balasGun++;
        car.GetComponent<Shooter>().balasMine++;
    }
}