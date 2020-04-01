using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuSystem : MonoBehaviour {
    public void Restart ( ) => SceneManager.LoadScene("Level");
}
