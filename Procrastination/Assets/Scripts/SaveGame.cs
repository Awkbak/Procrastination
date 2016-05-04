using UnityEngine;
using System.IO;
using System.Collections;

public class SaveGame : MonoBehaviour {

    //https://msdn.microsoft.com/en-us/library/8bh11f1k.aspx

    // Use this for initialization
    void Start () {
        loadGame();
	}

	// Update is called once per frame
	void Update () {

	}

    public void saveGame()
    {

    }

    public void loadGame()
    {
        Scanner k;
        try {
            k = new Scanner(System.IO.File.ReadAllText("playersave.dat"));

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
            switch (word)
            {
                case "NAME":
                    string name = k.getNextWord();
                    break;
                case "MONEY":
                    int money = k.nextInt();
                    break;
                case "BOSSLEVEL":
                    int bossLevel = k.nextInt();
                    break;
                case "DATE":
                    int month = k.nextInt();
                    int day = k.nextInt();
                    int year = k.nextInt();
                    break;
                case "INVENTORY":
                    loadInventory(k);
                    break;
                case "WORKERS":
                    loadWorkers(k);
                    break;
            }

        } while (k.hasNext());
    }

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

    private void loadWorkers(Scanner k)
    {
        string word = k.getNextWord();
        while (!word.Equals("END_WORKERS"))
        {

            word = k.getNextWord();
        }
    }

}

class Scanner : System.IO.StringReader
{
    string currentWord;

    public Scanner(string source) : base(source)
    {
        nextWord();
    }

    private void nextWord()
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        char nextChar;
        int next;
        do
        {
            next = this.Read();
            if (next < 0)
                break;
            nextChar = (char)next;
            if (char.IsWhiteSpace(nextChar))
                break;
            sb.Append(nextChar);
        } while (true);
        while ((this.Peek() >= 0) && (char.IsWhiteSpace((char)this.Peek())))
            this.Read();
        if (sb.Length > 0)
            currentWord = sb.ToString();
        else
            currentWord = null;
    }

    public string getNextWord()
    {
        string word = currentWord;
        nextWord();
        return word;
    }

    public bool hasNextInt()
    {
        if (currentWord == null)
            return false;
        int dummy;
        return int.TryParse(currentWord, out dummy);
    }

    public int nextInt()
    {
        try
        {
            return int.Parse(currentWord);
        }
        finally
        {
            nextWord();
        }
    }

    public bool hasNextDouble()
    {
        if (currentWord == null)
            return false;
        double dummy;
        return double.TryParse(currentWord, out dummy);
    }

    public double nextDouble()
    {
        try
        {
            return double.Parse(currentWord);
        }
        finally
        {
            nextWord();
        }
    }

    public bool hasNextFloat()
    {
        if (currentWord == null)
            return false;
        float dummy;
        return float.TryParse(currentWord, out dummy);
    }

    public float nextFloat()
    {
        try
        {
            return float.Parse(currentWord);
        }
        finally
        {
            nextWord();
        }
    }

    public bool hasNext()
    {
        return currentWord != null;
    }
}
