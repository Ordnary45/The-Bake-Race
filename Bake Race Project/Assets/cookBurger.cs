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
        // if patty is on the pan AND is not yet cooked
        if (touchingPan && !isCooked)
        {
            timer += Time.deltaTime;

            // obtaining timer values
            float remaining = cookTime - timer;
            int minutes = Mathf.FloorToInt(remaining / 60f);
            int seconds = Mathf.FloorToInt(remaining % 60f);
            int milliseconds = Mathf.FloorToInt((remaining * 1000f) % 1000f);

            // displaying timer text
            timerText.text = string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds);

            // color the timer green once cooking has completed
            if (timer >= cookTime)
            {
                rend.material = cookedMaterial;     // update patty material from raw to cooked
                isCooked = true;                    // set cooked flag to true
                timerText.text = "00:00:000";       // keep timer at zeroes
                timerText.color = Color.green;      // color timer text green
            }
        }
    }

    // called when patty comes into contact with pan
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Pan"))
        {
            touchingPan = true;
        }
    }

    // if patty exits pan prematurely, restart cooking progress
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