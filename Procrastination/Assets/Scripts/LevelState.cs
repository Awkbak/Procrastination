using UnityEngine;
using System.Collections;
using UnityEngine.UI;

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
    /// Corresponds to the time of day UI Text
    /// </summary>
    [SerializeField]
    private Text timeOfDayText;

    /// <summary>
    /// Corresponds to the current time UI Text
    /// </summary>
    [SerializeField]
    private Text curTimeText;

    /// <summary>
    /// Can you make money?
    /// </summary>
    private bool makeMoney = false;

    /// <summary>
    /// The current game's boss level
    /// </summary>
    private int bossLevel = 1;

    /// <summary>
    /// The current hour
    /// </summary>
    private int currentHour = 0;

    /// <summary>
    /// A general timer for timing
    /// </summary>
    private float generalTimer1 = 0.0f;

    /// <summary>
    /// A general timer for timing
    /// </summary>
    private float generalTimer2 = 0.0f;

	// Use this for initialization
	void Awake () {
        cur = this;
        timeOfDayText.text = "Early Morning";
        curTimeText.text = "7:59 am";
	}

    void Start()
    {
        Inventory.inv.increasePay(5);
    }

    public void FixedUpdate()
    {
        if (currentLevelState.Equals(LevelStates.Workday) || currentLevelState.Equals(LevelStates.Night))
        {
            generalTimer1 += Time.fixedDeltaTime;

            float perHour = (generalTimer2 / 9);

            int hour = (int) (generalTimer1 / perHour);
            int minutes = (int) (60 * (generalTimer1 - (hour * perHour)) / perHour);


            if(hour > 4)
            {
                hour -= 4;
                curTimeText.text = hour + ":" + (minutes < 10 ? "0" : "") + minutes + " pm";
            }
            else
            {
                curTimeText.text = (hour + 8) + ":" + (minutes < 10 ? "0" : "") + minutes + (hour == 4 ? " pm" : " am");
            }

            if(currentHour != hour)
            {
                if (makeMoney)
                {
                    Inventory.inv.payDay();
                }
                currentHour = hour;
            }

            if(generalTimer1 > generalTimer2 && currentLevelState.Equals(LevelStates.Workday))
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
            timeOfDayText.text = "Workday";
            generalTimer1 = 0;
            generalTimer2 = (0.1f * bossLevel * 10.0f) + 10.0f;
        }
    }

    public void toggleState()
    {
        if (currentLevelState.Equals(LevelStates.Build))
        {
            currentLevelState = LevelStates.Workday;
            timeOfDayText.text = "Workday";
            currentHour = 0;
            generalTimer1 = 0;
            generalTimer2 = (0.1f * bossLevel * 10.0f) + 10.0f;
            makeMoney = true;
        }
        else if (currentLevelState.Equals(LevelStates.Workday))
        {
            currentLevelState = LevelStates.Night;
            timeOfDayText.text = "Night";
            makeMoney = true;
        }
        else if (currentLevelState.Equals(LevelStates.Night))
        {
            currentLevelState = LevelStates.Build;
            timeOfDayText.text = "Early Morning";
            curTimeText.text = "7:59 am";
            makeMoney = false;
            ++bossLevel;
            Inventory.inv.increasePay(bossLevel * 5);
        }
    }

    public void gotCaught()
    {
        makeMoney = false;
        generalTimer1 = generalTimer2;
        Inventory.inv.penalty(generalTimer1 / generalTimer2, bossLevel);
        currentLevelState = LevelStates.Night;
    }
}
