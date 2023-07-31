using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;



//登录 消息

public class AckMessage : Message
{
    // 正常响应
    public const int Ok = 200;

    // 不合法消息体
    public const int Invalid = 400;

    // 失败消息体
    public const int Failed = 500;


    public const int AckMessageType = 100;

    private int _ack;
    private string _msg;
    private object _data;

    public static AckMessage GetOk()
    {
        AckMessage ack = new AckMessage();
        ack.ack = AckMessage.Ok;
        ack.msg = "成功";
        return ack;
    }

    public static AckMessage GetInvalid()
    {
        AckMessage ack = new AckMessage();
        ack.ack = AckMessage.Invalid;
        ack.msg = "非法操作";
        return ack;
    }

    public static AckMessage GetFailed()
    {
        AckMessage ack = new AckMessage();
        ack.ack = AckMessage.Failed;
        ack.msg = "操作失败";
        return ack;
    }
    public int ack
    {
        get { return _ack; }
        set { _ack = value; }
    }

    public string msg
    {
        get { return _msg; }
        set { _msg = value; }
    }
    public object data
    {
        get { return _data; }
        set { _data = value; }
    }

    public override Message Invoke()

    {
        return null;
    }

    public override int getMessageType()
    {
        return AckMessageType;
    }


    public override string ToString()
    {
        return "ack : " + ack + " \t " + "msg : " + msg + "\t " + "data : " + data + "\t ";  
    }
}

