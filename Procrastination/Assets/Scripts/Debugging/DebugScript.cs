using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;

public class DebugScript : MonoBehaviour {

    public static DebugScript d;
    private Text text;

    private int lines = 0;
    public int maxLines = 15;


	// Use this for initialization
	void Awake () {
        #if !Unity_EDITOR && DEVELOPMENT_BUILD
        Application.logMessageReceivedThreaded += handleLog;
        #endif
        d = this;
        text = GetComponent<Text>();
	}

    #if !Unity_EDITOR && DEVELOPMENT_BUILD
    private void handleLog(string logString, string stackTrace, LogType type)
    {
        if (type.Equals(LogType.Log))
        {
            println(logString);
        }
        else if(type.Equals(LogType.Warning))
        {
            println(type + ": " + logString);
        }
        else
        {
            if (stackTrace.Length < 50)
            {
                println(type + ": " + logString + " " + stackTrace);
            }
            else
            {
                println(type + ": " + logString + " " + stackTrace.Substring(0, 50));
            }
        }
    }
    #endif

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

    public void println(Vector3 vector)
    {
        println(vector.x + " : " + vector.y + " : " + vector.x);
    }

    public void println(Vector2 vector)
    {
        println(vector.x + " : " + vector.y);
    }

    public void clear()
    {
        text.text = "";
    }
}
