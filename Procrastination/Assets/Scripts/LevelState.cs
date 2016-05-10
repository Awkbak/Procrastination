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

    /// <summary>
    /// The current game's boss level
    /// </summary>
    private int bossLevel = 1;

    private float generalTimer1 = 0.0f;

	// Use this for initialization
	void Awake () {
        cur = this;
	}

    public void FixedUpdate()
    {
        if (currentLevelState.Equals(LevelStates.Workday))
        {
            generalTimer1 -= Time.fixedDeltaTime;

            if(generalTimer1 < 0)
            {
                toggleState();
            }
        }
    }
	
	public void setState(LevelStates state)
    {
        currentLevelState = state;

        if (currentLevelState.Equals(LevelStates.Workday))
        {
            generalTimer1 = (0.15f * bossLevel * 15.0f) + 15.0f;
        }
    }

    public void toggleState()
    {
        if (currentLevelState.Equals(LevelStates.Build))
        {
            currentLevelState = LevelStates.Workday;
            generalTimer1 = (0.15f * bossLevel * 3.0f) + 3.0f;
        }
        else if (currentLevelState.Equals(LevelStates.Workday))
        {
            currentLevelState = LevelStates.Night;
        }
        else if (currentLevelState.Equals(LevelStates.Night))
        {
            currentLevelState = LevelStates.Build;
        }
    }
}
