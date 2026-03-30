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
         // Keep UI above patty -- commented out to mount timer on wall
        // if (timerText != null)
        // {
        //     timerText.transform.position = transform.position + Vector3.up * 1f;
        // }

        // Handle cooking here instead of OnCollisionStay
        if (touchingPan && !isCooked)
        {
            timer += Time.deltaTime;

            // float remaining = cookTime - timer;
            // timerText.text = remaining.ToString("F1") + "s";

            float remaining = cookTime - timer;

            int minutes = Mathf.FloorToInt(remaining / 60f);
            int seconds = Mathf.FloorToInt(remaining % 60f);
            int milliseconds = Mathf.FloorToInt((remaining * 1000f) % 1000f);

            timerText.text = string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds);

            if (timer >= cookTime)
            {
                rend.material = cookedMaterial;
                isCooked = true;
                timerText.text = "00:00:000";
                timerText.color = Color.green;
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