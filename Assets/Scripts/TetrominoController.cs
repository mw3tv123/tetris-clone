using UnityEngine;

public class TetrominoController : MonoBehaviour {
    Vector3 rotationAngle = new Vector3(0, 0, 90);
    GameManager gameManager;

    #region Tetromino Properties
    float fall = 0f;
    public float fallSpeed = 1f;

    public bool allowRotation = true;
    public bool limitRotation = false;
    #endregion

    #region SoundFX
    public AudioClip moveSFX;       // Sound effect play when tetromino move.
    public AudioClip rotateSFX;     // Sound effect play when tetromino rotate.
    public AudioClip landSFX;       // Sound effect play when tetromino land.
    private AudioSource audioSource;
    #endregion

    #region Controller Optimize
    private float verticalSpeed = 0.05f;        // The speed the tetromino move down.
    private float horizontalSpeed = 0.1f;       // The speed the tetromino move left and right.
    private float buttonDownWaitMax = 0.25f;     // How long before tetromino recognizes the button is being held down.

    private float verticalTimer = 0;
    private float horizontalTimer = 0;
    private float buttonDownWaitTimer = 0;

    private bool movedImmediateHorizontal = false;
    private bool movedImmediateVertical = false;
    #endregion

    // Start is called before the first frame update
    void Start() {
        gameManager = FindObjectOfType<GameManager>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update() {
        CheckUserInput();
    }

    void CheckUserInput() {

        if ( Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.DownArrow) ) {
            movedImmediateHorizontal = false;
            movedImmediateVertical = false;
            
            horizontalTimer = 0;
            verticalTimer = 0;
            buttonDownWaitTimer = 0;
        }

        // Move tetromino to the right.
        if ( Input.GetKey(KeyCode.RightArrow) ) {
            if ( movedImmediateHorizontal ) {
                if ( buttonDownWaitTimer < buttonDownWaitMax ) {
                    buttonDownWaitTimer += Time.deltaTime;
                    return;
                }

                if ( horizontalTimer < horizontalSpeed ) {
                    horizontalTimer += Time.deltaTime;
                    return;
                }
            }

            if ( !movedImmediateHorizontal )
                movedImmediateHorizontal = true;

            horizontalTimer = 0;

            transform.position += Vector3.right;
            
            if ( IsValidPosition() ) {
                gameManager.UpdateGrid(this);
                PlayAudio(moveSFX);
            }
            else
                transform.position += Vector3.left;
        }
        // Move tetromino to the left.
        else if ( Input.GetKey(KeyCode.LeftArrow) ) {
            if ( movedImmediateHorizontal ) {
                if ( buttonDownWaitTimer < buttonDownWaitMax ) {
                    buttonDownWaitTimer += Time.deltaTime;
                    return;
                }
                
                if ( horizontalTimer < horizontalSpeed ) {
                    horizontalTimer += Time.deltaTime;
                    return;
                }
            }

            if ( !movedImmediateHorizontal )
                movedImmediateHorizontal = true;
            
            horizontalTimer = 0;

            transform.position += Vector3.left;

            if ( IsValidPosition() ) {
                gameManager.UpdateGrid(this);
                PlayAudio(moveSFX);
            }
            else
                transform.position += Vector3.right;
        }
        // Rotation tetromino.
        else if ( Input.GetKeyDown(KeyCode.UpArrow) && allowRotation ) {
            // Tetromino like Z and I only need to rotate in range of (0 -> 90) degree.
            if ( limitRotation )
                rotationAngle.z = transform.rotation.eulerAngles.z >= 90 ? -90 : 90;

            // Rotate this tetromino.
            transform.Rotate(rotationAngle);

            // If tetromino is out of grid border, rotate back to original position.
            if ( IsValidPosition() ) {
                gameManager.UpdateGrid(this);
                PlayAudio(rotateSFX);
            }
            else
                transform.Rotate(0, 0, -rotationAngle.z);
        }
        // Move tetromino down.
        else if ( Input.GetKey(KeyCode.DownArrow) || Time.time - fall >= fallSpeed ) {
            if ( movedImmediateVertical ) {
                if ( buttonDownWaitTimer < buttonDownWaitMax ) {
                    buttonDownWaitTimer += Time.deltaTime;
                    return;
                }
                
                if ( verticalTimer < verticalSpeed ) {
                    verticalTimer += Time.deltaTime;
                    return;
                }
            }

            if ( !movedImmediateVertical )
                movedImmediateVertical = true;

            verticalTimer = 0;

            transform.position += Vector3.down;
            fall = Time.time;

            if ( IsValidPosition() )
                gameManager.UpdateGrid(this);
            else {
                transform.position += Vector3.up;
                PlayAudio(landSFX);
                
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

    /// <summary>
    ///  Play a sound FX one time.
    /// </summary>
    void PlayAudio ( AudioClip audio ) => audioSource.PlayOneShot(audio);

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
