using UnityEngine;
using System.Collections;
using System.Diagnostics;

public class CheckGen : MonoBehaviour {

    private Draggable parent;
    private GameObject[] panels;
    private Renderer[] panelMats;
    public GameObject panelPrefab;
    public Material matGood;
    public Material matBad;
    private int size;

    private bool allGood = false;

	// Use this for initialization
	void Start () {
        size = parent.getSize();
        panels = new GameObject[size * size];
        panelMats = new Renderer[size * size];
        int index = 0;
        for(int e = 0; e < size; ++e)
        {
            for(int a = 0; a < size; ++a)
            {
                panels[index] = Instantiate(panelPrefab, transform.position, Quaternion.identity) as GameObject;
                panels[index].transform.SetParent(this.transform);
                panels[index].transform.Translate(1 * e, 0, -1 * a);
                panelMats[index] = panels[index].GetComponent<Renderer>();
                ++index;
            }
        }
        disappear();
	}
	
	public void setParent(Draggable p)
    {
        parent = p;
    }

    public bool disappear()
    {
        for(int e = 0; e < size * size; ++e)
        {
            panels[e].SetActive(false);
        }
        return allGood;
    }

    public void appear()
    {
        for (int e = 0; e < size * size; ++e)
        {
            panels[e].SetActive(true);
        }
        recheck();
    }

    public void recheck()
    {
        //Stopwatch watch = Stopwatch.StartNew();
        
        int index = 0;
        for (int e = 0; e < size; ++e)
        {
            for (int a = 0; a < size; ++a)
            {
                RaycastHit hit = new RaycastHit();
                Ray ray = new Ray(panels[index].transform.position, new Vector3(0, -1, 0));
                if(Physics.Raycast(ray, out hit, 1.5f))
                {
                    if (!hit.collider.CompareTag("Ground"))
                    {
                        panelMats[index].material = matBad;
                    }
                    else
                    {
                        panelMats[index].material = matGood;
                    }
                }
                ++index;
            }
        }
        //print(watch.ElapsedMilliseconds);
        //watch.Reset();
    }
}
