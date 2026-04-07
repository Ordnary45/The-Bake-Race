using UnityEngine;

public class DoorHandle : MonoBehaviour
{
    // Decides whihc controller is being used 
    public OVRInput.Controller controller;

    // The distance the controller needs to be to the door in order to grab 
    public float grabDistance = 0.2f;

    // How much force is applied to the door when trying to move it
    public float torqueStrength = 1f;

    // Used to find objects with the tag "Door"
    public string validTag = "Door";

    // Set references to componets of the targeted door 
    private Rigidbody doorRB;
    private HingeJoint hinge;
    private Transform door;

    // Tracking Variables 
    private bool isGrabbing;
    private float lastAngle;

    void Update()
    {
        // Scan for door witth the correct tag that is within grab distance
        TryFindDoor();

        // If there is no door in range, terminate the rest of the execution 
        if (door == null) return;

        // Sheck if user is pressing the grab button
        // B on right controller, Y on the left controller 
        bool vrGrab = OVRInput.Get(OVRInput.Button.Two, controller);

        // Calculate the distance from the users hand to the door 
        float dist = Vector3.Distance(transform.position, door.position);

        // Determine if the door is grabbable
        if (vrGrab && dist < grabDistance)
        {
            if (!isGrabbing)
            {
                isGrabbing = true;

                // Store the initial angle of the door
                lastAngle = GetHandAngle();
            }
        }
        else
        {
            // If the user let go of the grabbing button or the hand has moved out of grabbng distance 
            isGrabbing = false;
        }
    }

    // Used for physcis calculating to ensure the door moves smoothly
    void FixedUpdate()
    {
        // Don't apply required physics if door is not grabbed or hand is not in range
        if (!isGrabbing || doorRB == null || hinge == null) return;

        // Determine where the users hand is
        float currentAngle = GetHandAngle();

        // Calculate the difference in the angle since the last frame calculation
        float delta = Mathf.DeltaAngle(lastAngle, currentAngle);

        // Convert hinge axis into world space
        Vector3 axis = door.TransformDirection(hinge.axis);

        // Apply continious force to the door to ensure smooth movement 
        doorRB.AddTorque(axis * delta * torqueStrength, ForceMode.Force);

        // Update the last angle so the next frame will be properly calculated 
        lastAngle = currentAngle;
    }

    // Searches the environment to find the closest object tagged "Door" 
    void TryFindDoor()
    {
        // Create a vritual sphere around the hand to grab other colliders 
        Collider[] hit = Physics.OverlapSphere(transform.position, grabDistance);

        // Variables to keep track of the closest door
        Transform closest = null;
        Rigidbody closestRB = null;
        HingeJoint closestHinge = null;

        float closestDist = Mathf.Infinity;

        foreach (Collider col in hit)
        {
            // Ignore any object that does not have the tag "Door"
            if (!col.CompareTag(validTag))
            {
                continue;
            }

            // Check for the required components of the door 
            Rigidbody rb = col.GetComponentInParent<Rigidbody>();
            HingeJoint hj = col.GetComponentInParent<HingeJoint>();

            // If the door is missing the required components, it is not valid 
            if (rb == null || hj == null)
            {
                continue;
            }

            // Calculate the distance to the tragted door 
            float dist = Vector3.Distance(transform.position, rb.position);

            // If the ditance to the current door is closer than the previoud door, update trakcing variables 
            if (dist < closestDist)
            {
                closestDist = dist;
                closest = rb.transform;
                closestRB = rb;
                closestHinge = hj;
            }
        }

        // Assign the closest object with correct tag to these variables 
        door = closest;
        doorRB = closestRB;
        hinge = closestHinge;
    }

    // Calculates the angle of the users hand compared to the axis of the door
    float GetHandAngle()
    {
        // Convert the users controller position to the door's relative space 
        // Doing this makes it easier to calcuate door movement, making it smoother
        Vector3 local = door.InverseTransformPoint(transform.position);

        // Calcualte the angle of the door relative to the horzontal plane
        return Mathf.Atan2(local.x, local.z) * Mathf.Rad2Deg;
    }
}