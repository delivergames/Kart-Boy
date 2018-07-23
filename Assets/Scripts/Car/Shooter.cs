using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    public int nitroRestante;
    public GameObject gun;
    public byte balasGun=1;
    public GameObject mine;
    public byte balasMine=1;
    public float force;
    public float lifeTime;
    public float forceInit;
    Vector3 lugarDisparo;
    float velocidadAuto;
    Rigidbody rb;
    public float nitroPower;
    public float nitroTime;

    public void MissileShoot(Transform auto){
        Rigidbody rb = auto.GetComponent<Rigidbody>();
        if (balasGun >0)  StartCoroutine(ShootAnimation(auto,rb.velocity.magnitude));
    }
    IEnumerator  ShootAnimation(Transform transformCar, float velocidadAuto){
        GameObject newBala = Instantiate(gun, transformCar.position + (transform.forward * 3.35f), transformCar.rotation);
        lugarDisparo = transformCar.forward;
        newBala.GetComponent<Rigidbody>().AddForce(lugarDisparo * (  velocidadAuto + forceInit),ForceMode.Impulse);
        balasGun--;
        yield return new WaitForSeconds(1);
        newBala.GetComponent<Rigidbody>().AddForce(lugarDisparo * force,ForceMode.Impulse);
        StartCoroutine(DestruirMisil(newBala));
       
    }
    public void mineSpawn(Transform transformCar ){
        if (balasMine > 0)
        {
            GameObject newMine = Instantiate(mine, new Vector3(transform.position.x, transform.position.y +0.02f, transform.position.z) + (transform.forward *-3.6f), new Quaternion(0, 0, 0, 0));
            newMine.transform.eulerAngles = new Vector3(-90, 0, 0);
            balasMine--;
        }
    }

   IEnumerator DestruirMisil(GameObject gm)
    {
        yield return new WaitForSeconds(lifeTime);
        if (gm != null ) Destroy(gm);
        yield break;
    }
    public void NitroImpulse(Transform car,float torqueInit ){
        if (nitroRestante > 0)
        {
            nitroRestante -= 1;
            StartCoroutine(NitroTimer(car, torqueInit));
            
        }
    }
    IEnumerator NitroTimer(Transform auto,float torqueInit) {
        int nitroInicial= nitroRestante;
        auto.GetComponent<WheelDrive>().maxTorque +=nitroPower;
        nitroRestante = 0;
        yield return new WaitForSeconds(nitroTime);
        auto.GetComponent<WheelDrive>().maxTorque = torqueInit;
        nitroRestante = nitroInicial;
        yield break;


    }
}
