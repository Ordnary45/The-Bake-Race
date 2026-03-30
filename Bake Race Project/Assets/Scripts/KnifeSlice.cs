using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using EzySlice;
using UnityEngine.InputSystem;

public class KnifeSlice : MonoBehaviour
{
    [Header("Knife Settings")]
    public Transform startPoint;
    public Transform endPoint;
    public VelocityEstimator velocityEstimator;
    public LayerMask sliceable;

    [Header("Slice Settings")]
    public float minVelocityForCut = 0.5f;
    public float sliceCooldown = 0.1f;
    public float upwardForceMultiplier = 0.5f;

    [Header("Default Materials")]
    public Material defaultCrossSectionMaterial;

    private Dictionary<GameObject, float> lastSliceTime = new Dictionary<GameObject, float>();
    private bool isSlicing = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (velocityEstimator != null)
        {
            velocityEstimator.BeginEstimatingVelocity();
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isSlicing) return;

        Ray ray = new Ray(startPoint.position, endPoint.position - startPoint.position);
        bool hasHit = Physics.Raycast(ray, out RaycastHit hit, Vector3.Distance(startPoint.position, endPoint.position), sliceable);

        if(hasHit)
        {
            //Debug.Log($"Hit: {hit.transform.name}");

            GameObject target = hit.transform.gameObject;
            SliceableObj cuttable = target.GetComponent<SliceableObj>();

            if (cuttable == null || !cuttable.IsSliceable)
                return;

            if (lastSliceTime.ContainsKey(target) && Time.time - lastSliceTime[target] < sliceCooldown)
                return;

            Vector3 velocity = velocityEstimator.GetVelocityEstimate();
            if (velocity.magnitude < minVelocityForCut && velocity.magnitude < cuttable.minSliceVelocity)
                return;

            lastSliceTime[target] = Time.time;

            StartCoroutine(SliceCoroutine(target, cuttable, velocity));
        }

    }

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

    IEnumerator SliceCoroutine(GameObject target, SliceableObj cuttable, Vector3 velocity)
    {
        isSlicing = true;

        // Perform the slice
        Slice(target, cuttable, velocity);

        yield return new WaitForSeconds(0.05f);
        isSlicing = false;
    }

    public void Slice(GameObject target, SliceableObj cuttable, Vector3 velocity)
    {
        Vector3 sliceDirection = endPoint.position - startPoint.position;
        Vector3 planeNormal = Vector3.Cross(sliceDirection, velocity);
        planeNormal.Normalize();

        if (velocity.magnitude < 0.5f) return;

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

        cuttable.PrepareForSlice();
        SlicedHull hull = target.Slice(endPoint.position, planeNormal);

        if (hull != null)
        {
            // Store original parent and its properties
            Transform originalParent = target.transform.parent;
            Vector3 originalParentPosition = originalParent != null ? originalParent.position : Vector3.zero;
            Quaternion originalParentRotation = originalParent != null ? originalParent.rotation : Quaternion.identity;
            Vector3 originalParentScale = originalParent != null ? originalParent.localScale : Vector3.one;

            // Store the ISDK sibling reference
            GameObject originalISDK = cuttable.ISDK;

            // Store the original sibling index to maintain order
            int originalSiblingIndex = target.transform.GetSiblingIndex();

            Collider originalCollider = target.GetComponent<Collider>();
            if (originalCollider != null)
                originalCollider.enabled = false;

            // Create the sliced hulls
            GameObject upperHull = hull.CreateUpperHull(target, crossSectionMat);
            GameObject lowerHull = hull.CreateLowerHull(target, crossSectionMat);

            // Create parent copy for upper hull
            GameObject upperParent = null;
            if (originalParent != null)
            {
                // Instantiate a copy of the original parent
                upperParent = Instantiate(originalParent.gameObject, originalParentPosition, originalParentRotation);
                upperParent.transform.localScale = originalParentScale;
                upperParent.name = originalParent.name + upperHull.name;
                Destroy(upperParent.transform.GetChild(0).gameObject);
            }
            else
            {
                // If no parent, create a new empty parent
                upperParent = new GameObject("SlicedPiece_Upper");
                upperParent.transform.position = Vector3.zero;
            }

            // Create parent copy for lower hull
            GameObject lowerParent = null;
            if (originalParent != null)
            {
                // Instantiate a copy of the original parent
                lowerParent = Instantiate(originalParent.gameObject, originalParentPosition, originalParentRotation);
                lowerParent.transform.localScale = originalParentScale;
                lowerParent.name = originalParent.name + lowerHull.name;
                Destroy(lowerParent.transform.GetChild(0).gameObject);
            }
            else
            {
                // If no parent, create a new empty parent
                lowerParent = new GameObject("SlicedPiece_Lower");
                lowerParent.transform.position = Vector3.zero;
            }

            // Set up upper hull
            upperHull.transform.SetParent(upperParent.transform);
            upperHull.transform.localScale = target.transform.localScale;
            upperHull.transform.rotation = target.transform.rotation;
            upperHull.transform.localPosition = target.transform.localPosition;

            // Set up lower hull
            lowerHull.transform.SetParent(lowerParent.transform);
            lowerHull.transform.localScale = target.transform.localScale;
            lowerHull.transform.rotation = target.transform.rotation;
            lowerHull.transform.localPosition = target.transform.localPosition;

            // Apply slice effects and physics
            SetSliced(upperHull, cuttable, velocity, planeNormal);
            SetSliced(lowerHull, cuttable, velocity, -planeNormal);

            // Register the pieces (this will create copies of the ISDK objects)
            cuttable.RegisterSlicedPiece(upperHull);
            cuttable.RegisterSlicedPiece(lowerHull);

            // Copy any additional components from original parent to new parents
            if (originalParent != null)
            {
                CopyParentComponents(originalParent.gameObject, upperParent);
                CopyParentComponents(originalParent.gameObject, lowerParent);
            }

            // Position the new parents at the original parent's position
            if (originalParent != null)
            {
                upperParent.transform.position = originalParent.position;
                upperParent.transform.rotation = originalParent.rotation;
                lowerParent.transform.position = originalParent.position;
                lowerParent.transform.rotation = originalParent.rotation;
            }

            // Destroy the original ISDK if it exists
            if (originalISDK != null)
            {
                Destroy(originalISDK);
            }

            Destroy(target.transform.parent.gameObject);
        }
    }

    public void SetSliced(GameObject piece, SliceableObj originalCuttable, Vector3 cutVelocity, Vector3 direction)
    {
        MeshCollider collider = piece.GetComponent<MeshCollider>();
        if (collider == null)
        {
            collider = piece.AddComponent<MeshCollider>();
        }
        collider.convex = true;

        Rigidbody rb = piece.GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = piece.AddComponent<Rigidbody>();
        }

        //rb.AddExplosionForce(cutForce, slicedObject.transform.position, 1);
        //rb.AddForce(Vector3.up * cutForce * 0.3f, ForceMode.Impulse);

        // Set physics properties
        rb.mass = originalCuttable.mass / 2f;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;

        // Apply force based on cut velocity
        Vector3 forceDirection = cutVelocity.normalized;
        forceDirection.y += originalCuttable.upwardForceMultiplier;
        forceDirection.Normalize();

        float forceMagnitude = Mathf.Clamp(cutVelocity.magnitude * 1.5f, 1f, 20f);
        rb.AddForce(forceDirection * forceMagnitude, ForceMode.Impulse);

        // Add torque for spin
        Vector3 torque = Vector3.Cross(direction, cutVelocity.normalized) * forceMagnitude;
        rb.AddTorque(torque * 0.5f, ForceMode.Impulse);

        // Add small random force for variety
        rb.AddForce(Random.insideUnitSphere * 2f, ForceMode.Impulse);

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

    // Helper method to copy components from original parent to new parent
    void CopyParentComponents(GameObject sourceParent, GameObject destParent)
    {
        // Copy all components except Transform
        Component[] components = sourceParent.GetComponents<Component>();
        foreach (Component component in components)
        {
            if (component is Transform)
                continue;

            System.Type componentType = component.GetType();
            Component newComponent = destParent.GetComponent(componentType);

            if (newComponent == null)
            {
                newComponent = destParent.AddComponent(componentType);
            }

            // Copy public fields
            foreach (var field in componentType.GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance))
            {
                try
                {
                    field.SetValue(newComponent, field.GetValue(component));
                }
                catch (System.Exception e)
                {
                    Debug.LogWarning($"Failed to copy field {field.Name}: {e.Message}");
                }
            }
        }
    }
}
