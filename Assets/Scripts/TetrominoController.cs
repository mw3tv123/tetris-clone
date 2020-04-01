using UnityEngine;

public class TetrominoController : MonoBehaviour
{
    Vector3 rotationAngle = new Vector3(0, 0, 90);
    GameManager gameManager;

    float fall = 0f;
    public float fallSpeed = 1f;

    public bool allowRotation = true;
    public bool limitRotation = false;

    // Start is called before the first frame update
    void Start() {
        gameManager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update() {
        CheckUserInput();
    }

    void CheckUserInput() {
        // Move tetromino to the right.
        if ( Input.GetKeyDown(KeyCode.RightArrow) ) {
            transform.position += new Vector3(1, 0, 0);
            
            if ( IsValidPosition() )
                gameManager.UpdateGrid(this);
            else
                transform.position += new Vector3(-1, 0, 0);
        }
        // Move tetromino to the left.
        else if ( Input.GetKeyDown(KeyCode.LeftArrow) ) {
            transform.position += new Vector3(-1, 0, 0);

            if ( IsValidPosition() )
                gameManager.UpdateGrid(this);
            else
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
            if ( IsValidPosition() )
                gameManager.UpdateGrid(this);
            else
                transform.Rotate(0, 0, -rotationAngle.z);
        }
        // Move tetromino down.
        else if ( Input.GetKey(KeyCode.DownArrow) || Time.time - fall >= fallSpeed ) {
            transform.position += new Vector3(0, -1, 0);
            fall = Time.time;

            if ( IsValidPosition() )
                gameManager.UpdateGrid(this);
            else {
                transform.position += new Vector3(0, 1, 0);
                
                // Remove any row that is full.
                gameManager.DeleteRow();

                // If this tetro reachs out gridHight, then game is over.
                if ( gameManager.IsAboveGrid(this) )
                    gameManager.GameOver();
                
                // Disable this tetromino controller and spawn another piece.
                enabled = false;
                gameManager.SpawnNextTetromino();
            }
        }
    }

    bool IsValidPosition () {
        foreach ( Transform mino in transform ) {
            Vector2 position = gameManager.Round(mino.position);

            if ( !gameManager.CheckInsideGrid(position) )
                return false;
            
            if ( gameManager.GetMinoAtGridPosition(position) != null && gameManager.GetMinoAtGridPosition(position).parent != transform )
                return false;
        }
        return true;
    }
}
