using UnityEngine;

public class addBuns : MonoBehaviour
{
    public GameObject finalTopBun;
    public GameObject finalBottomBun;
    public Transform plate;
    public float revealDistance = 0.25f;

    private Renderer topRend, bottomRend;

    private static int bunCount = 0;

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

            if (distance <= revealDistance)
            {
                if (bunCount == 0)
                    bottomRend.enabled = true;
                else if (bunCount == 1)
                    topRend.enabled = true;

                bunCount++;
                bun.SetActive(false);
                return;
            }
        }
    }
}