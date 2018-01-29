using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BingoCard {

    public const int BINGONUM_ROW = 5;
    public const int BINGONUM_COL = 5;

    public GameObject[,] numbers;
    public GameObject panel;

    public BingoCard()
    {
    }
    public BingoCard(string panelName, string spriteName, Vector2 pos, float size, Transform parent)
    {
        initialize(panelName, spriteName, pos, size, parent);
    }

    public void initialize(string panelName, string spriteName, Vector2 pos, float size, Transform parent)
    {
        numbers = new GameObject[BINGONUM_COL, BINGONUM_ROW];

        panel = new GameObject("Panel");
        panel.name = panelName;
        panel.AddComponent<CanvasRenderer>();
        UnityEngine.UI.Image image = panel.AddComponent<UnityEngine.UI.Image>();
        image.sprite = Resources.Load<Sprite>(spriteName);
        panel.transform.parent = parent;

        resetBingoCard(pos, size);
    }
    void resetBingoCard(Vector2 cardPos, float bingoPanelSize)
    {
        string[] bingoChar = { "B", "I", "N", "G", "O" };

        //float bingoPanelSize = 150.0f;// 400.0f; 5로 나누어 지는 숫자
        float captionHeight = bingoPanelSize * 0.125f;// 50.0f;

        GameObject canvas = panel.transform.parent.gameObject;
        Rect rtCanvas = canvas.GetComponent<RectTransform>().rect;
        //위치, 크기, pivot 등을 조정하고
        panel.GetComponent<RectTransform>().sizeDelta = new Vector2(bingoPanelSize, bingoPanelSize + captionHeight);
        panel.GetComponent<RectTransform>().pivot = new Vector2(0.0f, 1.0f);
        panel.GetComponent<RectTransform>().position = canvas.transform.TransformPoint(cardPos);

        //Rect를 얻어와야 적용된 사이즈로 얻어올 수 있다.
        Rect rtBingoPanel = panel.GetComponent<RectTransform>().rect;//RectTransform은 local 좌표임.
        //Rect rtBingoNumber = pfBingoNumber.GetComponent<RectTransform>().rect;

        float numWidth = rtBingoPanel.width / 5.0f;
        float numHeight = numWidth;// rtBingoPanel.height / 5.0f;

        float offsetX = rtBingoPanel.min.x;//좌측
        float offsetY = rtBingoPanel.min.y + (numHeight * BINGONUM_COL);//하단
        Vector3 pos;

        GameObject pfBingoNumber = Resources.Load("Prefabs/bingo number") as GameObject;

        for (int i = 0; i < 5; i++)
        {
            int[] columnNums = createColumnNum(i);
            for (int n = 0; n < 5; n++)
            {
                GameObject number = GameObject.Instantiate(pfBingoNumber, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity).gameObject;
                number.name = "number" + bingoChar[i] + "Line" + n;

                if (i == 2 && n == 2)
                    number.GetComponentInChildren<UnityEngine.UI.Text>().text = "FREE";
                else
                    number.GetComponentInChildren<UnityEngine.UI.Text>().text = columnNums[n].ToString();

                numbers[i, n] = number;

                //Rect rt = number.GetComponent<RectTransform>().rect;

                pos.x = offsetX + (i * numWidth) + numWidth / 2.0f;
                pos.y = offsetY - (n * numHeight) - numHeight / 2.0f;
                pos.z = number.transform.position.z;
                number.transform.position = panel.transform.TransformPoint(pos);//RectTransform은 local 좌표임. 그래서 월드좌표로 변환해준다.

                number.GetComponent<RectTransform>().sizeDelta = new Vector2(numWidth, numHeight);

                number.transform.Find("back").GetComponent<RectTransform>().sizeDelta = new Vector2(numWidth - 2, numHeight - 2);
                number.transform.Find("marker").GetComponent<RectTransform>().sizeDelta = new Vector2(numWidth - 2, numHeight - 2);
                number.transform.Find("Text").GetComponent<RectTransform>().sizeDelta = new Vector2(numWidth - 2, numHeight - 2);

                //number.GetComponentInChildren<UnityEngine.UI.Text>().GetComponent<RectTransform>().sizeDelta = new Vector2(numWidth, numHeight);

                number.transform.parent = panel.transform;
            }
        }
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

                        UnityEngine.UI.Image img = numbers[i, n].transform.Find("marker").gameObject.GetComponent<UnityEngine.UI.Image>();
                        img.color = new Color(img.color.r, img.color.g, img.color.b, 0.3f);
                        return;
                    }
                }
            }
        }
        Debug.Log("no match");
    }

    private int[] createColumnNum(int c)//빙고카드에 각 Column 별로 번호를 지정한다. ex)B라인:1-15, I라인:16-30, N라인:31-45....
    {
        int[] ar = new int[15];

        for (int i = 0; i < ar.Length; i++)
            ar[i] = i + (c * 15) + 1;

        shuffle(ref ar);
        return ar;
    }

    public static void shuffle<T>(ref T[] ar)//배열에 있는 숫자를 섞는다.
    {
        T temp;
        int a = 0;
        for (int i = 0; i < ar.Length; i++)
        {
            a = Random.Range(0, ar.Length);
            temp = ar[a];
            ar[a] = ar[i];
            ar[i] = temp;
        }
    }
}
