using System.Collections.Generic;
using UnityEngine;

public class p1Controller : MonoBehaviour
{
    [SerializeField]
    private Animator[] buttonAnimators; // Assign your button animators in the inspector
    [SerializeField]
    private Animator doorAnimator; // Assign your door animator in the inspector
    [SerializeField]
    public GameObject interactText; // The text that shows when the player can interact

    [SerializeField]
    private int[] correctOrder = { 3, 0, 2, 1 }; // The correct order of button presses
    private List<int> playerOrder = new List<int>(); // The order the player pressed the buttons
    public Collider currentCollider;

    public void ButtonPress(int buttonIndex)
    {
        buttonAnimators[buttonIndex].SetTrigger("Press");
        playerOrder.Add(buttonIndex);

        if (playerOrder.Count == 4)
        {
            CheckOrder();
        }
    }

    private void CheckOrder()
    {
        for (int i = 0; i < correctOrder.Length; i++)
        {
            if (playerOrder[i] != correctOrder[i])
            {
                ResetButtons();
                return;
            }
        }

        OpenDoor();
    }

    private void ResetButtons()
    {
        foreach (Animator animator in buttonAnimators)
        {
            animator.SetTrigger("Reset");
        }

        playerOrder.Clear();
    }

    private void OpenDoor()
    {
        doorAnimator.SetTrigger("Open");
    }
}
