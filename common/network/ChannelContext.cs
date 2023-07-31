using System;
using System.Net.Sockets;
using System.Threading;



// 用于在处理链中传递的上下文
public class ChannelContext
{
    public Socket client;
    public HandlerChain chain;

    public int frameIdIncrement = 0;

    public string GenerateFrameId()
    {
        int incrId = Interlocked.Increment(ref frameIdIncrement);
        string frameIdStr = IntToString(MyUserInfo.GetInstance.ttid, 9) + IntToString(incrId, 10) ;
        return frameIdStr;
    }


    public string IntToString(int i, int numLength)
    {
        //数字右边部分
        string right = Convert.ToString(i);
        //左边差多少位补多少个0
        int count = numLength - right.Length; 

        string left = "";

        for (int j = 0; j < count; j++){
            left += "0";
        }
        return left + right;
    }


    public ChannelContext(Socket client)
    {
        this.client = client;
        this.chain = new HandlerChain();
    }

    public void AddHandler(Handler handler)
    {
        chain.AddHandler(handler);
    }

    public AckMessage Write(Message message)
    {
        return (AckMessage)chain.Write(this, message);
    }
}
