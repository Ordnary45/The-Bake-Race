using UnityEngine;

public class LightShowController : MonoBehaviour
{
    [Header("Lights to Animate")]
    public Light[] lights;

    [Header("Colour Settings")]
    public Gradient colourGradient;
    public float colourSpeed = 1f;

    [Header("Intensity Settings")]
    public float minIntensity = 5f;
    public float maxIntensity = 20f;
    public float pulseSpeed = 2f;

    private float time;

    void Update()
    {
        time += Time.deltaTime;

        // Animate colours + intensity
        foreach (var light in lights)
        {
            float t = Mathf.PingPong(time * colourSpeed, 1f);
            light.color = colourGradient.Evaluate(t);

            float intensity = Mathf.Lerp(minIntensity, maxIntensity,
                                         Mathf.PingPong(time * pulseSpeed, 1f));
            light.intensity = intensity;
        }
    }
}