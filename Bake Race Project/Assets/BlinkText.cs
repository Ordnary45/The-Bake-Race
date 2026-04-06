using UnityEngine;
using TMPro;

public class BlinkText : MonoBehaviour
{
    public float blinkInterval = 0.5f;
    private TextMeshPro textMesh;
    private bool isVisible = true;

    void Start()
    {
        textMesh = GetComponent<TextMeshPro>();
        InvokeRepeating("ToggleText", blinkInterval, blinkInterval);
    }

    void ToggleText()
    {
        isVisible = !isVisible;
        textMesh.enabled = isVisible;
    }
}