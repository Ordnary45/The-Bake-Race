using UnityEngine;

public class cookPatty : MonoBehaviour
{
    public Material rawMaterial;
    public Material cookedMaterial;

    private Renderer rend;
    private bool isCooked = false;

    void Start()
    {
        Debug.Log("START");
        
        rend = GetComponent<Renderer>();
        rend.material = rawMaterial;
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter HIT");

        if (isCooked) return;

        if (other.CompareTag("Pan"))
        {
            Debug.Log("CONTACT MADE WITH PAN!!!");
            Cook();
        }
    }

    void Cook()
    {
        rend.material = cookedMaterial;
        isCooked = true;
    }
}