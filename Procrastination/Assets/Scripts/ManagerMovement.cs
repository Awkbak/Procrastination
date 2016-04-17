using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ManagerMovement : MonoBehaviour {

    /// <summary>
    /// This objects Rigidbody
    /// </summary>
    private Rigidbody rigidbody;

    /// <summary>
    /// The manager's start position
    /// </summary>
    private Vector3 startPos;

    /// <summary>
    /// The manager's end position (players desk)
    /// </summary>
    private Vector3 endPos;

    /// <summary>
    /// Next node in an AStar sequence
    /// </summary>
    private Node nextNode;

    /// <summary>
    /// The next position in an AStar sequence
    /// </summary>
    private Vector3 nextPos;

    /// <summary>
    /// Is this manager currently moving?
    /// </summary>
    [SerializeField]
    private bool moving = false;

    /// <summary>
    /// Used to make sure movement is setup only once
    /// </summary>
    private bool setUpMovement = true;

    /// <summary>
    /// This manager's movement speed in Units/Second
    /// </summary>
    private float movementSpeed = 5.0f;

	// Use this for initialization
	void Start () {
        rigidbody = GetComponent<Rigidbody>();

        //Get where the manager starts on the level
        startPos = GameObject.FindGameObjectWithTag("ManagerStart").transform.position;
        //Convert to Vector2
        startPos.y = startPos.z;
        //Get where the player's desk is
        endPos = GameObject.FindGameObjectWithTag("PlayerDesk").transform.position;
        //Convert to Vector2
        endPos.y = endPos.z;

        //Send positions to AStar
        AStar.startPos = startPos;
        AStar.endPos = endPos;
        //Find the current path
        Node pathNode = AStar.solve();
        if(pathNode == null)
        {
            print("No path found");
        }
        else
        {//Print path
            Node current = pathNode;
            string path = "";
            while (current.child != null)
            {
                path += current.x + ":" + current.y + "  :  ";
                current = current.child;
            }
            path += current.x + ":" + current.y + "  :  ";
            Debug.Log(path);
        }
        
    }

    void FixedUpdate()
    {
        if (LevelState.cur.currentLevelState.Equals(LevelState.LevelStates.Workday))
        {
            if (setUpMovement)
            {
                
                startMoving();
            }
            else
            {
                if (moving)
                {
                    print(rigidbody.velocity);
                    Vector3 direction = transform.position - nextPos;
                    direction.y = 0;
                    if (direction.sqrMagnitude < 0.05f)
                    {
                        nextNode = nextNode.child;
                        if (nextNode != null)
                        {
                            nextPos = nextNode.generateVector3();
                        }
                        else
                        {
                            print("Done");
                            moving = false;
                        }
                    }
                    //print(nextPos + " : " + direction);
                    direction.Normalize();
                    rigidbody.velocity = -direction * movementSpeed;

                }
                else {
                    rigidbody.velocity = Vector3.zero;
                    //setUpMovement = true;
                }
            }
            
        }
    }

    public bool recalculate()
    {
        //Send positions to AStar
        AStar.startPos = startPos;
        AStar.endPos = endPos;
        //Find the current path
        return (AStar.solve() == null);
    }

    public void startMoving()
    {
        setUpMovement = false;
        moving = true;
        Vector3 startPosTrans = startPos;
        startPosTrans.z = startPos.y;
        startPos.y = 0.7f;
        transform.position = startPos;

        nextNode = AStar.solve();
        nextPos = nextNode.generateVector3();

    }

    public void stopMoving()
    {
        moving = false;
    }
}


/// <summary>
/// Computes the estimated shortest path between two places using the AStar algorithm
/// </summary>
public class AStar
{

    /// <summary>
    /// Where to begin the path
    /// </summary>
    public static Vector2 startPos;

    /// <summary>
    /// Where to end the path
    /// </summary>
    public static Vector2 endPos;

