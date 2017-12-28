using System;

public class MySocketMessage {

    public static int MESSAGETYPE_REQUEST = 1;
    public static int MESSAGETYPE_RESPONSE = 2;

    public static int MESSAGEKIND_ECHO = 0;
    public static int MESSAGEKIND_SEARCH = 1;
    public static int MESSAGEKIND_ACCOUNT = 2;

    private static int MESSAGETYPENUM = 3;//type + kind + size

    private static int mIntSizeInByte = sizeof(int) / sizeof(byte);// Integer.SIZE / Byte.SIZE;
    private static int mHeaderSize = mIntSizeInByte * MESSAGETYPENUM;

    public static byte[] addMessageHeader(byte[] message, int messageType, int messageKind)
    {
        int totalSize = mHeaderSize + message.Length;
        byte[] msgType = BitConverter.GetBytes(messageType);
        byte[] msgKind = BitConverter.GetBytes(messageKind);
        byte[] msgHSize = BitConverter.GetBytes(totalSize);

        byte[] result = new byte[totalSize];

        int destPos = 0;
        Array.Copy(msgType, 0, result, destPos, msgType.Length);
        destPos += msgType.Length;

        Array.Copy(msgKind, 0, result, destPos, msgKind.Length);
        destPos += msgKind.Length;

        Array.Copy(msgHSize, 0, result, destPos, msgHSize.Length);
        destPos += msgHSize.Length;

        Array.Copy(message, 0, result, destPos, message.Length);

        return result;
    }

    public static int getMessageType(byte []buf)
    {
        return BitConverter.ToInt32(buf, 0);
    }
    public static int getMessageKind(byte []buf)
    {
        return BitConverter.ToInt32(buf, mIntSizeInByte);
    }
    public static int getMessageSize(byte []buf)
    {
        return BitConverter.ToInt32(buf, mIntSizeInByte * 2);
    }
    public static byte[] getMessageBody(byte []buf)
    {
        int totalSize = getMessageSize(buf);
        int bodySize = totalSize - mHeaderSize;
        byte[] result = new byte[bodySize];
        Array.Copy(buf, mHeaderSize, result, 0, bodySize);
        return result;
    }
    public static String getMessageBodyString(byte []buf)
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

}
