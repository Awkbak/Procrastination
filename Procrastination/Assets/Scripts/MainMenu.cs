using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    /// <summary>
    /// Holds the main menu UI Button to load an existing game
    /// </summary>
    [SerializeField]
    private GameObject loadGameButton;

    void Awake()
    {
        //Orientation is only on mobile
        #if UNITY_ANDROID || UNITY_IPHONE
        //Make sure we are in landscape mode
        Screen.orientation = ScreenOrientation.LandscapeLeft;
#endif

        Random.seed = (int) System.DateTime.Now.Ticks;
    }

    void Start()
    {
        if (!SaveGame.save.saveExists())
        {
            loadGameButton.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update () {
        #if UNITY_ANDROID
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
        #endif
    }

    public void loadGame()
    {
        SaveGame.save.setIsLoadingGame(true);
        SceneManager.LoadScene("Floor1");
    }

    public void newGame()
    {
        SaveGame.save.setIsLoadingGame(false);
        SceneManager.LoadScene("Floor1");
    }

    public void exitGame()
    {
        Application.Quit();
    }
}
