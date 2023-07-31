using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;




public class PublicChatVideoMessage : Message
{
    public const int type = 203;
    private int _ttid;
    private string _username;

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
        return null;
    }

    public override int getMessageType()
    {
        return type;
    }

}

