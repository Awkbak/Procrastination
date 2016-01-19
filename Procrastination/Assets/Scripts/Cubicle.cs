using UnityEngine;
using System.Collections;

public class Cubicle : Draggable {

	// Use this for initialization
	void Awake () {
        tileSize = 4;
        child = GetComponentInChildren<CheckGen>();
        child.setParent(this);
	}
}
