using UnityEngine;

public class HandGrabbing : MonoBehaviour
{
    public OVRInput.Controller controller;

    [Header("Settings")]
    public float torqueStrength = 120f;
    public float grabDistance = 0.4f;

    private Rigidbody doorRB;
    private Transform doorTransform;
    private HingeJoint doorHinge;

    private bool isGrabbing;

    void Update()
    {
        bool gripHeld = OVRInput.Get(OVRInput.Button.PrimaryHandTrigger, controller);

        if (doorRB == null)
        {
            FindDoor();
        }

        if (doorRB == null) return;

        float dist = Vector3.Distance(transform.position, doorTransform.position);

        isGrabbing = gripHeld && dist < grabDistance;
    }

    void FixedUpdate()
    {
        if (!isGrabbing || doorRB == null || doorHinge == null) return;

        // hinge axis in world space
        Vector3 axis = doorTransform.TransformDirection(doorHinge.axis);

        // hand direction relative to door
        Vector3 toHand = transform.position - doorTransform.position;

        // remove influence outside hinge axis
        Vector3 projected = Vector3.ProjectOnPlane(toHand, axis);

        // compute rotation direction
        float torque = Vector3.Dot(Vector3.Cross(axis, projected), axis);

        doorRB.AddTorque(axis * torque * torqueStrength, ForceMode.Acceleration);
    }

    void FindDoor()
    {
        GameObject door = GameObject.FindGameObjectWithTag("Door");

        if (door == null) return;

        doorTransform = door.transform;
        doorRB = door.GetComponent<Rigidbody>();
        doorHinge = door.GetComponent<HingeJoint>();
    }
}