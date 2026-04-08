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

            // COOKED state (1:00 -> 1:10)
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