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

        bool hasHit = Physics.Linecast(startPoint.position, endPoint.position, out RaycastHit hit, sliceable);
        if(hasHit)
        {
            GameObject target = hit.transform.gameObject;
            SliceableObj cuttable = target.GetComponent<SliceableObj>();
            target = cuttable.gameObject;

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

        SlicedHull hull = target.Slice(endPoint.position, planeNormal);

        if (hull != null)
        {
            Collider originalCollider = target.GetComponent<Collider>();
            if (originalCollider != null)
                originalCollider.enabled = false;

            GameObject upperHull = hull.CreateUpperHull(target, crossSectionMat);
            GameObject lowerHull = hull.CreateLowerHull(target, crossSectionMat);

            SetSliced(upperHull, cuttable, velocity, planeNormal);
            SetSliced(lowerHull, cuttable, velocity, -planeNormal);

            cuttable.RegisterSlicedPiece(upperHull);
            cuttable.RegisterSlicedPiece(lowerHull);

            Destroy(target);
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

        // Add knife slice component to allow further cutting
        KnifeSlice sliceScript = piece.AddComponent<KnifeSlice>();
        sliceScript.startPoint = startPoint;
        sliceScript.endPoint = endPoint;
        sliceScript.velocityEstimator = velocityEstimator;
        sliceScript.sliceable = sliceable;
        sliceScript.defaultCrossSectionMaterial = defaultCrossSectionMaterial;
        sliceScript.minVelocityForCut = minVelocityForCut;
        sliceScript.sliceCooldown = sliceCooldown;
        sliceScript.upwardForceMultiplier = upwardForceMultiplier;
    }
}
