using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Car : MonoBehaviour {

    [Header("Configuration")]
    public CarHandlingSettings settings;
    public CarStructureSettings structure;

    public LayerMask groundLayers;
    public LayerMask wallLayers;

    [Header("Debug")]
    public bool grounded = false;
    public bool skidding = false;

    public float forwardSpeed;
    public float upSpeed;
    public Vector3 velocity;
    public Vector3 steeringDirection;

    BoxCollider collider;

    GameObject flWheel;
    GameObject frWheel;
    GameObject blWheel;
    GameObject brWheel;

    Collider flwCol, frwCol, brwCol, blwCol;

    Mesh wheelMesh;

    public Vector3 pivot;

	void Awake () {
        collider = GetComponent<BoxCollider>();

        //Spawn wheels
        frWheel = Instantiate(structure.wheelPrefab, GetWheelPosition(Wheel.FrontRight), transform.rotation, transform);
        flWheel = Instantiate(structure.wheelPrefab, GetWheelPosition(Wheel.FrontLeft), transform.rotation, transform);
        brWheel = Instantiate(structure.wheelPrefab, GetWheelPosition(Wheel.BackRight), transform.rotation, transform);
        blWheel = Instantiate(structure.wheelPrefab, GetWheelPosition(Wheel.BackLeft), transform.rotation, transform);

        //Add colliders to wheel (for quick bounds)
        frwCol = frWheel.AddComponent<MeshCollider>();
        flwCol = flWheel.AddComponent<MeshCollider>();
        brwCol = brWheel.AddComponent<MeshCollider>();
        blwCol = blWheel.AddComponent<MeshCollider>();

    }

    bool controlledUpdate = false;
    void Update() {

        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            controlledUpdate = true;
        }

        if (Input.GetKeyDown(KeyCode.Alpha2)) {
            controlledUpdate = false;
        }

        if (controlledUpdate == false || (controlledUpdate && Input.GetKeyDown(KeyCode.Alpha1))) {
            ControlledUpdate();
        }

    }

    void ControlledUpdate() {

        //Manage side collision
        RaycastHit sideHit;
        Physics.BoxCast(collider.bounds.center, collider.size * 0.5f,
        velocity.normalized, out sideHit, transform.rotation, (velocity * Time.deltaTime).magnitude, wallLayers);
        if (sideHit.collider != null) {
            Debug.Log("Side collision");
            velocity = Vector3.zero;
        }

        //Apply gravity
        upSpeed = upSpeed - Physics.gravity.magnitude * Time.deltaTime;

        if (grounded && upSpeed <= 0f) {
            upSpeed = -1f;
        }

        //Apply forward speed
        if (grounded && !skidding) {

            if (steeringDirection == transform.forward) {
                transform.position += transform.forward * forwardSpeed * Time.deltaTime;
            } else {
                float steeringAngle = Vector3.SignedAngle(transform.forward, steeringDirection, transform.up);

                Plane backPlane = new Plane(blWheel.transform.forward, transform.position + transform.rotation * structure.backAxisPosition);
                float planeHit;

                //Steer right
                if (steeringAngle > 0f) {
                    backPlane.Raycast(new Ray(frWheel.transform.position, frWheel.transform.right), out planeHit);
                    pivot = frWheel.transform.position + frWheel.transform.right * planeHit;

                    //Rotate around pivot
                    float perimeter = Vector3.Distance(brWheel.transform.position, pivot) * 2f * Mathf.PI;
                    float anglesPerUnit = 360f / perimeter;
                    transform.RotateAround(pivot, transform.up, forwardSpeed * anglesPerUnit * Time.deltaTime);

                }

                //Steer left
                else {
                    backPlane.Raycast(new Ray(flWheel.transform.position, -flWheel.transform.right), out planeHit);
                    pivot = flWheel.transform.position - flWheel.transform.right * planeHit;

                    //Rotate around pivot
                    float perimeter = Vector3.Distance(blWheel.transform.position, pivot) * 2f * Mathf.PI;
                    float anglesPerUnit = 360f / perimeter;
                    transform.RotateAround(pivot, transform.up, -(forwardSpeed * anglesPerUnit * Time.deltaTime));
                }
            }

        }

        //Apply up speed
        if (!grounded) {
            transform.position = transform.position + Vector3.up * upSpeed * Time.deltaTime;
        }
        //Apply friction
        if (grounded) {
            forwardSpeed = Mathf.MoveTowards(forwardSpeed, 0f, settings.friction * Time.deltaTime);
        }

        //Overcome floor
        RaycastHit hit;
        bool didHit = Physics.Raycast(new Ray(GetBottom() + Vector3.up * 2f, Vector3.down), out hit, float.MaxValue, groundLayers);

        if (didHit) {
            SetBottom(hit.point);
            debugPoint = hit.point;
            grounded = true;
        } else {
            grounded = false;
        }

        if (grounded && didHit) {
            //Adjust the rotation of the car according to the floor
            Vector3 oldBottom = GetBottom();
            Quaternion newRotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
            
            transform.rotation = newRotation;
            SetBottom(oldBottom);

        }

    }

    #region Methods for Controller

    public void Accelerate() {

        if (grounded && forwardSpeed < settings.maxSpeed) {
            forwardSpeed += settings.acceleration * Time.deltaTime;
        }
        
    }

    public void Brake() {
        if (grounded) {
            forwardSpeed = Mathf.MoveTowards(forwardSpeed, 0f, settings.brakeForce * Time.deltaTime);
        }
    }

    public void Steer(float input) {
        float n = Mathf.InverseLerp(-1f, 1f, input);
        float steeringSpeed = Mathf.Lerp(settings.minSteeringSpeed, settings.maxSteeringSpeed, Mathf.InverseLerp(0f, settings.maxSpeed, forwardSpeed));
        //Debug.Log(steeringSpeed);

        Vector3 targetSteeringDirection = Quaternion.Lerp(Quaternion.Euler(0f, -30f, 0f), Quaternion.Euler(0f, 30f, 0f), n) * transform.forward;
        steeringDirection = Vector3.MoveTowards(steeringDirection, targetSteeringDirection, steeringSpeed * Time.deltaTime).normalized;
        flWheel.transform.rotation = Quaternion.LookRotation(steeringDirection);
        frWheel.transform.rotation = Quaternion.LookRotation(steeringDirection);
    }

    #endregion

    Vector3 GetWheelPosition(Wheel wheel) {
        switch (wheel) {
            case Wheel.FrontRight:
                return transform.position + transform.rotation * structure.steeringAxisPosition + transform.right * structure.steeringAxisLength * 0.5f;
            case Wheel.FrontLeft:
                return transform.position + transform.rotation * structure.steeringAxisPosition - transform.right * structure.steeringAxisLength * 0.5f;
            case Wheel.BackRight:
                return transform.position + transform.rotation * structure.backAxisPosition + transform.right * structure.backAxisLength * 0.5f;
            case Wheel.BackLeft:
                return transform.position + transform.rotation * structure.backAxisPosition - transform.right * structure.backAxisLength * 0.5f;
            default:
                return new Vector3();
        }
    }

    Vector3 GetWheelBottom(Wheel wheel) {
        Vector3 pos = GetWheelPosition(wheel);
        //Vector3 bottom = pos - transform.up * 
        return pos;
    }

    Vector3 GetBottom() {
        if (collider == null) return Vector3.zero;
        return transform.position + (transform.rotation * collider.center) - transform.up * collider.size.y * 0.5f;
    }

    void SetBottom(Vector3 point) {
        transform.position = point;
        Vector3 delta = point - GetBottom();
        transform.position += delta;
    }

    Vector3 debugPoint;

    private void OnDrawGizmos() {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position + (transform.rotation * structure.steeringAxisPosition), 0.1f);
        Gizmos.DrawRay(transform.position + (transform.rotation * structure.steeringAxisPosition), steeringDirection);

        if(wheelMesh == null) {
            //Get Wheel Mesh
            wheelMesh = structure.wheelPrefab.GetComponent<MeshFilter>().sharedMesh;
        }

        if(flWheel == null) {
            Gizmos.color = Color.gray;
            Gizmos.DrawMesh(wheelMesh, GetWheelPosition(Wheel.FrontRight), transform.rotation);
            Gizmos.DrawMesh(wheelMesh, GetWheelPosition(Wheel.FrontLeft), transform.rotation);
            Gizmos.DrawMesh(wheelMesh, GetWheelPosition(Wheel.BackRight), transform.rotation);
            Gizmos.DrawMesh(wheelMesh, GetWheelPosition(Wheel.BackLeft), transform.rotation);
        }

        Gizmos.color = Color.cyan;
        //Gizmos.DrawSphere(pivot, 0.3f);

        Gizmos.DrawSphere(GetBottom(), 0.1f);

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(debugPoint, 0.1f);
    }

    public enum Wheel { FrontRight, FrontLeft, BackRight, BackLeft}

}

[System.Serializable]
public class CarHandlingSettings {
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
}

[System.Serializable]
public class CarStructureSettings {
    public GameObject wheelPrefab;
    public Vector3 steeringAxisPosition;
    public float steeringAxisLength;
    public Vector3 backAxisPosition;
    public float backAxisLength;
}
