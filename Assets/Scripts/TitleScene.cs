using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System;
using UnityEngine;

public class TitleScene : MonoBehaviour {

	// Use this for initialization
	void Start () {

        String a1 = "I am your father";
        byte []b1 = System.Text.Encoding.UTF8.GetBytes(a1);

        String c1 = System.Text.Encoding.Default.GetString(b1);

        Debug.Log("STart");
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    class CDoLogin : ClientSocket
    {
        public override void process()
        {
            base.process();//메시지를 보내는 로직

            //여기에 보낸 이후 Response 로직을 작성하시오.

            string echo = MySocketMessage.getMessageBodyString(recvBuf);
            Debug.Log("echo message = " + echo);

            stream.Flush();
            stream.Close();
            server.Close();
        }

    }

    public void clickLogin()
    {
        //UnityEngine.SceneManagement.SceneManager.LoadScene(1);

        string message = "I am your father";
        byte[] messageBuf = MySocketMessage.addMessageHeader(message, MySocketMessage.MESSAGETYPE_REQUEST, MySocketMessage.MESSAGEKIND_ECHO);

        CDoLogin doLogin = new CDoLogin();
        doLogin.comm(messageBuf, ClientSocket.SERVERKIND.LOGIN_SERVER);
    }

}
