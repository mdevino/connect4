using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SrcGrid : MonoBehaviour
{
    private int playerTurn;
    private int[,] grid;
    private DateTime lastPlaced;
    private int delay = 1;
    
    public Text text;
    public Color player1Color;
    public Color player2Color;
    public Color neuterColor;

    private GameObject[] coins;
    private int coinsAdded;
    private int columns = 7;
    private int rows = 6;
    private int winner;

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
                if (winner == 0 && (long)(DateTime.Now - lastPlaced).TotalSeconds > delay-1)
                {
                    placeCoin(mouse);
                    SwitchPlayers();
                    SetText();
                }
            }
        }
    }

    private void SwitchPlayers()
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

        for(int row = rows-1; row >= 0; row--)
        {
            if(grid[row, column] == 0)
            {
                grid[row, column] = playerTurn;
                break;
            } else if(row == 0)
            {
                return;
            }
        }

        GameObject coin = playerTurn == 1 ? coin1 : coin2;
        Instantiate(coin, newCoinPosition, Quaternion.identity);
        coins[coinsAdded] = coin;
        coinsAdded++;

        if (AreFourConnected(playerTurn))
        {
            winner = playerTurn;
            Debug.Log("Player " + winner + " wins");
        }

        if(winner == 0 && coins.Length > columns * rows)
        {
            winner = -1;
        }

        lastPlaced = DateTime.Now;
        // Debug.Log("Player " + playerTurn + " has placed a coin!  Mouse: x=" + mouse.x + ", y=" + mouse.y + "Object: x=" + newCoinPosition.x + ", y=" + newCoinPosition.y);
        Debug.Log(GridToString());
    }

    private void StartGame()
    {
        DeleteCoins();
        // By default, int array values are set to 0 in C#
        grid = new int[rows, columns];
        playerTurn = 1;
        winner = 0;
        SetText();
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

    private string GridToString()
    {
        string gridString = "";
        for (int row = 0; row < rows; row++)
        {
            for (int column = 0; column < columns; column++)
            {
                gridString += grid[row, column] + "\t";
            }
            gridString += "\n";
        }
        return gridString;
    }

    private bool AreFourConnected(int player)
    {

        // horizontalCheck 
        for (int j = 0; j < columns - 3; j++)
        {
            for (int i = 0; i <  rows; i++)
            {
                if (grid[i,j] == player && grid[i,j + 1] == player && grid[i,j + 2] == player && grid[i,j + 3] == player)
                {
                    return true;
                }
            }
        }
        // verticalCheck
        for (int i = 0; i <  rows - 3; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                if (grid[i,j] == player && grid[i + 1,j] == player && grid[i + 2,j] == player && grid[i + 3,j] == player)
                {
                    return true;
                }
            }
        }
        // ascendingDiagonalCheck 
        for (int i = 3; i <  rows; i++)
        {
            for (int j = 0; j < columns - 3; j++)
            {
                if (grid[i,j] == player && grid[i - 1,j + 1] == player && grid[i - 2,j + 2] == player && grid[i - 3,j + 3] == player)
                    return true;
            }
        }
        // descendingDiagonalCheck
        for (int i = 3; i <  rows; i++)
        {
            for (int j = 3; j < columns; j++)
            {
                if (grid[i,j] == player && grid[i - 1,j - 1] == player && grid[i - 2,j - 2] == player && grid[i - 3,j - 3] == player)
                    return true;
            }
        }
        return false;
    }

    private void SetText()
    {

        if (text != null)
        {
            string message;
            if (winner < 0)
            {
                text.color = neuterColor;
                message = "Draw";
            }
            else if (winner == 0)
            {
                text.color = playerTurn == 1 ? player1Color : player2Color;
                message = "Player " + playerTurn + "'s turn";
            }
            else
            {
                text.color = playerTurn == 1 ? player2Color : player1Color;
                message = "Player " + winner + " wins";
            }

            text.text = message;
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
