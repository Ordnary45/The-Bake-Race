using UnityEngine;

public class addTomato : MonoBehaviour
{
    public GameObject finalTomato;
    public Transform plate;
    public float revealDistance = 0.25f; //1.0f;

    private Renderer rend;

    void Start()
    {
        rend = finalTomato.GetComponent<Renderer>();
        rend.enabled = false;
    }

    void Update()
    {
        if (rend.enabled) return;

        GameObject[] buns = GameObject.FindGameObjectsWithTag("Tomato");

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