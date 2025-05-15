using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDoorController : MonoBehaviour
{
    [SerializeField] private Animator myDoor = null;
    [SerializeField] private bool openTrig = false;
    [SerializeField] private bool closeTrig = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (openTrig)
            {
                myDoor.Play("p5DoorOpen");
                gameObject.SetActive(false);
            }
            else if (closeTrig)
            {
                myDoor.Play("p5DoorClose");
                gameObject.SetActive(false);
            }
        }
    }
}
