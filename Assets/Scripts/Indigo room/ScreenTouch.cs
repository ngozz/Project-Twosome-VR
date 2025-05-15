using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ScreenTouch : MonoBehaviour
{
    public GameObject Screen;

    PlayerInputActions inputAction;

    public UnityEvent myAction; 

    private void Awake()
    {
        inputAction = new PlayerInputActions();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
            LeanTween.scale(Screen, Vector3.one, 2).setEaseInBounce();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
            LeanTween.scale(Screen, Vector3.zero, 2).setEaseInQuad();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (inputAction.Player.Interact.WasPerformedThisFrame())
            myAction.Invoke();
        }
    }



    public void OnEnable()
    {
        inputAction.Player.Enable();
    }

    public void OnDisable()
    {
        inputAction.Player.Disable();
    }
}
