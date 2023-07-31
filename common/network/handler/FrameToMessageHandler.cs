using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;



// 心跳检测, 与服务器断开连接时重连
public class IldeStatusHandler : Handler
{

    public object Read(ChannelContext ctx, object data)
    {
        return data;
	}

    public object Write(ChannelContext ctx, object data)
    {
        return data;
    }

    public object Write(ChannelContext ctx, object data, string frameId)
    {
        return data;
    }
}
