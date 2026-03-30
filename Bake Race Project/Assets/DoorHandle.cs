using UnityEngine;

public class VRFridgeDoorGrab : MonoBehaviour
{
    public Rigidbody doorRB;

    public Transform handle;

    public Transform leftHand;
    public Transform rightHand;

    public float grabDistance = 0.15f;
    public float torqueStrength = 120f;
    public float damping = 8f;

    private bool leftGrabbing;
    private bool rightGrabbing;

    void Update()
    {
        leftGrabbing = OVRInput.Get(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.LTouch);
        rightGrabbing = OVRInput.Get(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.RTouch);
    }

    void FixedUpdate()
    {
        Vector3 torque = Vector3.zero;

        // LEFT HAND
        if (leftGrabbing)
        {
            float dist = Vector3.Distance(leftHand.position, handle.position);

            if (dist < grabDistance)
            {
                Vector3 dir = leftHand.position - handle.position;
                torque += Vector3.Cross(Vector3.up, dir);
            }
        }

        // RIGHT HAND
        if (rightGrabbing)
        {
            float dist = Vector3.Distance(rightHand.position, handle.position);

            if (dist < grabDistance)
            {
                Vector3 dir = rightHand.position - handle.position;
                torque += Vector3.Cross(Vector3.up, dir);
            }
        }

        // Apply torque
        doorRB.AddTorque(torque * torqueStrength, ForceMode.Acceleration);

        // damping (stops spinning forever)
        doorRB.angularVelocity *= (1f - Time.fixedDeltaTime * damping);
    }
}