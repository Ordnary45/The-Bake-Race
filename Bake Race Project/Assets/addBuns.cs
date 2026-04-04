using UnityEngine;

public class addBuns : MonoBehaviour
{
    public GameObject finalTopBun;
    public GameObject finalBottomBun;
    public Transform plate;
    public float revealDistance = 0.25f; //1.0f;

    private Renderer topRend, bottomRend;

    private static int bunCount = 0;

    private bool bunPlaced = false;

    void Start()
    {
        topRend = finalTopBun.GetComponent<Renderer>();
        bottomRend = finalBottomBun.GetComponent<Renderer>();

        topRend.enabled = false;
        bottomRend.enabled = false;
    }

    void Update()
    {
        if (bunCount >= 2) return;

        GameObject[] buns = GameObject.FindGameObjectsWithTag("Bun");

        foreach (GameObject bun in buns)
        {
            float distance = Vector3.Distance(bun.transform.position, plate.position);

            if (distance <= revealDistance && !bunPlaced)
            {
                bunPlaced = true; // lock so we only process this bun once

                bunCount++;

                if (bunCount == 1)
                    bottomRend.enabled = true;
                else if (bunCount == 2)
                    topRend.enabled = true;

                bun.SetActive(false);

                // reset after a short delay so next bun works
                Invoke(nameof(ResetBunPlaced), 0.2f);

                return;
            }
        }
    }

    void ResetBunPlaced()
    {
        bunPlaced = false;
    }
}