using UnityEngine;
using System.IO;
using System.Collections;
using System.Text;

public class SaveGame : MonoBehaviour {

    //https://msdn.microsoft.com/en-us/library/8bh11f1k.aspx

    private string name = "";
    private int bossLevel = 1;
    private int month = 5;
    private int day = 4;
    private int year = 2016;

    // Use this for initialization
    void Start () {
        //getShop("playersave.dat");
        loadGame(@"playersave.dat");
        saveGame(@"playersave2.dat");
	}

    /// <summary>
    /// Saves the current game state
    /// </summary>
    public void saveGame(string fileName)
    {
        StringBuilder save = new StringBuilder("NAME ");
        save.Append(name + '\n');
        save.Append("MONEY ");
        save.Append(Inventory.inv.getMoney());
        save.Append("\nBOSSLEVEL ");
        save.Append(bossLevel);
        save.Append("\nDATE ");
        save.Append(month + " " + day + " " + year + '\n');
        saveInventory(save);
        saveWorkers(save);
        System.IO.File.WriteAllText(@fileName, save.ToString());
    }

    /// <summary>
    /// Saves the current inventory related stuff
    /// </summary>
    /// <param name="save"></param>
    public void saveInventory(StringBuilder save)
    {
        save.Append("INVENTORY\n");
        Draggable[] objects = GameObject.FindObjectsOfType<Draggable>();
        foreach(Draggable obj in objects)
        {
            if(obj.GetComponent<Camera>() == null)
            {
                if (obj.name.Contains("Long_Desk_With_Chair"))
                {
                    save.Append("LONG_WITH_CHAIR");
                    save.Append(" " + obj.transform.position.x + ' ' + obj.transform.position.y + ' ' + obj.transform.position.z + ' ' + obj.transform.rotation.eulerAngles.y + '\n');
                }
                else if (obj.name.Contains("Long_Desk"))
                {
                    save.Append("LONG");
                    save.Append(" " + obj.transform.position.x + ' ' + obj.transform.position.y + ' ' + obj.transform.position.z + ' ' + obj.transform.rotation.eulerAngles.y + '\n');
                }
                else if (obj.name.Contains("Small_Desk_With_Chair"))
                {
                    save.Append("SMALL_WITH_CHAIR");
                    save.Append(" " + obj.transform.position.x + ' ' + obj.transform.position.y + ' ' + obj.transform.position.z + ' ' + obj.transform.rotation.eulerAngles.y + '\n');
                }
                else if (obj.name.Contains("Small_Desk"))
                {
                    save.Append("SMALL");
                    save.Append(" " + obj.transform.position.x + ' ' + obj.transform.position.y + ' ' + obj.transform.position.z + ' ' + obj.transform.rotation.eulerAngles.y + '\n');
                }
                else if (obj.name.Contains("WaterCooler"))
                {
                    save.Append("WATERCOOLER");
                    save.Append(" " + obj.transform.position.x + ' ' + obj.transform.position.y + ' ' + obj.transform.position.z + ' ' + obj.transform.rotation.eulerAngles.y + '\n');
                }
            }
        }

        save.Append("END_INVENTORY\n");
    }

    /// <summary>
    /// Saves the states of workers
    /// </summary>
    /// <param name="save"></param>
    public void saveWorkers(StringBuilder save)
    {
        save.Append("WORKERS\n");
        save.Append("END_WORKERS\n");
    }

    /// <summary>
    /// Loads a saved game
    /// </summary>
    public void loadGame(string fileName)
    {
        Scanner k;
        try {
            k = new Scanner(System.IO.File.ReadAllText(@fileName));

        }
        catch (FileNotFoundException e)
        {
            print("File not found");
            return;
        }
        string word;

        do
        {
            word = k.getNextWord();
            print("G" + word + "G");
            switch (word)
            {
                case "NAME":
                    string name = k.getNextWord();
                    this.name = name;
                    break;
                case "MONEY":
                    int money = k.nextInt();
                    Inventory.inv.setMoney(money);
                    break;
                case "BOSSLEVEL":
                    int bossLevel = k.nextInt();
                    this.bossLevel = bossLevel;
                    break;
                case "DATE":
                    int month = k.nextInt();
                    int day = k.nextInt();
                    int year = k.nextInt();

                    this.month = month;
                    this.day = day;
                    this.year = year;
                    break;
                case "INVENTORY":
                    loadInventory(k);
                    break;
                case "WORKERS":
                    loadWorkers(k);
                    break;
                default:
                    print("Error Parsing Save File: Unexpected Keyword '" + @word + "'");
                    return;
            }

        } while (k.hasNext());
    }

