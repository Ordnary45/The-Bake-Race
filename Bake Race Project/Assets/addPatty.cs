using UnityEngine;

public class addPatty : MonoBehaviour
{
    public GameObject finalPatty;
    public Transform plate;
    public float revealDistance = 1.0f;

    private Renderer rend;

    void Start()
    {
        rend = finalPatty.GetComponent<Renderer>();
        rend.enabled = false;
    }

    void Update()
    {
        if (rend.enabled) return;

        GameObject[] buns = GameObject.FindGameObjectsWithTag("Patty");

        foreach (GameObject bun in buns)
        {
            float distance = Vector3.Distance(bun.transform.position, plate.position);

            if (distance <= revealDistance)
            {
                rend.enabled = true;
                bun.SetActive(false);
                return;
            }
        }
    }
}