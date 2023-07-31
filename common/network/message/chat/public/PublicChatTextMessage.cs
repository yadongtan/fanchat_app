using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;




public class PublicChatTextMessage : Message
{
    public const int type = 202;
    private int _ttid;
    private string _username;
    private long _time;
    private string _text; //消息内容


    public long time
    {
        get { return _time; }
        set { _time = value; }
    }

    public string text
    {
        get { return _text; }
        set { _text = value; }
    }

    public int ttid
    {
        get { return _ttid; }
        set { _ttid = value; }
    }

    public string username
    {
        get { return _username; }
        set { _username = value; }
    }



    public override Message Invoke()
    {
        // 收到别人发的信息
        lock (MyUserInfo.GetInstance.receivedMessage)
        {
            MyUserInfo.GetInstance.receivedMessage.AddLast(this);
        }
        return AckMessage.GetOk();
    }

    public override int getMessageType()
    {
        return type;
    }

}

