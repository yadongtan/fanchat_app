using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MsgInputFieldScript : MonoBehaviour
{
    public Text InputText;
    public Button DisabledSendMsgBtn;
    public Button EnabledSendMsgBtn;
    public ChatPanelManager cpm; 
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
        PublicChatTextMessage msg = new PublicChatTextMessage();
        msg.ttid = MyUserInfo.GetInstance.ttid;
        msg.username = MyUserInfo.GetInstance.username;
        msg.time = TimeUitls.getCurrentTimeSpan();
        msg.text = InputText.text;
        this.GetComponent<InputField>().text = "";
        FantasticNetworkClient.SyncClient.WriteSyncLambda(msg, ackMsg =>
        {
            if(ackMsg.ack == AckMessage.Ok)
            {
                Debug.Log("发送[所有人]消息: " + msg.text + " 成功 ");

                Loom.QueueOnMainThread((param) =>
                {
                    cpm.AddBubble(MyUserInfo.GetInstance.username ,msg.text, true);
                }, null);
            }
            else
            {
                Debug.Log("发送[所有人]消息: " + msg.text + " 失败 ");
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
