using UnityEngine;
using Oculus.Interaction;
using Oculus.Interaction.HandGrab;
using System.Collections;
using System.Collections.Generic;
using EzySlice;
using UnityEngine.InputSystem;

public class KnifeSlice : MonoBehaviour
{
    [Header("Knife Settings")]
    public Transform startPoint;                // Start of the knife
    public Transform endPoint;                  // End of the knife
    public VelocityEstimator velocityEstimator; // Reference to the velocity estimator component
    public LayerMask sliceable;                 // Layer mask for sliceable objects

    [Header("Slice Settings")]
    public float minVelocityForCut = 0.5f;      // Minimum velocity required to perform a cut
    public float sliceCooldown = 0.1f;          // Cooldown to avoid multiple slices in quick succession

    [Header("Default Materials")]
    public Material defaultCrossSectionMaterial;// Material placed on cross-section

    private Dictionary<GameObject, float> lastSliceTime = new Dictionary<GameObject, float>();
    private bool isSlicing = false;

    // Start velocity extimation
    void Start()
    {
        if (velocityEstimator != null)
        {
            velocityEstimator.BeginEstimatingVelocity();
        }
    }

    // Raycast based slicing
    void FixedUpdate()
    {
        if (isSlicing) return;

        // Perform raycast along knife blade to detect sliceable objects
        Ray ray = new Ray(startPoint.position, endPoint.position - startPoint.position);
        bool hasHit = Physics.Raycast(ray, out RaycastHit hit, Vector3.Distance(startPoint.position, endPoint.position), sliceable);

        if (hasHit)
        {
            GameObject target = hit.transform.gameObject;
            SliceableObj cuttable = target.GetComponent<SliceableObj>();

            // Skip if object is not sliceable or under cooldown or moving too slowly
            if (cuttable == null || !cuttable.IsSliceable)
                return;
            if (lastSliceTime.ContainsKey(target) && Time.time - lastSliceTime[target] < sliceCooldown)
                return;
            Vector3 velocity = velocityEstimator.GetVelocityEstimate();
            if (velocity.magnitude < minVelocityForCut)
                return;

            lastSliceTime[target] = Time.time;

            StartCoroutine(SliceCoroutine(target, cuttable, velocity));
        }

    }

    // Trigger based slicing when collision
    void OnTriggerEnter(Collider other)
    {
        SliceableObj cuttable = other.GetComponent<SliceableObj>();
        if (cuttable != null && cuttable.IsSliceable)
        {
            Vector3 velocity = velocityEstimator.GetVelocityEstimate();
            if (velocity.magnitude >= minVelocityForCut)
            {
                StartCoroutine(SliceCoroutine(cuttable.gameObject, cuttable, velocity));
            }
        }
    }

    // Coroutine to prevent multiple slices
    IEnumerator SliceCoroutine(GameObject target, SliceableObj cuttable, Vector3 velocity)
    {
        isSlicing = true;

        // Perform the slice
        Slice(target, cuttable, velocity);
        // The delay
        yield return new WaitForSeconds(0.1f);
        isSlicing = false;
    }

    // Main slicing logic that uses EzySlice
    public void Slice(GameObject target, SliceableObj cuttable, Vector3 velocity)
    {
        // Make the slice plane based on direction and velocity
        Vector3 sliceDirection = endPoint.position - startPoint.position;
        Vector3 planeNormal = Vector3.Cross(sliceDirection, velocity);
        planeNormal.Normalize();

        Material crossSectionMat = cuttable.crossSectionMaterial;
        if (crossSectionMat == null)
        {
            crossSectionMat = defaultCrossSectionMaterial;

            if (crossSectionMat == null)
            {
                crossSectionMat = new Material(Shader.Find("Standard"));
                crossSectionMat.color = new Color(0.7f, 0.7f, 0.7f, 1f);
            }
        }

        cuttable.PrepareForSlice(); //Play slice sound
        SlicedHull hull = target.Slice(endPoint.position, planeNormal); // Do the slice

        if (hull != null)
        {
            Transform originalParent = target.transform.parent;
            Vector3 originalParentScale = originalParent != null ? originalParent.localScale : Vector3.one;

            // Store the original sibling index to maintain order
            int originalSiblingIndex = target.transform.GetSiblingIndex();

            // Turn off original collider
            Collider originalCollider = target.GetComponent<Collider>();
            if (originalCollider != null)
                originalCollider.enabled = false;

            // Create the sliced hulls
            GameObject upperHull = hull.CreateUpperHull(target, crossSectionMat);
            GameObject lowerHull = hull.CreateLowerHull(target, crossSectionMat);

            // Store the HandGrab components before destroying
            HandGrabInteractable originalHandGrab = null;
            Grabbable originalGrabbable = null;

            if (originalParent != null)
            {
                originalHandGrab = originalParent.GetComponentInChildren<HandGrabInteractable>();
                originalGrabbable = originalParent.GetComponent<Grabbable>();
            }

            // Create new parents for both pieces with proper HandGrab setup
            GameObject upperParent = CreateNewPieceParent(originalParent, originalParentScale, upperHull, "Upper");
            GameObject lowerParent = CreateNewPieceParent(originalParent, originalParentScale, lowerHull, "Lower");

            // Set up upper hull
            upperHull.transform.SetParent(upperParent.transform, false);
            upperHull.transform.localPosition = Vector3.zero;
            upperHull.transform.localRotation = Quaternion.identity;
            upperHull.transform.localScale = Vector3.one;

            // Set up lower hull
            lowerHull.transform.SetParent(lowerParent.transform, false);
            lowerHull.transform.localPosition = Vector3.zero;
            lowerHull.transform.localRotation = Quaternion.identity;
            lowerHull.transform.localScale = Vector3.one;

            // Apply slice mesh and set layer
            SetSliced(upperHull, cuttable);
            SetSliced(lowerHull, cuttable);

            // Register the pieces
            cuttable.RegisterSlicedPiece(upperHull);
            cuttable.RegisterSlicedPiece(lowerHull);

            // Position the new parents at the original parent's position
            if (originalParent != null)
            {
                upperParent.transform.position = originalParent.position;
                upperParent.transform.rotation = originalParent.rotation;
                lowerParent.transform.position = originalParent.position;
                lowerParent.transform.rotation = originalParent.rotation;
            }

            // Setup grab components on the new pieces
            StartCoroutine(SetUpGrab(upperParent, originalHandGrab, originalGrabbable));
            StartCoroutine(SetUpGrab(lowerParent, originalHandGrab, originalGrabbable));

            // Turn off grab components before destroying
            Transform interactionChild = originalParent.Find("ISDK_HandGrabInteraction");
            if (interactionChild != null)
            {
                interactionChild.gameObject.SetActive(false);
            }

            Grabbable oldGrab = originalParent.GetComponent<Grabbable>();
            if (oldGrab != null)
            {
                oldGrab.enabled = false;
            }

            Destroy(target.transform.parent.gameObject); // Clean up original
        }
    }

