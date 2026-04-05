using UnityEngine;

public class addTomato : MonoBehaviour
{
    public GameObject finalTomato;
    public Transform plate;
    public float revealDistance = 0.25f;

    private Renderer rend;
    private bool added = false; // flag for keeping track if ingredient has already been added

    void Start()
    {
        rend = finalTomato.GetComponent<Renderer>();    // grab ingredient mesh
        rend.enabled = false;                           // ensure disabled by default
    }

    // instead of Update() we use TryAddIngredient() called from the controller
    public void TryAddIngredient()
    {
        if (added) return;  // if already added

        GameObject[] tomatoes = GameObject.FindGameObjectsWithTag("Tomato");

        foreach (GameObject tomato in tomatoes)
        {
            float distance = Vector3.Distance(tomato.transform.position, plate.position);

            if (distance <= revealDistance)
            {
                rend.enabled = true;
                tomato.SetActive(false);
                added = true;  // mark as added
                return;
            }
        }
    }

    // method for the controller to check if this step is done
    public bool IsAdded() => added;
}