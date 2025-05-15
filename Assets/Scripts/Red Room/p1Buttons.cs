using UnityEngine;
using UnityEngine.InputSystem;

public class p1Buttons : MonoBehaviour
{
    [SerializeField]
    private int buttonIndex; // The index of this button
    [SerializeField]
    private p1Controller puzzleController; // Reference to the PuzzleController
    private bool interactable = false; // Whether the player can interact with the buttons

    PlayerInputActions playerInputActions;

    void Start()
    {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();
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

    void OnTriggerEnter(Collider other)
    {
        Animator animator = GetComponent<Animator>();
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        if (other.CompareTag("MainCamera") && puzzleController.currentCollider == null && stateInfo.IsName("Default"))
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
        if (puzzleController.currentCollider == GetComponent<Collider>())
        {
            puzzleController.ButtonPress(buttonIndex);
            puzzleController.currentCollider = null;
            puzzleController.interactText.SetActive(false);
            interactable = false;
        }
    }
}
