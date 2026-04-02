using UnityEngine;

public class DoorHandle : MonoBehaviour
{
    public OVRInput.Controller controller;

    public float grabDistance = 0.5f;
    public float rotationSpeed = 4f;
    public float torqueStrength = 1f;

    private Rigidbody doorRB;
    private HingeJoint hinge;
    private Transform door;

    private bool isGrabbing;
    private float lastAngle;

    void Start()
    {
        GameObject doorObj = GameObject.FindGameObjectWithTag("Door");

        if (doorObj == null)
        {
            Debug.LogError("Door not found!");
            return;
        }

        door = doorObj.transform;
        doorRB = doorObj.GetComponent<Rigidbody>();
        hinge = doorObj.GetComponent<HingeJoint>();
    }

    void Update()
    {
        if (door == null) return;

        //  CHANGED HERE (A / X button instead of trigger)
        bool vrGrab = OVRInput.Get(OVRInput.Button.Two, controller);

        float dist = Vector3.Distance(transform.position, door.position);

        if (vrGrab && dist < grabDistance)
        {
            if (!isGrabbing)
            {
                isGrabbing = true;
                lastAngle = GetHandAngle();
            }
        }
        else
        {
            isGrabbing = false;
        }
    }

    void FixedUpdate()
    {
        if (!isGrabbing || doorRB == null || hinge == null) return;

        float currentAngle = GetHandAngle();

        float delta = Mathf.DeltaAngle(lastAngle, currentAngle);

        Vector3 axis = door.TransformDirection(hinge.axis);

        doorRB.AddTorque(axis * delta * torqueStrength, ForceMode.Force);

        lastAngle = currentAngle;
    }

    float GetHandAngle()
    {
        Vector3 local = door.InverseTransformPoint(transform.position);
        return Mathf.Atan2(local.x, local.z) * Mathf.Rad2Deg;
    }
}