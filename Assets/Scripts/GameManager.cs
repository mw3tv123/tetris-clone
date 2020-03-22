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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool CheckInsideGrid ( Vector2 position ) {
        return ((int)position.x >= 0 && (int)position.x < gridWidth && (int)position.y >= 0);
    }

    public Vector2 Round ( Vector2 position ) {
        return new Vector2(Mathf.Round(position.x), Mathf.Round(position.y));
    }
}
