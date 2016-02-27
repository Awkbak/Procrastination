using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    /// <summary>
    /// Starts the game
    /// </summary>
	public void startGame()
    {
        SceneManager.LoadScene(1);
    }
}
