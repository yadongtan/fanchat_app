using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;




public class GetAIFriendsListMessage : Message
{
    public const int type = 502;
    private int _ttid;

    public int ttid { get => _ttid; set => _ttid = value; }

    public GetAIFriendsListMessage()
    {

    }
    public GetAIFriendsListMessage(int user_ttid)
    {
        _ttid = user_ttid;
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

