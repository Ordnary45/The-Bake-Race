// using UnityEngine;
// using TMPro;

// public class cookBurger : MonoBehaviour
// {
//     public Material cookedMaterial;
//     public float cookTime = 5f;

//     public TextMeshProUGUI timerText;

//     private Renderer rend;
//     private float timer = 0f;
//     private bool isCooked = false;

//     void Start()
//     {
//         rend = GetComponent<Renderer>();
//     }

//     void Update()
//     {
//         if (timerText != null)
//         {
//             timerText.transform.position = transform.position + Vector3.up * 1f;
//         }
//     }

//     void OnCollisionStay(Collision collision)
//     {
//         if (collision.gameObject.CompareTag("Pan") && !isCooked)
//         {
//             timer += Time.deltaTime;

//             // Update UI
//             float remaining = cookTime - timer;
//             timerText.text = remaining.ToString("F1") + "s";

//             if (timer >= cookTime)
//             {
//                 rend.material = cookedMaterial;
//                 isCooked = true;
//                 timerText.text = "Done!";
//             }
//         }
//     }

//     void OnCollisionExit(Collision collision)
//     {
//         if (collision.gameObject.CompareTag("Pan") && !isCooked)
//         {
//             timer = 0f;
//             timerText.text = "";
//         }
//     }
// }

using UnityEngine;
using TMPro;

public class cookBurger : MonoBehaviour
{
    public Material cookedMaterial;
    public float cookTime = 5f;

    public TextMeshProUGUI timerText;

    private Renderer rend;
    private float timer = 0f;
    private bool isCooked = false;
    private bool touchingPan = false;

    void Start()
    {
        rend = GetComponent<Renderer>();
    }

    void Update()
    {
        // Keep UI above patty
        if (timerText != null)
        {
            timerText.transform.position = transform.position + Vector3.up * 1f;
        }

        // Handle cooking here instead of OnCollisionStay
        if (touchingPan && !isCooked)
        {
            timer += Time.deltaTime;

            float remaining = cookTime - timer;
            timerText.text = remaining.ToString("F1") + "s";

            if (timer >= cookTime)
            {
                rend.material = cookedMaterial;
                isCooked = true;
                timerText.text = "Burger cooked!";
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Pan"))
        {
            touchingPan = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Pan") && !isCooked)
        {
            touchingPan = false;
            timer = 0f;
            timerText.text = "";
        }
    }
}