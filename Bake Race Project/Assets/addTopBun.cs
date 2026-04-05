using UnityEngine;

public class addTopBun : MonoBehaviour
{
    public GameObject finalTopBun;
    public Transform plate;
    public float revealDistance = 0.25f;

    private Renderer rend;
    private bool added = false; // flag for keeping track if ingredient has already been added

    void Start()
    {
        rend = finalTopBun.GetComponent<Renderer>();    // grab ingredient mesh
        rend.enabled = false;                           // ensure disabled by default
    }

    // instead of Update() we use TryAddIngredient() called from the controller
    public void TryAddIngredient()
    {
        if (added) return;  // if already added

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

    // method for the controller to check if this step is done
    public bool IsAdded() => added;
}