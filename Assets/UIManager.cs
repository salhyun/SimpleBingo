using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour {

    public struct MyBingoCard
    {
        public GameObject[,] numbers;

        public MyBingoCard(int row, int col)
        {
            numbers = new GameObject[col, row];
        }

        public void checkNumber(int num)
        {
            for (int i = 0; i < numbers.GetLength(0); i++)
            {
                for (int n = 0; n < numbers.GetLength(1); n++)
                {
                    if (i == 2 && n == 2)//FREE
                    {
                    }
                    else
                    {
                        int numText = int.Parse(numbers[i, n].GetComponentInChildren<UnityEngine.UI.Text>().text);
                        if (numText == num)
                        {
                            Debug.Log("[" + i + ", " + n + "] = " + numbers[i, n]);
                            numbers[i, n].GetComponent<UnityEngine.UI.Button>().interactable = false;
                            return;
                        }
                    }
                }
            }
            Debug.Log("no match");
        }
    }

    struct BingoNumber
    {
        public string bingoChar;
        public int number;
    }

    public const int BINGOALLNUM = 75;
    public const int BINGONUM_ROW = 5;
    public const int BINGONUM_COL = 5;

    private BingoNumber[] BingoNumbers = new BingoNumber[BINGOALLNUM];

    //private BingoCard MyBingoCard = new BingoCard(BINGONUM_COL, BINGONUM_ROW);
    private MyBingoCard myBingoCard = new MyBingoCard(BINGONUM_COL, BINGONUM_ROW);

    private int BingoCounter = 0;

    public Camera mainCamera;
    public GameObject BingoPanel;
    public GameObject numberLine;

	// Use this for initialization
	void Start () {

        string[] bingoChar = { "B", "I", "N", "G", "O" };

        Rect rtBingoPanel = BingoPanel.GetComponent<RectTransform>().rect;//RectTransform은 local 좌표임.
        Rect rtNumberLine = numberLine.GetComponent<RectTransform>().rect;

        float offsetX = rtBingoPanel.min.x;//좌측
        float offsetY = rtBingoPanel.min.y+(rtNumberLine.height* BINGONUM_COL);//하단
        Vector3 pos;

        for(int i=0; i<5; i++)
        {
            int[] columnNums = createColumnNum(i);
            for(int n=0; n<5; n++)
            {
                numberLine = GameObject.Find("number" + bingoChar[i] + "Line" + n);

                if (i == 2 && n == 2)
                    numberLine.GetComponentInChildren<UnityEngine.UI.Text>().text = "FREE";
                else
                    numberLine.GetComponentInChildren<UnityEngine.UI.Text>().text = columnNums[n].ToString();

                //MyBingoCard.numbers[i, n] = columnNums[n];
                myBingoCard.numbers[i, n] = numberLine;

                Rect rt =  numberLine.GetComponent<RectTransform>().rect;

                pos.x = offsetX + (i * rt.width) + rt.width/2.0f;
                pos.y = offsetY - (n * rt.height) - rt.height/2.0f;
                pos.z = numberLine.transform.position.z;
                numberLine.transform.position = BingoPanel.transform.TransformPoint(pos);//RectTransform은 local 좌표임. 그래서 월드좌표로 변환해준다.
            }
        }

        resetBingoNumber(BingoNumbers);

        StartCoroutine(callNumber());

        Debug.Log("Start end");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator callNumber()
    {
        while(true)
        {
            Debug.Log("BingoNumber = " + BingoNumbers[BingoCounter].number);
            //MyBingoCard.checkNumber(BingoNumbers[BingoCounter++].number);
            myBingoCard.checkNumber(BingoNumbers[BingoCounter++].number);

            yield return new WaitForSeconds(2);
        }
    }

    private void resetBingoNumber(BingoNumber []bingoNumber)
    {
        string[] bingoChar = { "B", "I", "N", "G", "O" };
        int count=0;

        for (int n=0; n<5; n++)
        {
            for (int i = 0; i < 15; i++)
            {
                count = i + (n * 15);
                bingoNumber[count].number = count + 1;
                bingoNumber[count].bingoChar = bingoChar[n];
            }
        }
        shuffle(ref bingoNumber);
        BingoCounter = 0;
    }

    private int[] createColumnNum(int c)
    {
        int[] ar = new int[15];

        for (int i = 0; i < ar.Length; i++)
            ar[i] = i + (c * 15) + 1;

        shuffle(ref ar);
        return ar;
    }

    private void shuffle<T>(ref T []ar)
    {
        T temp;
        int a=0;
        for (int i=0; i<ar.Length; i++)
        {
            a = Random.Range(0, ar.Length);
            temp = ar[a];
            ar[a] = ar[i];
            ar[i] = temp;
        }
    }


}
