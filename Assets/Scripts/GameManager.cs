using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static int gridWidth = 10;
    public static int gridHeight = 20;
    public static Transform[,] grid = new Transform[gridWidth, gridHeight];

    // Start is called before the first frame update
    void Start()
    {
        SpawnNextTetromino();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private string GetRandomTetromino () {
        string randomTetrominoName = "Prefabs/";
        int randomTetromino = Random.Range(1, 8);
        switch (randomTetromino) {
            case 1:
                randomTetrominoName += "Tetromino_J";
                break;
            case 2:
                randomTetrominoName += "Tetromino_L";
                break;
            case 3:
                randomTetrominoName += "Tetromino_Long";
                break;
            case 4:
                randomTetrominoName += "Tetromino_S";
                break;
            case 5:
                randomTetrominoName += "Tetromino_Square";
                break;
            case 6:
                randomTetrominoName += "Tetromino_T";
                break;
            case 7:
                randomTetrominoName += "Tetromino_Z";
                break;
        }
        return randomTetrominoName;
    }

    public bool CheckInsideGrid ( Vector2 position ) {
        return ((int)position.x >= 0 && (int)position.x < gridWidth && (int)position.y >= 0);
    }

    public Vector2 Round ( Vector2 position ) {
        return new Vector2(Mathf.Round(position.x), Mathf.Round(position.y));
    }

    public void SpawnNextTetromino () {
        GameObject nextTetromino = (GameObject)Instantiate(Resources.Load(GetRandomTetromino(), typeof(GameObject)),
                                                           new Vector2(4.0f, 17.0f),
                                                           Quaternion.identity);
    }

    public void UpdateGrid (TetrominoController tetromino) {
        for ( int y = 0; y < gridHeight; y++ ) {
            for ( int x = 0; x < gridWidth; x++ ) {
                if ( grid[x, y] != null ) {
                    if ( grid[x, y].parent == tetromino.transform ) {
                        grid[x, y] = null;
                    }
                }
            }
        }

        foreach ( Transform mino in tetromino.transform ) {
            Vector2 minoPosition = Round(mino.position);
            if ( minoPosition.y < gridHeight ) {
                grid[(int)minoPosition.x, (int)minoPosition.y] = mino;
            }
        }
    }

    public Transform GetMinoAtGridPosition ( Vector2 position ) {
        if ( position.y > gridHeight - 1 )
            return null;
        else 
            return grid[(int)position.x, (int)position.y];
    }
}
