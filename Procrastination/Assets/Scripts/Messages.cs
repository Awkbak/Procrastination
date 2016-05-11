using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Messages : MonoBehaviour {

    private Text messageText;

    [SerializeField]
    private GameObject background;

    /// <summary>
    /// Messages for when you get caught
    /// </summary>
    [SerializeField]
    private string[] messages;

    void Awake()
    {
        messageText = GetComponent<Text>();

        messageText.text = "";
        background.SetActive(false);
        messageText.gameObject.SetActive(false);
    }

	public void displayMessage()
    {
        StartCoroutine(message());
    }

    public void firedMessage()
    {
        background.SetActive(true);
        messageText.gameObject.SetActive(true);
        messageText.text = "YOUR FIRED!!!";
    }

    IEnumerator message()
    {
        background.SetActive(true);
        messageText.gameObject.SetActive(true);
        int choice = Random.Range(0, messages.Length);

        messageText.text = messages[choice];

        yield return new WaitForSeconds(2.0f);

        messageText.text = "";
        background.SetActive(false);
        messageText.gameObject.SetActive(false);

    }
}
