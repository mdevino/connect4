using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SrcGrid : MonoBehaviour
{
    private int playerTurn;
    private int[,] grid;

    private GameObject[] coins;
    private int coinsAdded;

    private int columns = 7;
    private int rows = 6;

    private SpriteRenderer spriteRenderer;

    public GameObject coin1;
    public GameObject coin2;



    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        StartGame();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Translating pixels to game unities
            Vector3 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouse.z = 0f;
            if (spriteRenderer.bounds.Contains(mouse))
            {
                placeCoin(mouse, playerTurn);
                switchPlayers();
            }
        }
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

    private void placeCoin(Vector3 mouse, int player)
    {
        Vector3 objectPos = mouse;
        GameObject coin = playerTurn == 1 ? coin1 : coin2;
        Instantiate(coin, objectPos, Quaternion.identity);
        coins[coinsAdded] = coin;
        coinsAdded++;

        Debug.Log(
            "Player " + playerTurn + " has placed a coin!" + 
            " Mouse: x=" + mouse.x + ", y=" + mouse.y + 
            "Object: x=" + objectPos.x + ", y=" + objectPos.y
            );
    }

    private void StartGame()
    {
        if(coinsAdded > 0)
            DeleteCoins();
        coins = new GameObject[columns * rows];
        // By default, int array values are set to 0 in C#
        grid = new int[columns, rows];
        playerTurn = 1;
    }

    private void DeleteCoins()
    {
        foreach(GameObject coin in coins)
        {
            Destroy(coin);
        }
        coinsAdded = 0;
    }
}
