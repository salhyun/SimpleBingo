using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Threading;

public class pingtest : MonoBehaviour {

    int count = 0;

    Socket server = null;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    // Use this for initialization
    void Start () {

        //StartCoroutine(comm());

        server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        IPEndPoint ipep = new IPEndPoint(IPAddress.Any, 9200);
        server.Bind(ipep);

        Thread thread = new Thread(new ThreadStart(comm));
	}

    public void comm()
    {

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    //IEnumerator comm()
    //{
    //    while(true)
    //    {

    //        Debug.Log("comm count = " + count++);

    //        yield return new WaitForSeconds(1.5f);
    //    }
    //}
}
