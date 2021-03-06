﻿using System.Collections;
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

		Debug.Log ("MonoBehaviour Start in Title Scene");
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

            string messageBody = MySocketMessage.getMessageBodyString(recvBuf);
            Debug.Log("login response message = " + messageBody);

            stream.Flush();
            stream.Close();
            server.Close();

			if (messageBody.Equals ("login success")) {
				//request LobbyAddress
				string account = "panigale@naver.com";
				Debug.Log (" messageBody = " + account);
				messageBuf = MySocketMessage.addMessageHeader(account, MySocketMessage.MESSAGETYPE_REQUEST, MySocketMessage.MESSAGEKIND_LOBBYADDRESS);
				base.process ();

				messageBody = MySocketMessage.getMessageBodyString (recvBuf);
				Debug.Log ("LobbyAddress response message = " + messageBody);
			}
        }
    }

    public void clickLogin()
    {
        //UnityEngine.SceneManagement.SceneManager.LoadScene(1);

        //string message = "I am your father";
        //byte[] messageBuf = MySocketMessage.addMessageHeader(message, MySocketMessage.MESSAGETYPE_REQUEST, MySocketMessage.MESSAGEKIND_ECHO);

        string myAccount = "panigale@naver.com";
        byte[] messageBuf = MySocketMessage.addMessageHeader(myAccount, MySocketMessage.MESSAGETYPE_REQUEST, MySocketMessage.MESSAGEKIND_LOGIN);

        CDoLogin doLogin = new CDoLogin();
        doLogin.comm(messageBuf, ClientSocket.SERVERKIND.LOGIN_SERVER);
    }

}
