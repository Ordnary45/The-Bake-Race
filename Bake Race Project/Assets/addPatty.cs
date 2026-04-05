using UnityEngine;

public class addPatty : MonoBehaviour
{
    public GameObject finalPatty;
    public Transform plate;
    public float revealDistance = 0.25f;

    private Renderer rend;
    private bool added = false; // flag for keeping track if ingredient has already been added

    void Start()
    {
        rend = finalPatty.GetComponent<Renderer>();     // grab ingredient mesh
        rend.enabled = false;                           // ensure disabled by default
    }

    // instead of Update() we use TryAddIngredient() called from the controller
    public void TryAddIngredient()
    {
        if (added) return;  // if already added

        GameObject[] pattys = GameObject.FindGameObjectsWithTag("Patty");

        foreach (GameObject patty in pattys)
        {
            float distance = Vector3.Distance(patty.transform.position, plate.position);

            if (distance <= revealDistance)
            {
                rend.enabled = true;
                patty.SetActive(false);
                added = true;  // mark as added
                return;
            }
        }
    }

    // method for the controller to check if this step is done
    public bool IsAdded() => added;
}