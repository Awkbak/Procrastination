using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenuOld : MonoBehaviour {

    void Start()
    {

        //Orientation is only on mobile
        #if UNITY_ANDROID || UNITY_IPHONE
        //Make sure we are in landscape mode
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        #endif
    }

    /// <summary>
    /// Starts the game
    /// </summary>
	public void startGame()
    {
        SceneManager.LoadScene(1);
    }
}
