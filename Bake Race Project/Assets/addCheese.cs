// using UnityEngine;

// public class addCheese : MonoBehaviour
// {
//     public GameObject finalCheese;
//     public Transform plate;
//     public float revealDistance = 0.25f; //1.0f;

//     private Renderer rend;

//     void Start()
//     {
//         rend = finalCheese.GetComponent<Renderer>();
//         rend.enabled = false;
//     }

//     void Update()
//     {
//         if (rend.enabled) return;

//         GameObject[] buns = GameObject.FindGameObjectsWithTag("Cheese");

//         foreach (GameObject bun in buns)
//         {
//             float distance = Vector3.Distance(bun.transform.position, plate.position);

//             if (distance <= revealDistance)
//             {
//                 rend.enabled = true;
//                 bun.SetActive(false);
//                 return;
//             }
//         }
//     }
// }

using UnityEngine;

public class addCheese : MonoBehaviour
{
    public GameObject finalCheese;
    public Transform plate;
    public float revealDistance = 0.25f;

    private Renderer rend;
    private bool added = false; // new flag

    void Start()
    {
        rend = finalCheese.GetComponent<Renderer>();
        rend.enabled = false;
    }

    // new method called by the controller
    public void TryAddIngredient()
    {
        if (added) return;  // already added

        GameObject[] cheeses = GameObject.FindGameObjectsWithTag("Cheese");

        foreach (GameObject cheese in cheeses)
        {
            float distance = Vector3.Distance(cheese.transform.position, plate.position);

            if (distance <= revealDistance)
            {
                rend.enabled = true;
                cheese.SetActive(false);
                added = true;  // mark as added
                return;
            }
        }
    }

    // method for the controller to check if this step is done
    public bool IsAdded() => added;
}