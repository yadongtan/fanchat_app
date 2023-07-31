using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;



//登录 消息

public class SignInMessage : Message
{
    public const int SignInMessageType = 101;
    private int _ttid;
    private string _username;
    private string _password;
    private string _deviceModel;
    private string _deviceName;
    private string _deviceType;

    public string deviceModel
    {
        get { return _deviceModel; }
        set { _deviceModel = value; }
    }
    public string deviceName
    {
        get { return _deviceName; }
        set { _deviceName = value; }
    }

    public string deviceType
    {
        get { return _deviceType; }
        set { _deviceType = value; }
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
        return SignInMessageType;
    }

}

