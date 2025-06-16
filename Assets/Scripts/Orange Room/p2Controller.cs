using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class p2Controller : MonoBehaviour
{
    [SerializeField]
    private p2Buttons[] buttons;
    [SerializeField]
    private Animator doorAnimator; // Assign your door animator in the inspector
    [SerializeField]
    public GameObject interactText; // The text that shows when the player can interact
    [SerializeField]
    private AudioSource buttonPressSound; // The sound to play when a button is pressed
    [SerializeField]
    private AudioSource doorOpenSound; // The sound to play when the door opens

    [SerializeField]
    private int[] correctOrder = { 5, 2, 7, 1 }; // The correct order of button presses
    private List<int> playerOrder = new List<int>(); // The order the player pressed the buttons
    public Collider currentCollider;

    public void ButtonPress()
    {
        if (CheckOrder())
        {
            OpenDoor();
        }
    }

    private bool CheckOrder()
    {
        for (int i = 0; i < correctOrder.Length; i++)
        {
            if (int.Parse(buttons[i].GetComponentInChildren<TextMeshPro>().text) != correctOrder[i])
            {
                return false;
            }
            Debug.Log("Button " + i + " is correct");
        }

        return true;
    }

    private void OpenDoor()
    {
        doorAnimator.SetTrigger("Open");
    }
}