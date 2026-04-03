using UnityEngine;

public class addLettuce : MonoBehaviour
{
    public GameObject finalLettuce;
    public Transform plate;
    public float revealDistance = 0.25f; //1.0f;

    private Renderer rend;

    void Start()
    {
        rend = finalLettuce.GetComponent<Renderer>();
        rend.enabled = false;
    }

    void Update()
    {
        if (rend.enabled) return;

        GameObject[] buns = GameObject.FindGameObjectsWithTag("Lettuce");

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