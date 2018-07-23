using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace UnityStandardAssets.Vehicles.Car
{

    public class IACar : MonoBehaviour
    {
        CarAIControl wd;
        [HideInInspector]
        public Vector3 dot;
        public Transform front;
        public byte porcentajeDisparo;
        public byte porcentajeMineo;

        GamaManager gm;
        Shooter sh;
        CAR car;

        //Seguimiento
        public GameObject dotContainer;
        BoxCollider[] dots;
        public Vector3[] primerosDotsPos;
        [HideInInspector]
        public int indice;

        float frenos;

        public Transform dota2;

        // Use this for initialization
        void Start()
        {
            car = GetComponent<CAR>();
            wd = GetComponent<CarAIControl>();
            dots = dotContainer.GetComponentsInChildren<BoxCollider>();
            dot = dots[0].transform.position + agregoOffsetSiHay();
            gm = GameObject.FindObjectOfType<GamaManager>();
            sh = GetComponent<Shooter>();
            StartCoroutine("Disparo");
            StartCoroutine("Mineo");
            
        }


        // Update is called once per frame
        void Update()
        {
            dota2 = dots[indice].transform;
            wd.m_Target = dota2;
			wd.offsetTarget = agregoOffsetSiHay ();
            //wd.Move(steerResult(), speedResult() - frenos, speedResult() - frenos, 0f);

            //wd.Move(, speedResult() - frenos);


        }


        void OnTriggerEnter(Collider collida)
        {
            
            if (collida.transform.tag == "RoadDot" && car.lastDot != collida.transform)
            {
                Debug.Log("TRIGGERED");
                car.lastDot = collida.transform;
                aumentoIndice();
            }
        }


        #region auxSeguimiento

        IEnumerator Disparo()
        {
            yield return new WaitForSeconds(1);
            while (true)
            {
				foreach (CAR autop in gm.cars)
                {
                    Transform auto = autop.transform;
                    if (Mathf.Abs(funcionX(auto.position)) < 4f && Vector3.Distance(front.position, auto.position) < 4 && auto.name != transform.name)
                    {
                        if (porcentaje(porcentajeDisparo))
                        {
                            Debug.Log("BANG BANG dispare");
                            sh.MissileShoot(transform);
                        }
                    }
                }
                yield return new WaitForSeconds(0.5f);
            }
        }

        IEnumerator Mineo()
        {
            yield return new WaitForSeconds(13);
            while (true)
            {
                if (porcentaje(porcentajeMineo))
                {
                    Debug.Log("PUUUM tire mina");
                    sh.mineSpawn(transform);
                }
                yield return new WaitForSeconds(10f);
            }
        }
        public void aumentoIndice()
        {
            if (indice < dots.Length - 1)
            {
                indice++;
            }
            else
            {
                indice = 0;
            }
            cambioDot();
        }

        const int offDots = 6;
        void cambioDot()
        {

            dot = dots[indice].GetComponent<dot>().getRandomPos() + agregoOffsetSiHay();
            frenos = dots[indice].GetComponent<dot>().frenada();
        }
        #endregion

        #region aux
        const float angular = 25f;
        float steerResult()
        {

            return Mathf.Clamp(funcionX(dot), -angular, angular) / angular;
            //return   (Mathf.Clamp ( (Vector2.SignedAngle (carAngleFixer(dot.transform.position.y), dot.transform.position)),-wd.maxAngle,wd.maxAngle)   ) / -wd.maxAngle ;
        }

        float speedResult()
        {
            return Mathf.Clamp(1 - Mathf.Abs(steerResult()), 0.3f, 1f);

        }

        Vector2 carAngleFixer(float yPoint)
        {
            return new Vector2(front.transform.position.x, yPoint);
        }


        float funcionX(Vector3 dota)
        {
            Vector3 targetDir = new Vector3(dota.x, front.transform.position.y, dota.z) - front.transform.position;
            Vector3 normalizedDirection = targetDir;
            float destino = Vector3.Cross(transform.forward, normalizedDirection).y;


            return destino * 5.37f;
        }

        Vector3 agregoOffsetSiHay()
        {
            return (indice <= primerosDotsPos.Length - 1) ? primerosDotsPos[indice] : Vector3.zero;
        }

        bool porcentaje(byte probabilidades)
        {
            return (Random.Range(0, 100) <= probabilidades);
        }
        #endregion
    }
}
