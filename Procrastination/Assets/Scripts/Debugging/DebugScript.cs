using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;

/// <summary>
/// Used in order to capture messages to conole and display them on mobile
/// </summary>
public class DebugScript : MonoBehaviour {

    /// <summary>
    /// Only use outside of the editor (can just use normal console within editor)
    /// and if it's a dev build (don't need console output in production)
    /// </summary>
    #if !Unity_EDITOR && DEVELOPMENT_BUILD

    /// <summary>
    /// Text component used to display console output
    /// </summary>
    private Text text;
    /// <summary>
    /// How many lines are displaying in the console
    /// </summary>
    private int lines = 0;
    /// <summary>
    /// Max number of lines the console can display (Default = 15)
    /// </summary>
    private int maxLines = 15;


	// Use this for initialization
	void Awake () {
        //Register a callback for the Unity Debug messages
        Application.logMessageReceivedThreaded += handleLog;
        //Get the attached text component
        text = GetComponent<Text>();
	}

    /// <summary>
    /// Handles captured messages from the debug console
    /// </summary>
    /// <param name="logString">The message displayed</param>
    /// <param name="stackTrace">Stack trace of where the message occured (Mainly used with error and exception messaged)</param>
    /// <param name="type">What kind of message (Log, Warning, Warning, Error, Exception, Assert)</param>
    private void handleLog(string logString, string stackTrace, LogType type)
    {
        //If it's a log, just output it
        if (type.Equals(LogType.Log))
        {
            println(logString);
        }//If it's a warning, include that type (Warning)
        else if(type.Equals(LogType.Warning))
        {
            println(type + ": " + logString);
        }//Otherwise, it is serious and output the type, log, and stacktrace
        else
        {
            //Wrapping lines messes up the console
            //This curbs it a bit, but still allows for some stack output
            //Since in an error, reading what's wrong is most important
            if (stackTrace.Length < 100)
            {
                println(type + ": " + logString + " " + stackTrace);
            }
            else
            {
                println(type + ": " + logString + " " + stackTrace.Substring(0, 100));
            }
        }
    }
    
    /// <summary>
    /// Add to the console, but don't create a new line
    /// </summary>
    /// <param name="line">What to add to the console</param>
    public void print(string line)
    {
        text.text += line;
    }
    /// <summary>
    /// Add to the console, but don't create a new line
    /// </summary>
    /// <param name="obj">Object to print (Calls objects toString method)</param>
    public new void print(object obj)
    {
        print(obj.ToString());
    }

    /// <summary>
    /// Add to the console and create a enw line
    /// </summary>
    /// <param name="line">What to add to the console</param>
    public void println(string line)
    {
        text.text += line + '\n';
        //If you reach the max number of lines, clear out the top one
        if(lines > maxLines)
        {
            text.text = text.text.Substring(text.text.IndexOf('\n') + 1);
        }
        else
        {
            ++lines;
        }
    }

    /// <summary>
    /// Add to the console and create a new line
    /// </summary>
    /// <param name="obj">Object to print (Calls objects toString method)</param>
    public void println(object obj)
    {
        println(obj.ToString());
    }

    /// <summary>
    /// Clears the console
    /// </summary>
    public void clear()
    {
        text.text = "";
    }
    #endif
}
