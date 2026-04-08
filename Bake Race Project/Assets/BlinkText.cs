using UnityEngine;
using TMPro;

public class BlinkText : MonoBehaviour
{
    public float blinkInterval = 0.5f; // Time between each blink
    private TextMeshPro textMesh;
    private bool isVisible = true;

    void Start()
    {
        textMesh = GetComponent<TextMeshPro>(); // Get the TextMeshPro component attached to the object
        InvokeRepeating("ToggleText", blinkInterval, blinkInterval); // Repeatedly call ToggleText at set intervals
    }

    void ToggleText()
    {
        isVisible = !isVisible; // Flip visibility state
        textMesh.enabled = isVisible; // Apply visibility to the text component
    }
}