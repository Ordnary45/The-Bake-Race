// using UnityEngine;

// public class addLettuce : MonoBehaviour
// {
//     public GameObject finalLettuce;
//     public Transform plate;
//     public float revealDistance = 0.25f; //1.0f;

//     private Renderer rend;

//     void Start()
//     {
//         rend = finalLettuce.GetComponent<Renderer>();
//         rend.enabled = false;
//     }

//     void Update()
//     {
//         if (rend.enabled) return;

//         GameObject[] buns = GameObject.FindGameObjectsWithTag("Lettuce");

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

public class addLettuce : MonoBehaviour
{
    public GameObject finalLettuce;
    public Transform plate;
    public float revealDistance = 0.25f;

    private Renderer rend;
    private bool added = false; // new flag

    void Start()
    {
        rend = finalLettuce.GetComponent<Renderer>();
        rend.enabled = false;
    }

    // new method called by the controller
    public void TryAddIngredient()
    {
        if (added) return;  // already added

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