    /// <summary>
    /// Loads a saved state of the inventory
    /// </summary>
    /// <param name="k"></param>
    private void loadInventory(Scanner k)
    {
        string word = k.getNextWord();
        while (!word.Equals("END_INVENTORY"))
        {
            string itemName = word;
            Vector3 itemPosition = new Vector3(k.nextFloat(), k.nextFloat(), k.nextFloat());
            float itemRotation = k.nextFloat();

            GameObject item = Inventory.inv.makeItem(itemName);
            item.transform.position = itemPosition;
            item.transform.rotation = Quaternion.Euler(0, itemRotation, 0);

            word = k.getNextWord();
        }
    }

    /// <summary>
    /// Loads a saved state of workers
    /// </summary>
    /// <param name="k"></param>
    private void loadWorkers(Scanner k)
    {
        string word = k.getNextWord();
        while (!word.Equals("END_WORKERS"))
        {

            word = k.getNextWord();
        }
    }

    /// <summary>
    /// Reference for parsing files without using borrowed code
    /// </summary>
    /// <param name="fileName"></param>
    public void getShop(string fileName)
    {
        // Handle any problems that might arise when reading the text
        try
        {
            string line;
            StreamReader theReader = new StreamReader(@fileName, Encoding.Default);
            using (theReader)
            {
                line = theReader.ReadLine();

                if (line != null)
                {
                    do
                    {
                        string[] entries = line.Split(' ');
                        Debug.Log(entries[0]);
                        //op.Create(entries[0], System.Int32.Parse(entries[1]), System.Single.Parse(entries[2]), System.Single.Parse(entries[3]), System.Single.Parse(entries[4]), System.Int32.Parse(entries[5]));
                        //House_feature hue = Instantiate<House_feature>(op);
                        //Debug.Log(hue.nahme);
                        //availableItems.Add(hue);
                        if (entries.Length > 0)
                            line = theReader.ReadLine();
                    }
                    while (line != null);
                }
                theReader.Close();
            }
        }
        catch (System.Exception e)
        {
            Debug.Log(e);
            return;
        }
    }

}

class Scanner : System.IO.StringReader
{
    char[] separators = { ' ', '\n', '\t' };
    
    string[] wordList;
    int curWord;

    public Scanner(string source) : base(source)
    {

        wordList = source.Split(separators);
        curWord = 0;
    }

    public string getNextWord()
    {

        ++curWord;
        if(curWord > wordList.Length)
        {
            return "";
        }
        else
        {
            wordList[curWord - 1] = wordList[curWord - 1].Trim();
            return wordList[curWord - 1];
        }
        
    }

    public int nextInt()
    {
        ++curWord;
        if(curWord > wordList.Length)
        {
            return int.MinValue;
        }
        try
        {
            wordList[curWord - 1] = wordList[curWord - 1].Trim();
            return int.Parse(wordList[curWord - 1]);
        }
        catch
        {
            return int.MinValue;
        }

    }

    public double nextDouble()
    {
        ++curWord;
        if (curWord > wordList.Length)
        {
            return double.MinValue;
        }
        try
        {
            wordList[curWord - 1] = wordList[curWord - 1].Trim();
            return double.Parse(wordList[curWord - 1]);
        }
        catch
        {
            return int.MinValue;
        }
    }

    public float nextFloat()
    {
        ++curWord;
        if (curWord > wordList.Length)
        {
            return float.MinValue;
        }
        try
        {
            wordList[curWord - 1] = wordList[curWord - 1].Trim();
            return float.Parse(wordList[curWord - 1]);
        }
        catch
        {
            return float.MinValue;
        }
    }

    public bool hasNext()
    {
        return (curWord < wordList.Length);
    }
}
