using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static int gridWidth = 10;
    public static int gridHeight = 20;

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
}
