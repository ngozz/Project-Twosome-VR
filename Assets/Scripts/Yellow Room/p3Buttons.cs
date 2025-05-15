using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class p3Buttons : MonoBehaviour
{
    [SerializeField]
    private Animator buttonAnimators;
    [SerializeField]
    private p3Controller puzzleController;
    [SerializeField]
    private int buttonIndex;

    private bool interactable = false; // Whether the player can interact with the buttons

    PlayerInputActions playerInputActions;

    // [SerializeField]
    // private GameObject interactText;

    void Start()
    {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (interactable)
        {
            if (playerInputActions.Player.Interact.triggered)
            {
                OnButtonPressed();
            }
        }
    }

    
    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("MainCamera"))
        {
            // interactText.SetActive(true);
            interactable = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("MainCamera"))
        {
            // interactText.SetActive(false);
            interactable = false;
        }
    }

    void OnButtonPressed()
    {

        
        
           puzzleController.ButtonPress(buttonIndex);
        
    }
}
