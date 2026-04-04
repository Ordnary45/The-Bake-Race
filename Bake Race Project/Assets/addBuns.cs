// using UnityEngine;

// public class addBuns : MonoBehaviour
// {
//     public GameObject finalTopBun;
//     public GameObject finalBottomBun;
//     public Transform plate;
//     public float revealDistance = 0.25f;

//     private Renderer topRend, bottomRend;

//     private static int bunCount = 0;

//     void Start()
//     {
//         topRend = finalTopBun.GetComponent<Renderer>();
//         bottomRend = finalBottomBun.GetComponent<Renderer>();

//         topRend.enabled = false;
//         bottomRend.enabled = false;
//     }

//     void Update()
//     {
//         if (bunCount >= 2) return;

//         GameObject[] buns = GameObject.FindGameObjectsWithTag("Bun");

//         foreach (GameObject bun in buns)
//         {
//             float distance = Vector3.Distance(bun.transform.position, plate.position);

//             if (distance <= revealDistance)
//             {
//                 if (bunCount == 0)
//                     bottomRend.enabled = true;
//                 else if (bunCount == 1)
//                     topRend.enabled = true;

//                 bunCount++;
//                 bun.SetActive(false);
//                 return;
//             }
//         }
//     }
// }

using UnityEngine;

public class addBuns : MonoBehaviour
{
    public GameObject finalTopBun;
    public GameObject finalBottomBun;
    public Transform plate;
    public float revealDistance = 0.25f;

    private Renderer topRend, bottomRend;
    private int bunCount = 0; // track how many buns have been added
    private bool added = false; // marks if this ingredient step is done

    void Start()
    {
        topRend = finalTopBun.GetComponent<Renderer>();
        bottomRend = finalBottomBun.GetComponent<Renderer>();

        topRend.enabled = false;
        bottomRend.enabled = false;
    }

    // Called by BurgerController to attempt adding a bun
    public void TryAddIngredient()
    {
        if (added) return; // already finished both buns

        GameObject[] buns = GameObject.FindGameObjectsWithTag("Bun");

        foreach (GameObject bun in buns)
        {
            float distance = Vector3.Distance(bun.transform.position, plate.position);

            if (distance <= revealDistance)
            {
                if (bunCount == 0)
                    bottomRend.enabled = true;
                else if (bunCount == 1)
                    topRend.enabled = true;

                bun.SetActive(false);
                bunCount++;

                // If both buns added, mark as done
                if (bunCount >= 2)
                    added = true;

                return;
            }
        }
    }

    // Controller uses this to know when to move to next step
    public bool IsAdded() => added;
}