using UnityEngine;
using System.Collections;
using System.Diagnostics;

public class CheckGen : MonoBehaviour {

    /// <summary>
    /// The object which this panel checker is attached to
    /// </summary>
    private Draggable parent;
    /// <summary>
    /// List of the panels attached
    /// </summary>
    private GameObject[] panels;
    /// <summary>
    /// List of the Renderers of the panels attached
    /// </summary>
    private Renderer[] panelMats;
    /// <summary>
    /// Material of the panels that signify there is no obtruction
    /// </summary>
    public Material matGood;
    /// <summary>
    /// Material of the panels that signify there is an obtruction
    /// </summary>
    public Material matBad;
    /// <summary>
    /// Number of panels
    /// </summary>
    private int size;

    private bool allGood = false;

	// Use this for initialization
	void Start () {
        //Get number of panels and initialize arrays
        size = transform.childCount;
        panels = new GameObject[transform.childCount];
        panelMats = new Renderer[transform.childCount];

        //Get all children and put them inside arrays
        int index = 0;
        foreach(Transform t in transform)
        {
            panels[index] = t.gameObject;
            panelMats[index] = t.GetComponent<Renderer>();
            panels[index].SetActive(false);
            ++index;
        }
    }
	
    /// <summary>
    /// Set the parent object of this panel checker
    /// </summary>
    /// <param name="p">Parent Object</param>
	public void setParent(Draggable p)
    {
        parent = p;
    }

    /// <summary>
    /// Make panels dissapear (most likely because the user wants gto place parent)
    /// </summary>
    /// <returns>Whether or not every panel is not obstructed</returns>
    public bool disappear()
    {
        for(int e = 0; e < size; ++e)
        {
            panels[e].SetActive(false);
        }
        if (allGood)
        {
            return (AStar.solve() != null);
        }
        return false;
    }

    /// <summary>
    /// Make panels appear (Most likely because the user is dragging parent)
    /// </summary>
    public void appear()
    {
        for (int e = 0; e < size; ++e)
        {
            panels[e].SetActive(true);
        }
        recheck();
    }

    /// <summary>
    /// Checks panels to see if they are obstructed and updates their color/material accordingly
    /// </summary>
    public void recheck()
    {
        allGood = true;

        //Go through every panel
        for (int e = 0; e < size; ++e)
        {
            //Create a ray at the panel and send it down
            RaycastHit hit = new RaycastHit();
            Ray ray = new Ray(panels[e].transform.position, new Vector3(0, -1, 0));
            //If something is hit
            if(Physics.Raycast(ray, out hit, 1.5f))
            {
                //If the ground is hit, there are no obstructions
                if (!hit.collider.CompareTag("Ground"))
                {
                    allGood = false;
                    panelMats[e].material = matBad;
                }
                else
                {
                    panelMats[e].material = matGood;
                }
            }
            else
            {
                allGood = false;
                panelMats[e].material = matBad;
            }
        }
    }
}
