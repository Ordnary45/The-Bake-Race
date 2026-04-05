using UnityEngine;

public class addLettuce : MonoBehaviour
{
    public GameObject finalLettuce;
    public Transform plate;
    public float revealDistance = 0.25f;

    private Renderer rend;
    private bool added = false; // flag for keeping track if ingredient has already been added

    void Start()
    {
        rend = finalLettuce.GetComponent<Renderer>();   // grab ingredient mesh
        rend.enabled = false;                           // ensure disabled by default
    }

    // instead of Update() we use TryAddIngredient() called from the controller
    public void TryAddIngredient()
    {
        if (added) return;  // if already added

        GameObject[] lettuces = GameObject.FindGameObjectsWithTag("Lettuce");

        foreach (GameObject lettuce in lettuces)
        {
            float distance = Vector3.Distance(lettuce.transform.position, plate.position);

            if (distance <= revealDistance)
            {
                rend.enabled = true;
                lettuce.SetActive(false);
                added = true;  // mark as added
                return;
            }
        }
    }

    // method for the controller to check if this step is done
    public bool IsAdded() => added;
}