using UnityEngine;
using TMPro;

public class cookBurger : MonoBehaviour
{
    public Material cookedMaterial;
    public Material burntMaterial;

    public float cookTime = 60f;      // 1 minute to cooked
    public float burnTime = 70f;      // 1 min 10 sec = burnt threshold

    public TextMeshProUGUI timerText;       // timer text UI element

    private Renderer rend;
    private float timer = 0f;
    private bool touchingPan = false;

    // flags for cooking states
    private bool isCooked = false;
    private bool isBurnt = false;

    void Start()
    {
        rend = GetComponent<Renderer>();    // obtain renderer
    }

    void Update()
    {
        if (touchingPan)        // if patty comes into contact with pan
        {
            timer += Time.deltaTime;

            // format time (counting UP)
            int minutes = Mathf.FloorToInt(timer / 60f);
            int seconds = Mathf.FloorToInt(timer % 60f);
            int milliseconds = Mathf.FloorToInt((timer * 1000f) % 1000f);

            // formatting timer to be in 00:00:000 format
            timerText.text = string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds);

            // COOKED state (1:00 -> 1:10)
            if (timer >= cookTime && timer <= burnTime && !isCooked)
            {
                rend.material = cookedMaterial;     // turn patty brown when cooked
                timerText.color = Color.green;      // green text for cooked

                isCooked = true;                    // setting flags
                isBurnt = false;
            }

            // BURNT state (after 1:10)
            if (timer > burnTime && !isBurnt)
            {
                rend.material = burntMaterial;      // turn patty black when burnt
                timerText.color = Color.red;        // red text for burnt

                isBurnt = true;                     // setting burnt flag
            }
        }
    }

    void OnCollisionEnter(Collision collision)      // function used for tracking the patty collision with the pan
    {
        if (collision.gameObject.CompareTag("Pan"))
        {
            touchingPan = true;
        }
    }

    void OnCollisionExit(Collision collision)       // function used for tracking collision end
    {
        if (collision.gameObject.CompareTag("Pan"))
        {
            touchingPan = false;

            // reset everything if removed early
            timer = 0f;
            isCooked = false;
            isBurnt = false;

            timerText.text = "";                    // reset UI
            timerText.color = Color.white;
        }
    }
}