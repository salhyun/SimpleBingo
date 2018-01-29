using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour {

    struct BingoNumber
    {
        public string bingoChar;
        public int number;
    }

    public const int BINGOALLNUM = 75;

    private BingoNumber[] mBingoNumbers = new BingoNumber[BINGOALLNUM];

    //private BingoCard myBingoCard = new BingoCard();
    private BingoCard[] myBingoCards=null;

    ArrayList mCallerBalls = new ArrayList();

    private int mBingoCounter = 0;

    float mBingoPanelSize = 150.0f;
    float mBingoPanelCaptionHeight = 0.125f;

    public Camera mMainCamera;
    public Transform mpfCallerBall;
    //public Transform mpfBingoNumber;

	// Use this for initialization
	void Start () {

        string[] bingoChar = { "B", "I", "N", "G", "O" };

        Rect rtCanvas = this.GetComponent<RectTransform>().rect;

        myBingoCards = new BingoCard[2];
        myBingoCards[0] = new BingoCard();
        myBingoCards[0].initialize("Panigale1", "images/bingoCard", new Vector2(rtCanvas.min.x, rtCanvas.max.y), 150.0f, this.transform);

        myBingoCards[1] = new BingoCard();
        myBingoCards[1].initialize("Panigale2", "images/bingoCard", new Vector2(rtCanvas.min.x, rtCanvas.max.y - 180.0f), 150.0f, this.transform);

        resetBingoNumber(mBingoNumbers);

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
            int callerNumber = mBingoNumbers[mBingoCounter++].number;
            Debug.Log("BingoNumber = " + callerNumber);

            //Canvas 위치에다가 볼 생성 시킴
            StartCoroutine(generateCallerBall(callerNumber));

            if (mCallerBalls.Count > 0)
            {
                foreach(GameObject ball in mCallerBalls)
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

        float callerBallSize = mBingoPanelSize / 5.0f;

        //Rect rtCallerBall = mpfCallerBall.GetComponent<RectTransform>().rect;
        //Rect rtPanel = mBingoPanel.GetComponent<RectTransform>().rect;
        Rect rt = this.GetComponent<RectTransform>().rect;

        var callerBall = Instantiate(mpfCallerBall, Vector3.zero, Quaternion.identity);
        callerBall.GetComponent<RectTransform>().sizeDelta = new Vector2(callerBallSize, callerBallSize);
        callerBall.transform.Find("Text").GetComponent<RectTransform>().sizeDelta = new Vector2(callerBallSize*0.7f, callerBallSize*0.7f);
        Rect rtCallerBall = callerBall.GetComponent<RectTransform>().rect;
        //Vector3 pos = new Vector3(rtPanel.x + rtCallerBall.width / 2.0f, rtPanel.max.y + rtCallerBall.height * 0.75f + 0.0f, 0.0f);
        Vector3 pos = new Vector3(rt.center.x, rt.max.y-rtCallerBall.height/2.0f, 0.0f);
        callerBall.transform.position = this.GetComponent<RectTransform>().TransformPoint(pos);
        //pos = mBingoPanel.GetComponent<RectTransform>().TransformPoint(pos);
        //callerBall.transform.position = mBingoPanel.GetComponent<RectTransform>().TransformPoint(pos);

        callerBall.GetComponentInChildren<UnityEngine.UI.Text>().text = callerNumber.ToString();
        callerBall.GetComponent<callerBallAnimation>().rotSpeed = callerBallSize;
        callerBall.parent = transform;

        mCallerBalls.Add(callerBall.gameObject);

        //myBingoCard.checkNumber(callerNumber);
        //bingoCard.checkNumber(callerNumber);

        foreach(BingoCard card in myBingoCards)
        {
            card.checkNumber(callerNumber);
        }
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

        //shuffle(ref bingoNumber);
        BingoCard.shuffle(ref bingoNumber);

        mBingoCounter = 0;
    }
}
