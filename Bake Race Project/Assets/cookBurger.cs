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
//     private bool touchingPan = false;

//     void Start()
//     {
//         rend = GetComponent<Renderer>();
//     }

//     void Update()
//     {
//         // if patty is on the pan AND is not yet cooked
//         if (touchingPan && !isCooked)
//         {
//             timer += Time.deltaTime;

//             // obtaining timer values
//             float remaining = cookTime - timer;
//             int minutes = Mathf.FloorToInt(remaining / 60f);
//             int seconds = Mathf.FloorToInt(remaining % 60f);
//             int milliseconds = Mathf.FloorToInt((remaining * 1000f) % 1000f);

//             // displaying timer text
//             timerText.text = string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds);

//             // color the timer green once cooking has completed
//             if (timer >= cookTime)
//             {
//                 rend.material = cookedMaterial;     // update patty material from raw to cooked
//                 isCooked = true;                    // set cooked flag to true
//                 timerText.text = "00:00:000";       // keep timer at zeroes
//                 timerText.color = Color.green;      // color timer text green
//             }
//         }
//     }

//     // called when patty comes into contact with pan
//     void OnCollisionEnter(Collision collision)
//     {
//         if (collision.gameObject.CompareTag("Pan"))
//         {
//             touchingPan = true;
//         }
//     }

//     // if patty exits pan prematurely, restart cooking progress
//     void OnCollisionExit(Collision collision)
//     {
//         if (collision.gameObject.CompareTag("Pan") && !isCooked)
//         {
//             touchingPan = false;
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
    public Material burntMaterial;

    public float cookTime = 60f;      // 1 minute to cooked
    public float burnTime = 70f;      // 1 min 10 sec = burnt threshold

    public TextMeshProUGUI timerText;

    private Renderer rend;
    private float timer = 0f;
    private bool touchingPan = false;

    private bool isCooked = false;
    private bool isBurnt = false;

    void Start()
    {
        rend = GetComponent<Renderer>();
    }

    void Update()
    {
        if (touchingPan)
        {
            timer += Time.deltaTime;

            // format time (counting UP)
            int minutes = Mathf.FloorToInt(timer / 60f);
            int seconds = Mathf.FloorToInt(timer % 60f);
            int milliseconds = Mathf.FloorToInt((timer * 1000f) % 1000f);

            timerText.text = string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds);

            // COOKED state (1:00 → 1:10)
            if (timer >= cookTime && timer <= burnTime && !isCooked)
            {
                rend.material = cookedMaterial;
                timerText.color = Color.green;

                isCooked = true;
                isBurnt = false;
            }

            // BURNT state (after 1:10)
            if (timer > burnTime && !isBurnt)
            {
                rend.material = burntMaterial;
                timerText.color = Color.red;

                isBurnt = true;
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
        if (collision.gameObject.CompareTag("Pan"))
        {
            touchingPan = false;

            // reset everything if removed early
            timer = 0f;
            isCooked = false;
            isBurnt = false;

            timerText.text = "";
            timerText.color = Color.white;
        }
    }
}