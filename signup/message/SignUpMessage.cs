using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;



//注册 消息

public class SignUpMessage : Message
{
    public const int type = 301;
    private string _username;
    private string _password;
    private string _phone;


    public string phone
    {
        get { return _phone; }
        set { _phone = value; }
    }

    public string username
    {
        get { return _username; }
        set { _username = value; }
    }

    public string password
    {
        get { return _password; }
        set { _password = value; }
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

