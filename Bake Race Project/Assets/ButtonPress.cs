using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ButtonPress : MonoBehaviour
{
    public string nextSceneName = "SampleScene";  // Scene to load after button press
    private bool isPressed = false;

    private Interactable interactable;

    void Start()
    {
        interactable = GetComponent<Interactable>(); // Get Interactable component on this object
        if (interactable == null)
        {
            Debug.LogError("ButtonPress: No Interactable component found on this object!"); // Error if missing
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (isPressed) return; // Prevent multiple triggers

        // Check if the colliding object is allowed to press the button
        if (collision.collider.CompareTag("Pressable") || collision.collider.CompareTag("Player"))
        {
            isPressed = true;

            // Trigger the button animation
            Animator animator = interactable.GetAnimator();
            if (animator != null)
            {
                animator.SetTrigger(interactable.GetAnimationTriggerName());
            }

            // Delay scene load so animation can play
            StartCoroutine(LoadSceneWithDelay(2f));
        }
    }

    IEnumerator LoadSceneWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay); // Wait before loading scene
        SceneManager.LoadScene(nextSceneName);  // Load the next scene
    }
}