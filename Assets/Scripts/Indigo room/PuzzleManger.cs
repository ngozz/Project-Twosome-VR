using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

public class PuzzleManger : MonoBehaviour
{

    public GameObject[] puzzlePieceHolder1;
    public GameObject[] puzzlePieceHolder2;
    public GameObject[] puzzlePieceHolder3;
    public UnityEvent chickenDinner;
    public GameObject clue;

    private int redCounter;
    private int greenCounter;
    private int yellowCounter;

    public void Winner()
    {
        if (puzzlePieceHolder1[0].activeInHierarchy && puzzlePieceHolder2[1].activeInHierarchy && puzzlePieceHolder3[2].activeInHierarchy)
        {
            Debug.Log("You Win");
            clue.SetActive(true);
            chickenDinner.Invoke();
        }
    }


    public void RedScreen()
    {
        redCounter++;

        switch (redCounter)
        {
            case 0:
                puzzlePieceHolder1[0].SetActive(true);
                puzzlePieceHolder1[1].SetActive(false);
                puzzlePieceHolder1[2].SetActive(false);
                break;
            case 1:
                puzzlePieceHolder1[0].SetActive(false);
                puzzlePieceHolder1[1].SetActive(true);
                puzzlePieceHolder1[2].SetActive(false);
                break;
            case 2:
                puzzlePieceHolder1[0].SetActive(false);
                puzzlePieceHolder1[1].SetActive(false);
                puzzlePieceHolder1[2].SetActive(true);
                break;
            case 3:
                puzzlePieceHolder1[0].SetActive(true);
                puzzlePieceHolder1[1].SetActive(false);
                puzzlePieceHolder1[2].SetActive(false);
                redCounter = 0;
                break;
            default:
                break;
        }
    }

    public void GreenScreen()
    {
        greenCounter++;

        switch (greenCounter)
        {
            case 0:
                puzzlePieceHolder2[0].SetActive(true);
                puzzlePieceHolder2[1].SetActive(false);
                puzzlePieceHolder2[2].SetActive(false);
                break;
            case 1:
                puzzlePieceHolder2[0].SetActive(false);
                puzzlePieceHolder2[1].SetActive(true);
                puzzlePieceHolder2[2].SetActive(false);
                break;
            case 2:
                puzzlePieceHolder2[0].SetActive(false);
                puzzlePieceHolder2[1].SetActive(false);
                puzzlePieceHolder2[2].SetActive(true);
                break;
            case 3:
                puzzlePieceHolder2[0].SetActive(true);
                puzzlePieceHolder2[1].SetActive(false);
                puzzlePieceHolder2[2].SetActive(false);
                greenCounter = 0;
                break;
            default:
                break;
        }
    }

    public void YellowScreen()
    {
        yellowCounter++;

        switch (yellowCounter)
        {
            case 0:
                puzzlePieceHolder3[0].SetActive(true);
                puzzlePieceHolder3[1].SetActive(false);
                puzzlePieceHolder3[2].SetActive(false);
                break;
            case 1:
                puzzlePieceHolder3[0].SetActive(false);
                puzzlePieceHolder3[1].SetActive(true);
                puzzlePieceHolder3[2].SetActive(false);
                break;
            case 2:
                puzzlePieceHolder3[0].SetActive(false);
                puzzlePieceHolder3[1].SetActive(false);
                puzzlePieceHolder3[2].SetActive(true);
                break;
            case 3:
                puzzlePieceHolder3[0].SetActive(true);
                puzzlePieceHolder3[1].SetActive(false);
                puzzlePieceHolder3[2].SetActive(false);
                yellowCounter = 0;
                break;
            default:
                break;
        }
    }
}
