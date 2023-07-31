using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;



// handler

public interface Handler
{

    //读入
    object Read(ChannelContext ctx, object data);
    //写出
    object Write(ChannelContext ctx, object data);


}
