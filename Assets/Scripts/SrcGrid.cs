using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SrcGrid : MonoBehaviour
{
    private int playerTurn;
    private int[,] matrix;


    // Start is called before the first frame update
    void Start()
    {
        clearGrid();
        playerTurn = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Player " + playerTurn + "'s turn");
            placeCoin(Input.mousePosition.x, Input.mousePosition.y);
            switchPlayers();
        }
    }

    private void clearGrid()
    {
        matrix = new int[6, 7] {
            { 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0 }
        };
    }

    private void switchPlayers()
    {
        if (playerTurn == 1)
        {
            playerTurn = 2;
        }
        else
        {
            playerTurn = 1;
        }
    }

    private void placeCoin(float x, float y)
    {
        Debug.Log("Player " + playerTurn + " has placed a coin at x=" + x + ", y=" + y);
    }
}
