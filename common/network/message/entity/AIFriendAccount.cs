using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;




public class AIFriendAccount
{
    private int _ttid;
    private int _ai_ttid;
    private string _model;   //模型
    private string _name;
    private string _content;
    private string _ai_type;
    private string _ctime;

    public AIFriendAccount() { }
    public int ttid { get => _ttid; set => _ttid = value; }
    public int ai_ttid { get => _ai_ttid; set => _ai_ttid = value; }
    public string model { get => _model; set => _model = value; }
    public string name { get => _name; set => _name = value; }
    public string content { get => _content; set => _content = value; }
    public string ai_type { get => _ai_type; set => _ai_type = value; }
    public string ctime { get => _ctime; set => _ctime = value; }


}

