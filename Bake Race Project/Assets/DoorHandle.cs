using UnityEngine;

public class DoorHandle : MonoBehaviour
{
    public OVRInput.Controller controller;

    public float grabDistance = 2f;
    public float rotationSpeed = 8f;
    public float torqueStrength = 15f;

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

        bool vrGrab = OVRInput.Get(OVRInput.Button.PrimaryHandTrigger, controller);

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

        doorRB.AddTorque(axis * delta * torqueStrength, ForceMode.VelocityChange);

        lastAngle = currentAngle;
    }

    float GetHandAngle()
    {
        Vector3 local = door.InverseTransformPoint(transform.position);
        return Mathf.Atan2(local.x, local.z) * Mathf.Rad2Deg;
    }
}
