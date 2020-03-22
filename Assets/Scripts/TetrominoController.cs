﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrominoController : MonoBehaviour
{
    Vector3 rotationAngle = new Vector3(0, 0, 90);

    float fall = 0f;
    public float fallSpeed = 1f;

    public bool allowRotation = true;
    public bool limitRotation = false;

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        CheckUserInput();
    }

    void CheckUserInput() {
        // Move tetromino to the right.
        if ( Input.GetKeyDown(KeyCode.RightArrow) ) {
            transform.position += new Vector3(1, 0, 0);
            
            if ( !IsValidPosition() )
                transform.position += new Vector3(-1, 0, 0);
        }
        // Move tetromino to the left.
        else if ( Input.GetKeyDown(KeyCode.LeftArrow) ) {
            transform.position += new Vector3(-1, 0, 0);

            if ( !IsValidPosition() )
                transform.position += new Vector3(1, 0, 0);
        }
        // Rotation tetromino.
        else if ( Input.GetKeyDown(KeyCode.UpArrow) && allowRotation ) {
            // Tetromino like Z and I only need to rotate in range of (0 -> 90) degree.
            if ( limitRotation )
                rotationAngle.z = transform.rotation.eulerAngles.z >= 90 ? -90 : 90;

            // Rotate this tetromino.
            transform.Rotate(rotationAngle);

            // If tetromino is out of grid border, rotate back to original position.
            if ( !IsValidPosition() )
                transform.Rotate(0, 0, -rotationAngle.z);
        }
        // Move tetromino down.
        else if ( Input.GetKeyDown(KeyCode.DownArrow) || Time.time - fall >= fallSpeed ) {
            transform.position += new Vector3(0, -1, 0);
            fall = Time.time;

            if ( !IsValidPosition() ) {
                transform.position += new Vector3(0, 1, 0);
                
                // Disable this tetromino controller and spawn another piece.
                enabled = false;
                FindObjectOfType<GameManager>().SpawnNextTetromino();
            }
        }
    }

    bool IsValidPosition () {
        foreach ( Transform mino in transform ) {
            Vector2 position = FindObjectOfType<GameManager>().Round(mino.position);

            if ( !FindObjectOfType<GameManager>().CheckInsideGrid(position) )
                return false;
        }
        return true;
    }
}
