using UnityEngine;

public class addOnion : MonoBehaviour
{
    public GameObject finalOnion;
    public Transform plate;
    public float revealDistance = 0.25f;

    private Renderer rend;
    private bool added = false; // flag for keeping track if ingredient has already been added

    void Start()
    {
        rend = finalOnion.GetComponent<Renderer>();     // grab ingredient mesh
        rend.enabled = false;                           // ensure disabled by default
    }

    // instead of Update() we use TryAddIngredient() called from the controller
    public void TryAddIngredient()
    {
        if (added) return;  // if already added

        GameObject[] onions = GameObject.FindGameObjectsWithTag("Onion");

        foreach (GameObject onion in onions)
        {
            float distance = Vector3.Distance(onion.transform.position, plate.position);

            if (distance <= revealDistance)
            {
                rend.enabled = true;
                onion.SetActive(false);
                added = true;  // mark as added
                return;
            }
        }
    }

    // method for the controller to check if this step is done
    public bool IsAdded() => added;
}