    /// <summary>
    /// Finds a path from the managers position and the players desk
    /// </summary>
    /// <returns>Path to the goal</returns>
    public static Node solve()
    {
        //Max number of checked nodes
        int maxCycles = 500;
        //Current number of checked nodes
        int curCycle = 0;

        //Create nodes from the start and end positino
        Node startNode = new Node((int)startPos.x, (int)startPos.y);
        Node endNode = new Node((int)endPos.x, (int)endPos.y);

        //Calculate heuristics of start and end position
        startNode.h = 0;
        startNode.f = dist(startNode, endNode);
        endNode.f = 0;

        //Create lists of to be visited, visited, and wall nodes
        List<Node> openList = new List<Node>();
        List<Node> closedList = new List<Node>();
        List<Node> walls = new List<Node>();

        //Add the start node to the to be visited list
        openList.Add(startNode);

        //While there are still nodes to be visited
        while(openList.Count > 0)
        {

            ++curCycle;
            //Keep the to be visited nodes sorted by priority
            openList.Sort();
            //Get the next node in openlist
            Node current = openList[0];
            openList.RemoveAt(0);

            //If we are at max cycles, break out
            if(curCycle == maxCycles)
            {
                Debug.Log("Reached Search Limit");
                return null;
            }

            //If we are done
            if (current.Equals(endNode))
            {
                //Generate back path
                while (!current.Equals(startNode))
                {
                    current.parent.child = current;
                    current = current.parent;
                }
                return current;
            }
            //Go through neighbors (not corners)
            for(int e = -1; e <= 1; ++e)
            {
                for(int a = -1; a <= 1; ++a)
                {
                    //No corners
                    if(e == a || (e == -1 && a == 1) || (e == 1 && a == -1))
                    {
                        continue;
                    }
                    int x = current.x + e;
                    int y = current.y + a;

                    //Make a node of the neighbor
                    Node neighbor = new Node(x, y);

                    //If this isn't an already found wall
                    if (!walls.Contains(neighbor))
                    {
                        //Set the parent as the current node and set heuristics
                        neighbor.parent = current;
                        neighbor.setG(current.g + 1);
                        neighbor.setH(dist(neighbor, endNode));

                        //If node hasn't been traversed already
                        if (closedList.Contains(neighbor) == false)
                        {
                            //If the node is waiting to be traversed
                            if (openList.Contains(neighbor))
                            {
                                //See if this path to the node is quicker
                                //and update if it is
                                Node inList = openList[openList.IndexOf(neighbor)];
                                if(neighbor.f < inList.f)
                                {
                                    inList.setG(neighbor.g);
                                    inList.setH(neighbor.h);
                                    inList.parent = neighbor.parent;
                                }
                            }//If node has been traversed already
                            else
                            {
                                //See if it is a valid position and add to the to be visited nodes if it is
                                if (positionAvailable(neighbor))
                                {
                                    openList.Add(neighbor);
                                }//If it isn't a valid position, add it to the walls list
                                else
                                {
                                    walls.Add(neighbor);
                                }
                            }
                        }
                    }
                }
            }
            //Mark this node as visited
            closedList.Add(current);
        }
        Debug.Log("No more options");
        return null;

    }

    /// <summary>
    /// Compute the manhatten distance from one node to another
    /// </summary>
    /// <param name="begin">Node to start distance check</param>
    /// <param name="end">Node to end distance check</param>
    /// <returns>Manhatten distance between begin and end</returns>
    public static int dist(Node begin, Node end)
    {
        return (int) (Mathf.Abs(begin.x - end.x) + Mathf.Abs(begin.y - end.y));
    }

    /// <summary>
    /// Check if a position is valid to move on
    /// </summary>
    /// <param name="node">Node to check</param>
    /// <returns>Is this node pathable?</returns>
    public static bool positionAvailable(Node node)
    {
        //Get the tag of what is on the node
        string tag = Touch_Handler.handler.raycast(node.x, node.y);
        //Return if it is pathable based on tag
        return (tag.Equals("Ground") || tag.Equals("PlayerDesk") || tag.Equals("ManagerStart"));
    }
    
}

/// <summary>
/// A node withing each position on AStar
/// </summary>
public class Node : System.IEquatable<Node>, System.IComparable<Node>
{
    /// <summary>
    /// X position of the node
    /// </summary>
    public int x { get; set; }

    /// <summary>
    /// Y position of the node
    /// </summary>
    public int y { get; set; }

    /// <summary>
    /// Heuristic value = g + h
    /// </summary>
    public int f { get; set; }

    /// <summary>
    /// Estimated cost to reach the goal
    /// </summary>
    public int h;

    /// <summary>
    /// Travel cost to this node
    /// </summary>
    public int g;

    /// <summary>
    /// The node prior to this in the path
    /// </summary>
    public Node parent { get; set; }

    /// <summary>
    /// The node after this in the path
    /// </summary>
    public Node child { get; set; }

    public Node() { }

    public Node(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    /// <summary>
    /// Set the cost to travel to this node
    /// </summary>
    /// <param name="val">Cost of travel</param>
    public void setG(int val)
    {
        g = val;
        f = g + h;
    }

    /// <summary>
    /// Set the heuristic cost to the end
    /// </summary>
    /// <param name="val">Heuristis cost to the end</param>
    public void setH(int val)
    {
        h = val;
        f = g + h;
    }

    /// <summary>
    /// Get the cost to travel to this node
    /// </summary>
    /// <returns>Cost to travel to this node</returns>
    public int getG() { return g; }

    /// <summary>
    /// Get the heuristic cost of this node
    /// </summary>
    /// <returns>Heuristic cost to the end node</returns>
    public int getH() { return h; }

    public Vector3 generateVector3()
    {
        return new Vector3(x, 0, y);
    }

    public bool Equals(Node other)
    {
        return (this.x == other.x && this.y == other.y);
    }

    public int CompareTo(Node other)
    {
        return this.f.CompareTo(other.f);
    }
}