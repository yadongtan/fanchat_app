using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;




public class AddFriendMessage : Message
{
    public const int type = 402;
    private int _ttid;
    private int _friend_ttid;
    public AddFriendMessage()
    {
    }


    public AddFriendMessage(int ttid, int friendTTid)
    {
        this.ttid = ttid;
        this.friend_ttid = friendTTid;
    }

    public int ttid
    {
        get { return _ttid; }
        set { _ttid = value; }
    }
    public int friend_ttid
    {
        get { return _friend_ttid; }
        set { _friend_ttid = value; }
    }



    public override Message Invoke()
    {
        return AckMessage.GetOk();
    }

    public override int getMessageType()
    {
        return type;
    }

}

