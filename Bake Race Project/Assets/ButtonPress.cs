using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ButtonPress : MonoBehaviour
{
    public string nextSceneName = "SampleScene";  // Name of the next scene
    private bool isPressed = false;

    private Interactable interactable;

    void Start()
    {
        interactable = GetComponent<Interactable>();
        if (interactable == null)
        {
            Debug.LogError("ButtonPress: No Interactable component found on this object!");
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (isPressed) return;

        if (collision.collider.CompareTag("Pressable") || collision.collider.CompareTag("Player"))
        {
            isPressed = true;

            // Trigger the animation
            Animator animator = interactable.GetAnimator();
            if (animator != null)
            {
                animator.SetTrigger(interactable.GetAnimationTriggerName());
            }

            // Load next scene after a 2 seconds so the animation is visible
            StartCoroutine(LoadSceneWithDelay(2f));
        }
    }

    IEnumerator LoadSceneWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(nextSceneName);
    }
}