using System;
using System.Collections.Generic;
using UnityEngine;

public class p3Controller : MonoBehaviour
{
    [SerializeField]
    private Animator[] buttonAnimators;

    [SerializeField]
    private Animator doorAnimator;

    private List<int> pressedButtons = new List<int> { 0, 10, 11 };
    private List<int> currentSequence = new List<int>();

    public void ButtonPress(int buttonIndex)
    {
        buttonAnimators[buttonIndex].SetTrigger("Press");

        if (pressedButtons.Contains(buttonIndex) && !currentSequence.Contains(buttonIndex))
        {
            currentSequence.Add(buttonIndex);

            if (CheckConsecutivePresses())
            {
                OpenDoor();
            }
        }
    }

    private bool CheckConsecutivePresses()
    {
        if (currentSequence.Count == pressedButtons.Count)
        {
            // sort the lists to check regardless of order
            currentSequence.Sort();
            pressedButtons.Sort();

            for (int i = 0; i < pressedButtons.Count; i++)
            {
                if (currentSequence[i] != pressedButtons[i])
                {
                    currentSequence.Clear();
                    return false;
                }
            }

            currentSequence.Clear();
            return true;
        }
        return false;
    }

    private void OpenDoor()
    {
        doorAnimator.SetTrigger("Open");
    }
}
