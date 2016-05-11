using UnityEngine;
using System.Collections;

public class FloorGenerator : MonoBehaviour {

    /// <summary>
    /// Prefab for the floor/carpet of the game
    /// </summary>
    [SerializeField]
    private GameObject floorPrefab;

	// Use this for initialization
	void Start () {
	    for(int e = -19; e < 20; ++e)
        {
            for(int a = -19; a < 20; ++a)
            {
                Instantiate(floorPrefab, new Vector3(e * 2 + .5f, 0.01f, a * 2 + .5f), Quaternion.identity);
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
