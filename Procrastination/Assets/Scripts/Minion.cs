using UnityEngine;
using System.Collections;

public class Minion : MonoBehaviour {

    /// <summary>
    /// Is this minion currently working?
    /// </summary>
    private bool working = false;

    /// <summary>
    /// How much money per time this minion makes
    /// </summary>
    private float moneyPerTime = 10.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void toggleProductivity()
    {
        working = !working;
    }
}
