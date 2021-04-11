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

    private float cellWidth;
    private float x0;
    private float y0;
    private float xf;
    private float yf;

    private SpriteRenderer spriteRenderer;

    public GameObject coin1;
    public GameObject coin2;



    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        Vector3 size = spriteRenderer.bounds.size;
        Vector3 center = spriteRenderer.bounds.center;
        cellWidth = size.x / columns;
        x0 = center.x - size.x / 2;
        xf = center.x + size.x / 2;
        y0 = center.y - size.y / 2;
        yf = center.y + size.y / 2;

        Debug.Log("x0=" + x0 + " xf=" + xf + " y0=" + y0 + " yf=" + yf);

        StartGame();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Translating pixels to game unities
            Vector3 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouse.z = gameObject.transform.position.z;
            if (spriteRenderer.bounds.Contains(mouse))
            {
                placeCoin(mouse);
                switchPlayers();
            }
        }
    }

    private void switchPlayers()
    {
        playerTurn = playerTurn == 1 ? 2 : 1;
    }

    private void placeCoin(Vector3 mouse)
    {
        Vector3 newCoinPosition = new Vector3();
        int column = GetColumn(mouse);
        newCoinPosition.x = x0 + (column * cellWidth) + (cellWidth / 2);
        newCoinPosition.y = yf - cellWidth / 2;
        newCoinPosition.z = gameObject.transform.position.z;

        Vector3 objectPos = mouse;
        GameObject coin = playerTurn == 1 ? coin1 : coin2;
        Instantiate(coin, newCoinPosition, Quaternion.identity);
        coins[coinsAdded] = coin;
        coinsAdded++;

        Debug.Log("Player " + playerTurn + " has placed a coin!  Mouse: x=" + mouse.x + ", y=" + mouse.y + "Object: x=" + newCoinPosition.x + ", y=" + newCoinPosition.y);

    }

    private void StartGame()
    {
        DeleteCoins();
        // By default, int array values are set to 0 in C#
        grid = new int[columns, rows];
        playerTurn = 1;
    }

    private void DeleteCoins()
    {
        if (coins != null && coins.Length != 0)
        {
            foreach (GameObject coin in coins)
            {
                Destroy(coin);
            }
        }
        coins = new GameObject[columns * rows];
        coinsAdded = 0;
    }

    private int GetColumn(Vector3 mouse)
    {
        int columnIndex = 0;
        float columnStart = x0;
        float columnEnd = x0 + cellWidth;
        while(columnIndex < columns)
        {
            if (columnStart < mouse.x && mouse.x < columnEnd)
            {
                return columnIndex;
            }
            else
            {
                columnStart = columnEnd;
                columnEnd = columnEnd + cellWidth;
                columnIndex++;
            }
        }
        return -1;
    }
}
