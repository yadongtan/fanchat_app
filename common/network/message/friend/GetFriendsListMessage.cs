using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;




public class GetFriendsListMessage : Message
{
    public const int type = 403;
    private int _ttid;

    public class userFriendDetails
    {
        int TTid;        //我的ttid
        int FriendTTid;    //朋友的ttid
        string FriendUsername;                 //朋友的username
        string Status;  //朋友的在线状态

        public userFriendDetails()
        {

        }

        public userFriendDetails(int ttid, int friendTTtid, string friendUsername, string status)
        {
            this.TTid = ttid;
            this.FriendTTid = friendTTtid;
            this.FriendUsername = friendUsername;
            this.Status = status;
        }


        public int ttid
        {
            get { return TTid; }
            set { TTid = value; }
        }
        public int friend_ttid
        {
            get { return FriendTTid; }
            set { FriendTTid = value; }
        }

        public string friend_username
        {
            get { return FriendUsername; }
            set { FriendUsername = value; }
        }

        public string status
        {
            get { return Status; }
            set { Status = value; }
        }

    }

    public GetFriendsListMessage()
    {
    }


    public GetFriendsListMessage(int ttid)
    {
        this.ttid = ttid;
    }

    public int ttid
    {
        get { return _ttid; }
        set { _ttid = value; }
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

