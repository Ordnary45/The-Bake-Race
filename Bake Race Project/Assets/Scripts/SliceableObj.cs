using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using EzySlice;


public class SliceableObj : MonoBehaviour
{
    [Header("Slice Settings")]
    public Material crossSectionMaterial;
    public int maxSlices = 5; // Maximum number of times this object can be sliced
    public float minSliceVelocity = 0.5f;
    public bool canBeSliced = true;

    [Header("Physics Settings")]
    public float mass = 1f;
    public float bounceForce = 2f;
    public float upwardForceMultiplier = 0.5f;

    private int sliceCount = 0;
    private List<GameObject> slicedPieces = new List<GameObject>();

    public int SliceCount => sliceCount;
    public bool IsSliceable => canBeSliced && sliceCount < maxSlices;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Set initial rigidbody properties if it exists
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.mass = mass;
        }

        // If no cross-section material is assigned, try to find one
        if (crossSectionMaterial == null)
        {
            crossSectionMaterial = GetDefaultCrossSectionMaterial();
        }
    }

    Material GetDefaultCrossSectionMaterial()
    {
        // Try to find a default material in resources
        Material defaultMat = Resources.Load<Material>("DefaultCrossSection");
        if (defaultMat == null)
        {
            // Create a default material if none exists
            defaultMat = new Material(Shader.Find("Black"));
            defaultMat.color = Color.gray;
        }
        return defaultMat;
    }

    public void RegisterSlicedPiece(GameObject piece)
    {
        sliceCount++;
        if (!slicedPieces.Contains(piece))
        {
            slicedPieces.Add(piece);

            // Add cuttable component to the new piece
            SliceableObj newSliceable = piece.AddComponent<SliceableObj>();
            newSliceable.crossSectionMaterial = crossSectionMaterial;
            newSliceable.maxSlices = maxSlices - sliceCount;
            newSliceable.minSliceVelocity = minSliceVelocity;
            newSliceable.mass = mass / 2f; // Pieces are lighter
            newSliceable.bounceForce = bounceForce;
            newSliceable.upwardForceMultiplier = upwardForceMultiplier;
        }
    }
}
