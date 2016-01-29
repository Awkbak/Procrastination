using UnityEngine;
using System.Collections;

public class Cubicle : Draggable {

	void Awake () {
        child = GetComponentInChildren<CheckGen>();
        child.setParent(this);
	}
}
