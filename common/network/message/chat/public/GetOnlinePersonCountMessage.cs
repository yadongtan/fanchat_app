using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;




public class GetOnlinePersonCountMessage : Message
{
    public const int type = 200;


    public override Message Invoke()
    {
        return null;
    }

    public override int getMessageType()
    {
        return type;
    }

}

