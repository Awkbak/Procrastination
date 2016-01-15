using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DebugScript : MonoBehaviour {

    public static DebugScript d;
    private Text text;

    private int lines = 0;
    public int maxLines = 15;


	// Use this for initialization
	void Awake () {
        d = this;
        text = GetComponent<Text>();
	}
	
	public void print(string line)
    {
        text.text += line;
    }

    public void println(string line)
    {
        text.text += line + '\n';
        if(lines == 15)
        {
            text.text = text.text.Substring(text.text.IndexOf('\n') + 1);
        }
        else
        {
            ++lines;
        }
    }

    public void clear()
    {
        text.text = "";
    }
}
