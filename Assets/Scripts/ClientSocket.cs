using System.Net.Sockets;
using System.Threading;

public class ClientSocket {

    public enum SERVERKIND
    {
        LOGIN_SERVER,
        LOBBY_SERVER
    }

    protected SERVERKIND serverKind;
    protected byte[] messageBuf;
    protected byte[] recvBuf = new byte[1024];

    protected TcpClient server=null;
    protected NetworkStream stream;

    public ClientSocket()
    {
    }

    private void connect(SERVERKIND serverKind)
    {
        if (serverKind == SERVERKIND.LOGIN_SERVER)
        {
            server = new TcpClient("127.0.0.1", 9100);
        }
        else if (serverKind == SERVERKIND.LOBBY_SERVER)
        {
        }
        else
        {
        }
    }

    public void comm(byte[] buf, SERVERKIND kind)
    {
        serverKind = kind;
        messageBuf = buf;

        Thread thread = new Thread(new ThreadStart(process));
        thread.Start();
    }

    public virtual void process()
    {
        connect(serverKind);

        stream = server.GetStream();
        stream.Write(messageBuf, 0, messageBuf.Length);

        int len = 0, nMessageSize = 0;

        len = stream.Read(recvBuf, 0, recvBuf.Length);

        MySocketMessage.convertEndian(recvBuf);

        //유효성 검사
        int nIdentify = MySocketMessage.getMessageIdentify(recvBuf);
        if (nIdentify != MySocketMessage.MESSAGE_IDENTIFY)//유효하지 않는 메시지
        {
            stream.Flush();
            stream.Close();
            server.Close();
            return;
        }

        nMessageSize = MySocketMessage.getMessageSize(recvBuf);
        while (len < nMessageSize)
        {
            len += stream.Read(recvBuf, len, recvBuf.Length);
            //데이타 전송중 중지 되었을 경우 처리
        }
    }

}
