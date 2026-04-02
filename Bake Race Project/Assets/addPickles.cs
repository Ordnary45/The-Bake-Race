using UnityEngine;

public class addPickles : MonoBehaviour
{
    public GameObject finalPickles;
    public Transform plate;
    public float revealDistance = 1.0f;

    private Renderer rend;

    void Start()
    {
        rend = finalPickles.GetComponent<Renderer>();
        rend.enabled = false;
    }

    void Update()
    {
        if (rend.enabled) return;

        GameObject[] buns = GameObject.FindGameObjectsWithTag("Pickles");

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