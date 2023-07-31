using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;



// 私发给朋友的消息
public class FriendDirectMessage : Message
{
    public const int type = 404;

    private int _from_ttid;
    private int _dest_ttid;

    private long _time;
    private string _text; //消息内容
    private AccountType _accountType = AccountType.User;

    public AccountType account_type
    {
        get { return _accountType; }
        set { _accountType = value; }
    }

    public int from_ttid
    {
        get { return _from_ttid; }
        set { _from_ttid = value; }
    }

    public int dest_ttid
    {
        get { return _dest_ttid; }
        set { _dest_ttid = value; }
    }

    public string text
    {
        get { return _text; }
        set { _text = value; }
    }
    public long time
    {
        get { return _time; }
        set { _time = value; }
    }
    public FriendDirectMessage()
    {
    }


    public FriendDirectMessage(int fromTTid, int destTTid, long time, string text)
    {
        this.from_ttid = fromTTid;
        this.dest_ttid = destTTid;
        this.time = time;
        this.text = text;
    }




    public override Message Invoke()
    {
        // 新消息来了
        lock (MyUserInfo.GetInstance.newFriendMessages)
        {
            int fri_ttid = this.from_ttid == MyUserInfo.GetInstance.ttid ? this.dest_ttid : this.from_ttid;
            // 新消息列表
            LinkedList<FriendDirectMessage> lists = (LinkedList<FriendDirectMessage>) MyUserInfo.GetInstance.newFriendMessages[fri_ttid];
            // DCL 
            if (lists == null)
            {
                lock (MyUserInfo.GetInstance.newFriendMessages)
                {
                    if (MyUserInfo.GetInstance.newFriendMessages[fri_ttid] == null)
                    {
                        MyUserInfo.GetInstance.newFriendMessages[fri_ttid] = new LinkedList<FriendDirectMessage>();
                    }
                }
            }
            LinkedList<FriendDirectMessage> lists2 = (LinkedList<FriendDirectMessage>)MyUserInfo.GetInstance.newFriendMessages[fri_ttid];
            lists2.AddLast(this);
            Debug.Log("newFriendMessages长度: " + lists2.Count);
        }
        return AckMessage.GetOk();
    }

    public override int getMessageType()
    {
        return type;
    }

}

