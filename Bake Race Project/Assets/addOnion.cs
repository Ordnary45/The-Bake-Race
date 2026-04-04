// using UnityEngine;

// public class addOnion : MonoBehaviour
// {
//     public GameObject finalOnion;
//     public Transform plate;
//     public float revealDistance = 0.25f; //1.0f;

//     private Renderer rend;

//     void Start()
//     {
//         rend = finalOnion.GetComponent<Renderer>();
//         rend.enabled = false;
//     }

//     void Update()
//     {
//         if (rend.enabled) return;

//         GameObject[] buns = GameObject.FindGameObjectsWithTag("Onion");

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

public class addOnion : MonoBehaviour
{
    public GameObject finalOnion;
    public Transform plate;
    public float revealDistance = 0.25f;

    private Renderer rend;
    private bool added = false; // new flag

    void Start()
    {
        rend = finalOnion.GetComponent<Renderer>();
        rend.enabled = false;
    }

    // new method called by the controller
    public void TryAddIngredient()
    {
        if (added) return;  // already added

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