using System;

public class MySocketMessage {
	
	public static int MESSAGE_IDENTIFY=333;

    public static int MESSAGETYPE_REQUEST = 1;
    public static int MESSAGETYPE_RESPONSE = 2;

    public static int MESSAGEKIND_ECHO = 0;
	public static int MESSAGEKIND_PINGTEST = 1;
    public static int MESSAGEKIND_SEARCH = 2;
    public static int MESSAGEKIND_LOGIN = 3;
	public static int MESSAGEKIND_LOBBYADDRESS = 4;
	public static int MESSAGEKIND_LOBBYENTERANCE = 5;

    private static int MESSAGETYPENUM = 4;//identify + type + kind + size

    private static int mIntSizeInByte = sizeof(int) / sizeof(byte);// Integer.SIZE / Byte.SIZE;
    private static int mHeaderSize = mIntSizeInByte * MESSAGETYPENUM;

    public static byte[] addMessageHeader(string message, int messageType, int messageKind)
    {
        byte[] messageBuf = System.Text.Encoding.UTF8.GetBytes(message);
        byte []msgIdentify = BitConverter.GetBytes(MESSAGE_IDENTIFY);
        byte[] msgType = BitConverter.GetBytes(messageType);
        byte[] msgKind = BitConverter.GetBytes(messageKind);

        int totalSize = mHeaderSize + messageBuf.Length;
        byte[] msgSize = BitConverter.GetBytes(totalSize);

        if (BitConverter.IsLittleEndian)
        {
            Array.Reverse(msgIdentify);
            Array.Reverse(msgType);
            Array.Reverse(msgKind);
            Array.Reverse(msgSize);
            //Array.Reverse(messageBuf);//지금은(PC) 문자열은 Big Endian으로 저장된다(int형은 Little Endian). 다른 플렛폼에서는 모르겠다
        }

        byte[] result = new byte[totalSize];

        int destPos = 0;
        Array.Copy(msgIdentify, 0, result, destPos, msgType.Length);
		destPos += msgType.Length;

        Array.Copy(msgType, 0, result, destPos, msgType.Length);
        destPos += msgType.Length;

        Array.Copy(msgKind, 0, result, destPos, msgKind.Length);
        destPos += msgKind.Length;

        Array.Copy(msgSize, 0, result, destPos, msgSize.Length);
        destPos += msgSize.Length;

        Array.Copy(messageBuf, 0, result, destPos, messageBuf.Length);

        return result;
    }
	
	public static int getMessageIdentify(byte []buf)
	{
		return BitConverter.ToInt32(buf, 0);
	}
    public static int getMessageType(byte []buf)
    {
		return BitConverter.ToInt32(buf, mIntSizeInByte);
    }
    public static int getMessageKind(byte []buf)
    {
        return BitConverter.ToInt32(buf, mIntSizeInByte * 2);
    }
    public static int getMessageSize(byte []buf)
    {
        return BitConverter.ToInt32(buf, mIntSizeInByte * 3);
    }
    public static byte[] getMessageBody(byte []buf)
    {
        int totalSize = getMessageSize(buf);
        int bodySize = totalSize - mHeaderSize;
        byte[] result = new byte[bodySize];
        Array.Copy(buf, mHeaderSize, result, 0, bodySize);
        return result;
    }
    public static string getMessageBodyString(byte []buf)
    {
        int totalSize = getMessageSize(buf);
        int bodySize = totalSize - mHeaderSize;
        byte[] body = new byte[bodySize];
        Array.Copy(buf, mHeaderSize, body, 0, bodySize);

        return System.Text.Encoding.Default.GetString(body);
    }
    public static int getHeaderSize()
    {
        return mHeaderSize;
    }

    public static void convertEndian(byte []buf)
    {
        if(BitConverter.IsLittleEndian)
        {
            Array.Reverse(buf, 0, mIntSizeInByte);
            Array.Reverse(buf, mIntSizeInByte, mIntSizeInByte);
            Array.Reverse(buf, mIntSizeInByte * 2, mIntSizeInByte);
            Array.Reverse(buf, mIntSizeInByte * 3, mIntSizeInByte);

            //지금은(PC) 문자열은 Big Endian으로 저장된다.(int형은 Little Endian) 다른 플렛폼에서는 모르겠다
            //int size = getMessageSize(buf);
            //Array.Reverse(buf, mIntSizeInByte * 4, size - mHeaderSize);
        }
    }
}
