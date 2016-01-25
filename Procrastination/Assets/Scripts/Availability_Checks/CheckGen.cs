using UnityEngine;
using System.Collections;
using System.Diagnostics;

public class CheckGen : MonoBehaviour {

    private Draggable parent;
    private GameObject[] panels;
    private Renderer[] panelMats;
    public Material matGood;
    public Material matBad;
    private int size;

    private bool allGood = false;

	// Use this for initialization
	void Start () {
        size = transform.childCount;
        panels = new GameObject[transform.childCount];
        panelMats = new Renderer[transform.childCount];
        int index = 0;
        foreach(Transform t in transform)
        {
            panels[index] = t.gameObject;
            panelMats[index] = t.GetComponent<Renderer>();
            panels[index].SetActive(false);
            ++index;
        }
	}
	
	public void setParent(Draggable p)
    {
        parent = p;
    }

    public bool disappear()
    {
        for(int e = 0; e < size; ++e)
        {
            panels[e].SetActive(false);
        }
        return allGood;
    }

    public void appear()
    {
        for (int e = 0; e < size; ++e)
        {
            panels[e].SetActive(true);
        }
        recheck();
    }

    public void recheck()
    {
        //Stopwatch watch = Stopwatch.StartNew();
        
        for (int e = 0; e < size; ++e)
        {
            RaycastHit hit = new RaycastHit();
            Ray ray = new Ray(panels[e].transform.position, new Vector3(0, -1, 0));
            if(Physics.Raycast(ray, out hit, 1.5f))
            {
                if (!hit.collider.CompareTag("Ground"))
                {
                    panelMats[e].material = matBad;
                }
                else
                {
                    panelMats[e].material = matGood;
                }
            }
        }
        //print(watch.ElapsedMilliseconds);
        //watch.Reset();
    }
}
