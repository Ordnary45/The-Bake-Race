// using UnityEngine;

// public class addPatty : MonoBehaviour
// {
//     public GameObject finalPatty;
//     public Transform plate;
//     public float revealDistance = 0.25f; //1.0f;

//     private Renderer rend;

//     void Start()
//     {
//         rend = finalPatty.GetComponent<Renderer>();
//         rend.enabled = false;
//     }

//     void Update()
//     {
//         if (rend.enabled) return;

//         GameObject[] buns = GameObject.FindGameObjectsWithTag("Patty");

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

public class addPatty : MonoBehaviour
{
    public GameObject finalPatty;
    public Transform plate;
    public float revealDistance = 0.25f;

    private Renderer rend;
    private bool added = false; // new flag

    void Start()
    {
        rend = finalPatty.GetComponent<Renderer>();
        rend.enabled = false;
    }

    // new method called by the controller
    public void TryAddIngredient()
    {
        if (added) return;  // already added

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