    // Helper method to make anew parent for sliced pieces
    private GameObject CreateNewPieceParent(Transform originalParent, Vector3 originalScale, GameObject hull, string pieceName)
    {
        GameObject newParent = new GameObject(originalParent.name + "_" + pieceName);

        // Transform
        newParent.transform.position = originalParent.position;
        newParent.transform.rotation = originalParent.rotation;
        newParent.transform.localScale = originalScale;

        //Rigidbody
        Rigidbody originalRb = originalParent.GetComponent<Rigidbody>();
        Rigidbody newRb = newParent.AddComponent<Rigidbody>();

        if (originalRb != null)
        {
            newRb.mass = originalRb.mass / 2f;
            newRb.linearDamping = originalRb.linearDamping;
            newRb.angularDamping = originalRb.angularDamping;
            newRb.useGravity = originalRb.useGravity;
            newRb.isKinematic = false;
        }

        // Find the type of collider the original is and add the same one to the new parent
        Collider originalCollider = originalParent.GetComponent<Collider>();
        System.Type type = originalCollider.GetType();
        Collider newCollider = (Collider)newParent.AddComponent(type);

        if (originalCollider is BoxCollider box)
    {
        BoxCollider newBox = newCollider as BoxCollider;
        newBox.center = box.center;
        newBox.size = box.size;
    }
    else if (originalCollider is SphereCollider sphere)
    {
        SphereCollider newSphere = newCollider as SphereCollider;
        newSphere.center = sphere.center;
        newSphere.radius = sphere.radius;
    }

        //Grabbable
        Grabbable grabbable = newParent.AddComponent<Grabbable>();

        grabbable.InjectOptionalRigidbody(newRb);
        return newParent;
    }

    // Coroutine to set up grab components on the new sliced pieces
    private IEnumerator SetUpGrab(GameObject pieceParent, HandGrabInteractable originalHandGrab, Grabbable originalGrabbable)
    {
        // Give time for object to initialize before adding grab components
        yield return new WaitForFixedUpdate();
        yield return new WaitForFixedUpdate();

        // Get the Rigidbody and Grabbable from the parent
        Rigidbody rb = pieceParent.GetComponent<Rigidbody>();
        Grabbable grabbable = pieceParent.GetComponent<Grabbable>();

        // Stuff needed for grab
        GrabFreeTransformer transformer = pieceParent.AddComponent<GrabFreeTransformer>();
        grabbable.InjectOptionalOneGrabTransformer(transformer);
        grabbable.InjectOptionalTwoGrabTransformer(transformer);
        transformer.Initialize(grabbable);

        // More time
        yield return null;

        // Create a child GameObject to hold the interaction components
        GameObject interactionChild =
            new GameObject("ISDK_HandGrabInteraction_" + System.Guid.NewGuid());

        interactionChild.transform.SetParent(pieceParent.transform, false);
        interactionChild.transform.localPosition = Vector3.zero;
        interactionChild.transform.localRotation = Quaternion.identity;

        // Hand grab interactable
        HandGrabInteractable handGrab = interactionChild.AddComponent<HandGrabInteractable>();

        if (originalHandGrab != null)
        {
            handGrab.HandAlignment = originalHandGrab.HandAlignment;
        }

        handGrab.InjectRigidbody(rb);
        handGrab.InjectOptionalPointableElement(grabbable);

        // Grab interactable
        GrabInteractable grabInteractable = interactionChild.AddComponent<GrabInteractable>();

        grabInteractable.InjectRigidbody(rb);
        grabInteractable.InjectOptionalPointableElement(grabbable);

        rb.WakeUp();
    }

    // Helper method to make the mesh collider, set the layer, and not yet reached max slice count
    public void SetSliced(GameObject piece, SliceableObj originalCuttable)
    {
        MeshCollider collider = piece.GetComponent<MeshCollider>();
        if (collider == null)
        {
            collider = piece.AddComponent<MeshCollider>();
        }
        collider.sharedMesh = piece.GetComponent<MeshFilter>().mesh;
        collider.convex = true;

        // Set layer to the same as original
        piece.layer = originalCuttable.gameObject.layer;

        if (((1 << piece.layer) & sliceable) == 0)
        {
            for (int i = 0; i < 32; i++)
            {
                if ((sliceable & (1 << i)) != 0)
                {
                    piece.layer = i;
                    break;
                }
            }
        }
    }
}
