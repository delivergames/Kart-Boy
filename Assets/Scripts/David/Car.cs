using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Car : MonoBehaviour {

    [Header("Settings")]

    [Tooltip("How much the car accelerates over time.")]
    public float acceleration = 15f;

    [Tooltip("The maximum speed the car can reach by accelerating.")]
    public float maxSpeed = 15f;

    [Tooltip("How fast the car brakes.")]
    public float brakeForce = 20f;

    [Tooltip("Useless right now.")]
    public float friction = 3f;

    [Tooltip("Steering speed when the car is at minimum speed.")]
    public float minSteeringSpeed = 4f;

    [Tooltip("Steering speed when the car is at maximum speed.")]
    public float maxSteeringSpeed = 2f;



    [Header("Structure")]
    public GameObject wheelPrefab;
    public Vector3 steeringAxisPosition;
    public float steeringAxisLength;
    public Vector3 backAxisPosition;
    public float backAxisLength;

    public LayerMask groundLayers;
    public LayerMask wallLayers;

    public bool grounded = false;
    public bool skidding = false;

    public float forwardSpeed;
    public Vector3 velocity;
    public Vector3 steeringDirection;

    BoxCollider collider;

    GameObject frontLeftWheel;
    GameObject frontRightWheel;
    GameObject backLeftWheel;
    GameObject backRightWheel;

    Mesh wheelMesh;

    public Vector3 pivot;

	void Awake () {
        collider = GetComponent<BoxCollider>();

        //Spawn wheels
        frontRightWheel = Instantiate(wheelPrefab, GetWheelPosition(Wheel.FrontRight), transform.rotation, transform);
        frontLeftWheel = Instantiate(wheelPrefab, GetWheelPosition(Wheel.FrontLeft), transform.rotation, transform);
        backRightWheel = Instantiate(wheelPrefab, GetWheelPosition(Wheel.BackRight), transform.rotation, transform);
        backLeftWheel = Instantiate(wheelPrefab, GetWheelPosition(Wheel.BackLeft), transform.rotation, transform);

    }

    void Update() {

        //if(velocity.magnitude > 0f)
        //transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(steeringDirection), 70f * Time.deltaTime);


        //Manage side collision
        RaycastHit sideHit;
        Physics.BoxCast(collider.bounds.center, collider.size * 0.5f,
        velocity.normalized, out sideHit, transform.rotation, (velocity * Time.deltaTime).magnitude, wallLayers);
        if (sideHit.collider != null) {
            Debug.Log("Side collision");
            velocity = Vector3.zero;
        }

        //Apply gravity
        if (!grounded) {
            velocity += Vector3.down * Physics.gravity.magnitude * Time.deltaTime;
        }

        //Apply speed
        if(grounded && !skidding) {

            if(steeringDirection == transform.forward) {
                transform.position += transform.forward * forwardSpeed * Time.deltaTime;
            } else {
                float steeringAngle = Vector3.SignedAngle(transform.forward, steeringDirection, transform.up);

                Plane backPlane = new Plane(backLeftWheel.transform.forward, transform.position + transform.rotation * backAxisPosition);
                float planeHit; 

                //Steer right
                if (steeringAngle > 0f) {
                    backPlane.Raycast(new Ray(frontRightWheel.transform.position, frontRightWheel.transform.right), out planeHit);
                    pivot = frontRightWheel.transform.position + frontRightWheel.transform.right * planeHit;
                    
                    //Rotate around pivot
                    float perimeter = Vector3.Distance(backRightWheel.transform.position, pivot) * 2f * Mathf.PI;
                    float anglesPerUnit = 360f / perimeter;
                    transform.RotateAround(pivot, transform.up, forwardSpeed * anglesPerUnit * Time.deltaTime);

                } 
                
                //Steer left
                else {
                    backPlane.Raycast(new Ray(frontLeftWheel.transform.position, -frontLeftWheel.transform.right), out planeHit);
                    pivot = frontLeftWheel.transform.position - frontLeftWheel.transform.right * planeHit;

                    //Rotate around pivot
                    float perimeter = Vector3.Distance(backLeftWheel.transform.position, pivot) * 2f * Mathf.PI;
                    float anglesPerUnit = 360f / perimeter;
                    transform.RotateAround(pivot, transform.up, -(forwardSpeed * anglesPerUnit * Time.deltaTime));
                }
            }

        } else if (grounded) {
            //velocity = Vector3.MoveTowards(velocity, Vector3.zero, friction * Time.deltaTime);
        } else {
            //transform.position = transform.position + velocity * Time.deltaTime;
        }

        //Overcome floor
        RaycastHit hit;
        bool didHit = Physics.BoxCast(collider.bounds.center + Vector3.up * collider.size.y * 2f, collider.size * 0.5f,
        Vector3.down, out hit, transform.rotation, collider.bounds.size.y * 4f, groundLayers);

        if (didHit && hit.point.y >= collider.bounds.min.y) {
            transform.position = new Vector3(transform.position.x, hit.point.y, transform.position.z);
            grounded = true;

        } else if (didHit && hit.point.y >= collider.bounds.min.y - 0.1f) {
            grounded = true;
        } else {
            grounded = false;
        }

        

    }

    #region Methods for Controller

    public void Accelerate() {

        if (grounded && forwardSpeed < maxSpeed) {
            forwardSpeed += acceleration * Time.deltaTime;
        }
        
    }

    public void Brake() {
        if (grounded) {
            forwardSpeed = Mathf.MoveTowards(forwardSpeed, 0f, brakeForce * Time.deltaTime);
        }
    }

    public void Steer(float input) {
        float n = Mathf.InverseLerp(-1f, 1f, input);
        float steeringSpeed = Mathf.Lerp(minSteeringSpeed, maxSteeringSpeed, Mathf.InverseLerp(0f, maxSpeed, forwardSpeed));
        Debug.Log(steeringSpeed);

        Vector3 targetSteeringDirection = Quaternion.Lerp(Quaternion.Euler(0f, -30f, 0f), Quaternion.Euler(0f, 30f, 0f), n) * transform.forward;
        steeringDirection = Vector3.MoveTowards(steeringDirection, targetSteeringDirection, steeringSpeed * Time.deltaTime).normalized;
        frontLeftWheel.transform.rotation = Quaternion.LookRotation(steeringDirection);
        frontRightWheel.transform.rotation = Quaternion.LookRotation(steeringDirection);
    }

    #endregion

    Vector3 GetWheelPosition(Wheel wheel) {
        switch (wheel) {
            case Wheel.FrontRight:
                return transform.position + transform.rotation * steeringAxisPosition + transform.right * steeringAxisLength * 0.5f;
            case Wheel.FrontLeft:
                return transform.position + transform.rotation * steeringAxisPosition - transform.right * steeringAxisLength * 0.5f;
            case Wheel.BackRight:
                return transform.position + transform.rotation * backAxisPosition + transform.right * backAxisLength * 0.5f;
            case Wheel.BackLeft:
                return transform.position + transform.rotation * backAxisPosition - transform.right * backAxisLength * 0.5f;
            default:
                return new Vector3();
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position + (transform.rotation * steeringAxisPosition), 0.1f);
        Gizmos.DrawRay(transform.position + (transform.rotation * steeringAxisPosition), steeringDirection);

        if(wheelMesh == null) {
            //Get Wheel Mesh
            wheelMesh = wheelPrefab.GetComponent<MeshFilter>().sharedMesh;
        }

        Gizmos.DrawMesh(wheelMesh, GetWheelPosition(Wheel.FrontRight), transform.rotation);
        Gizmos.DrawMesh(wheelMesh, GetWheelPosition(Wheel.FrontLeft), transform.rotation);
        Gizmos.DrawMesh(wheelMesh, GetWheelPosition(Wheel.BackRight), transform.rotation);
        Gizmos.DrawMesh(wheelMesh, GetWheelPosition(Wheel.BackLeft), transform.rotation);

        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(pivot, 0.3f);
    }

    public enum Wheel { FrontRight, FrontLeft, BackRight, BackLeft}

}
