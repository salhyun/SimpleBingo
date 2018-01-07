using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour {

    public struct MyBingoCard//추상적인 개념의 빙고카드
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

                            UnityEngine.UI.Image img = numbers[i, n].transform.FindChild("marker").gameObject.GetComponent< UnityEngine.UI.Image>();
                            img.color = new Color(img.color.r, img.color.g, img.color.b, 0.3f);
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

    ArrayList callerBalls = new ArrayList();

    private int BingoCounter = 0;

    public Camera mainCamera;
    public GameObject BingoPanel;
    public Transform pfCallerBall;
    public Transform pfBingoNumber;

	// Use this for initialization
	void Start () {

        string[] bingoChar = { "B", "I", "N", "G", "O" };

        testBitConvert();

        resetBingoCard(BingoPanel, myBingoCard);

        resetBingoNumber(BingoNumbers);

        StartCoroutine(callNumber());

        Debug.Log("Start end");
	}

    void resetBingoCard(GameObject panel, MyBingoCard bingoCard)
    {
        string[] bingoChar = { "B", "I", "N", "G", "O" };

        Rect rtBingoPanel = panel.GetComponent<RectTransform>().rect;//RectTransform은 local 좌표임.
        Rect rtBingoNumber = pfBingoNumber.GetComponent<RectTransform>().rect;

        float offsetX = rtBingoPanel.min.x;//좌측
        float offsetY = rtBingoPanel.min.y + (rtBingoNumber.height * BINGONUM_COL);//하단
        Vector3 pos;

        for (int i = 0; i < 5; i++)
        {
            int[] columnNums = createColumnNum(i);
            for (int n = 0; n < 5; n++)
            {
                GameObject number = Instantiate(pfBingoNumber, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity).gameObject;
                number.name = "number" + bingoChar[i] + "Line" + n;

                if (i == 2 && n == 2)
                    number.GetComponentInChildren<UnityEngine.UI.Text>().text = "FREE";
                else
                    number.GetComponentInChildren<UnityEngine.UI.Text>().text = columnNums[n].ToString();

                bingoCard.numbers[i, n] = number;

                Rect rt = number.GetComponent<RectTransform>().rect;

                pos.x = offsetX + (i * rt.width) + rt.width / 2.0f;
                pos.y = offsetY - (n * rt.height) - rt.height / 2.0f;
                pos.z = number.transform.position.z;
                number.transform.position = panel.transform.TransformPoint(pos);//RectTransform은 local 좌표임. 그래서 월드좌표로 변환해준다.
                number.transform.parent = panel.transform;
            }
        }

    }

    void testBitConvert()
    {
        int num = 10;

        byte[] message = MySocketMessage.addMessageHeader(System.Text.Encoding.Default.GetBytes("I am your father"), 12, 32);

        int type = MySocketMessage.getMessageType(message);
        int kind = MySocketMessage.getMessageKind(message);
        int size = MySocketMessage.getMessageSize(message);

        string body = MySocketMessage.getMessageBodyString(message);

        Debug.Log("testBitConvert");
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator callNumber()
    {
        while(true)
        {
            int callerNumber = BingoNumbers[BingoCounter++].number;
            Debug.Log("BingoNumber = " + callerNumber);

            StartCoroutine("generateCallerBall", callerNumber);

            if (callerBalls.Count > 0)
            {
                foreach(GameObject ball in callerBalls)
                {
                    ball.GetComponent<callerBallAnimation>().startAnimation();
                }
            }

            yield return new WaitForSeconds(2);
        }
    }

    IEnumerator generateCallerBall(int callerNumber)
    {
        yield return new WaitForSeconds(0.5f);

        Rect rtCallerBall = pfCallerBall.GetComponent<RectTransform>().rect;
        Rect rtPanel = BingoPanel.GetComponent<RectTransform>().rect;

        Rect rt = GetComponent<RectTransform>().rect;

        Vector3 pos = new Vector3(rtPanel.x + rtCallerBall.width / 2.0f, rtPanel.max.y + rtCallerBall.height * 0.75f + 0.0f, 0.0f);
        pos = BingoPanel.GetComponent<RectTransform>().TransformPoint(pos);

        var callerBall = Instantiate(pfCallerBall, pos, Quaternion.identity);
        callerBall.GetComponentInChildren<UnityEngine.UI.Text>().text = callerNumber.ToString();
        callerBall.parent = transform;

        callerBalls.Add(callerBall.gameObject);

        myBingoCard.checkNumber(callerNumber);
    }

    private void resetBingoNumber(BingoNumber []bingoNumber)//게임 진행시 번호호출순서를 섞는다.
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

    private int[] createColumnNum(int c)//빙고카드에 각 Column 별로 번호를 지정한다. ex)B라인:1-15, I라인:16-30, N라인:31-45....
    {
        int[] ar = new int[15];

        for (int i = 0; i < ar.Length; i++)
            ar[i] = i + (c * 15) + 1;

        shuffle(ref ar);
        return ar;
    }

    private void shuffle<T>(ref T []ar)//배열에 있는 숫자를 섞는다.
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
