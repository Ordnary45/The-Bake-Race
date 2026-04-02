using UnityEngine;

public class RevealTopBun : MonoBehaviour
{
    public GameObject finalTopBun; // top bun mesh
    public Transform plate;         // finalPlate object
    public float revealDistance = 1.0f; // how close a Bun needs to be

    private Renderer rend;

    void Start()
    {
        // Start invisible
        rend = finalTopBun.GetComponent<Renderer>();
        rend.enabled = false;
    }

    void Update()
    {
        // Already revealed? Don't check anymore
        if (rend.enabled) return;

        // Check all Buns
        GameObject[] buns = GameObject.FindGameObjectsWithTag("Bun");
        foreach (GameObject bun in buns)
        {
            float distance = Vector3.Distance(bun.transform.position, plate.position);
            if (distance <= revealDistance)
            {
                rend.enabled = true; // reveal the top bun
                return; // stop checking
            }
        }
    }
}