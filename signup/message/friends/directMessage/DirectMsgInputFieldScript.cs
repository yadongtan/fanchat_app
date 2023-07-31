using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DirectMsgInputFieldScript : MonoBehaviour
{
    public Text InputText;
    public Button DisabledSendMsgBtn;
    public Button EnabledSendMsgBtn;
    public DirectChatPanelManager cpm; 
    public void Awake()
    {
        DisabledSendMsgBtn.gameObject.SetActive(true);
        EnabledSendMsgBtn.gameObject.SetActive(false);
    }

    public void OnInputChanged(string nothing)
    {
    }

    public void OnClickSendMsgBtn()
    {
        FriendDirectMessage msg = new FriendDirectMessage();
        msg.from_ttid = MyUserInfo.GetInstance.ttid;
        msg.dest_ttid = MyUserInfo.GetInstance.currentChatTTid;
        msg.time = TimeUitls.getCurrentTimeSpan();
        msg.text = InputText.text;
        msg.account_type = MyUserInfo.GetInstance.cururentChatAcountType;
        this.GetComponent<InputField>().text = "";
        FantasticNetworkClient.SyncClient.WriteSyncLambda(msg, ackMsg =>
        {
            if (ackMsg.ack == AckMessage.Ok)
            {
                Debug.Log("发送消息给[" + msg.dest_ttid + "]: " + msg.text + " 成功 ");
                Loom.QueueOnMainThread((param) =>
                {
                    cpm.AddBubble(MyUserInfo.GetInstance.username, msg.text, true);
                }, null);
                // 如果历史列表为null
                if (MyUserInfo.GetInstance.historyFriendMessage[msg.dest_ttid] == null)
                {
                    lock (MyUserInfo.GetInstance.historyFriendMessage)
                    {
                        if (MyUserInfo.GetInstance.historyFriendMessage[msg.dest_ttid] == null)
                        {
                            MyUserInfo.GetInstance.historyFriendMessage[msg.dest_ttid] = new LinkedList<FriendDirectMessage>();
                        }
                    }
                }
                    // 添加至历史列表
                ((LinkedList<FriendDirectMessage>)MyUserInfo.GetInstance.historyFriendMessage[msg.dest_ttid])
                        .AddLast(msg);
            }
            else
            {
                Debug.Log("发送消息给[" + msg.dest_ttid + "]: " + msg.text + " 失败 ");
            }
            return null;
        });
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        string input = InputText.text;
        if (input == null || input == "")
        {
            DisabledSendMsgBtn.gameObject.SetActive(true);
            EnabledSendMsgBtn.gameObject.SetActive(false);
        }
        else
        {
            DisabledSendMsgBtn.gameObject.SetActive(false);
            EnabledSendMsgBtn.gameObject.SetActive(true);
        }
    }
}
