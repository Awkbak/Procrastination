using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndMenu : MonoBehaviour {

    private Text survivalText;

    [SerializeField]
    private GameObject reloadButton;

	// Use this for initialization
	void Start () {
        survivalText = GetComponent<Text>();

        survivalText.text = "You survived " + LevelState.bossLevel + " days";

        if (!SaveGame.save.saveExists())
        {
            reloadButton.SetActive(false);
        }
    }

    void Update()
    {
        #if UNITY_ANDROID
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
        #endif
    }

    public void newGame()
    {
        SceneManager.LoadScene(1);
    }

    public void reload()
    {
        SaveGame.save.setIsLoadingGame(true);
        SceneManager.LoadScene(1);
    }

    public void mainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void exitGame()
    {
        Application.Quit();
    }
}
