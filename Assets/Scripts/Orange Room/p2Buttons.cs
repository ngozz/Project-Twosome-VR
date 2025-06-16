using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class p2Buttons : MonoBehaviour
{
    [SerializeField]
    private int startNumber = 1; // The index of this button
    [SerializeField]
    private p2Controller puzzleController; // Reference to the PuzzleController
    private bool interactable = false; // Whether the player can interact with the buttons
    private TextMeshPro buttonText; // Reference to the TextMeshPro component

    PlayerInputActions playerInputActions;

    void Start()
    {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();
        buttonText = GetComponentInChildren<TextMeshPro>();
        UpdateButtonText();
    }

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
        if (other.CompareTag("MainCamera") && puzzleController.currentCollider == null)
        {
            puzzleController.currentCollider = GetComponent<Collider>();
            puzzleController.interactText.SetActive(true);
            interactable = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("MainCamera") && puzzleController.currentCollider == GetComponent<Collider>())
        {
            puzzleController.currentCollider = null;
            puzzleController.interactText.SetActive(false);
            interactable = false;
        }
    }

    public void OnButtonPressed()
    {
        // if (puzzleController.currentCollider == GetComponent<Collider>())
        // {
            startNumber++;
            if (startNumber > 7)
            {
                startNumber = 1;
            }
            UpdateButtonText();
            puzzleController.ButtonPress();
            puzzleController.currentCollider = null;
            puzzleController.interactText.SetActive(false);
            interactable = false;
        // }
    }

    private void UpdateButtonText()
    {
        buttonText.text = startNumber.ToString();
    }
}