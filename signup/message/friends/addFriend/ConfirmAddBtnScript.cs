using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ConfirmAddBtnScript : MonoBehaviour
{

    public InputField FriendTTidInputField;
    public Text ShowPromptAfterConfirm;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BackToFriendScene()
    {
        SceneManager.LoadScene("FriendsScene");
    }
    public void OnClickToAddFriend()
    {
        int friendTTid = -1;
        try
        {
            friendTTid = Convert.ToInt32(FriendTTidInputField.text);
        }catch(Exception e)
        {

            ShowPromptAfterConfirm.text = "请输入正确的ttid";
            return;
        }

        Debug.Log("请求添加朋友[" + friendTTid + "]");
        AddFriendMessage msg = new AddFriendMessage(MyUserInfo.GetInstance.ttid, friendTTid);


        FantasticNetworkClient.SyncClient.WriteSyncLambda(msg, (ackMsg) =>
        {
            if (ackMsg.ack == AckMessage.Ok)
            {
                Debug.Log("添加成功! msg:" + ackMsg.msg);
                Loom.QueueOnMainThread((param) =>
                {
                    ShowPromptAfterConfirm.text = "添加用户[" + friendTTid + "]成功!";
                    ShowPromptAfterConfirm.gameObject.SetActive(true);
                }, null);
            }
            else
            {
                Debug.Log("添加失败! msg:" + ackMsg.msg);
                Loom.QueueOnMainThread((param) =>
                {
                    ShowPromptAfterConfirm.text = "添加用户[" + friendTTid + "]失败!";
                    ShowPromptAfterConfirm.gameObject.SetActive(true);
                }, null);
            }
            return null;
        });

    }

}
