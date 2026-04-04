using UnityEngine;

public class addBottomBun : MonoBehaviour
{
    public GameObject finalBottomBun;
    public Transform plate;
    public float revealDistance = 0.25f;

    private Renderer rend;
    private bool added = false;

    void Start()
    {
        rend = finalBottomBun.GetComponent<Renderer>();
        rend.enabled = false;
    }

    public void TryAddIngredient()
    {
        if (added) return;

        GameObject[] buns = GameObject.FindGameObjectsWithTag("Bun");
        foreach (GameObject bun in buns)
        {
            if (Vector3.Distance(bun.transform.position, plate.position) <= revealDistance)
            {
                rend.enabled = true;
                bun.SetActive(false);
                added = true;
                return;
            }
        }
    }

    public bool IsAdded() => added;
}