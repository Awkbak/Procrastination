using UnityEngine;
using System.Collections;

public class LevelState : MonoBehaviour {

    /// <summary>
    /// A static reference to this script
    /// </summary>
    public static LevelState cur;

    /// <summary>
    /// The possible states of the level
    /// </summary>
    public enum LevelStates { Build, Workday, Night}

    /// <summary>
    /// The levels current level state
    /// </summary>
    public LevelStates currentLevelState = LevelStates.Build;

	// Use this for initialization
	void Awake () {
        cur = this;
	}
	
	public void setState(LevelStates state)
    {
        currentLevelState = state;
    }
}
