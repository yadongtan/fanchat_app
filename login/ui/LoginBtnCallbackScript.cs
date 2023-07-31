using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoginBtnCallbackScript : MonoBehaviour
{

    public InputField TTidInputField;
    public InputField PasswordInputField;

    public Text PromptText;

    public void Awake()
    {
        if(MyUserInfo.GetInstance.ttid != 0)
        {
            TTidInputField.text = Convert.ToString(MyUserInfo.GetInstance.ttid);
        }
        DontDestroyOnLoad(this);
    }

    public void Update()
    {

    }

    public IEnumerator ShowPromptText()
    {
        PromptText.gameObject.SetActive(true);
        for (float timeSet = 5; timeSet >= 0; timeSet -= Time.deltaTime)
            yield return null;
        PromptText.gameObject.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
    }


    public void OnClick()
    {
        int ttid = 0;
        try
        {
            string TTid = TTidInputField.text;
            ttid = Convert.ToInt32(TTid);
        }catch(Exception e)
        {
            PromptText.text = "请输入正确的TTid";
            StartCoroutine(ShowPromptText());
            return;
        }
        if(ttid == 0 || ttid <0)
        {
            PromptText.text = "TTid不能为空";
            StartCoroutine(ShowPromptText());
            return;
        }
        string password = PasswordInputField.text;
        SignInMessage msg = new SignInMessage();
        msg.ttid = ttid;
        msg.password = password;
        msg.deviceModel = SystemInfo.deviceModel;
        msg.deviceName = SystemInfo.deviceName;
        msg.deviceType = SystemInfo.deviceType.ToString();

        if(password == null || password == "")
        {
            PromptText.text = "密码不能为空";
            StartCoroutine(ShowPromptText());
            return;
        }
        // 尝试登录
        try
        {
            FantasticNetworkClient.SyncClient.WriteSyncLambda(msg, (ackMsg) =>
            {
                if (ackMsg.ack == AckMessage.Ok)
                {
                    // 登录成功!
                    Debug.Log("登录成功! msg:" + ackMsg.msg);
                    Loom.QueueOnMainThread((param) =>
                    {
                        PromptText.text = (string)ackMsg.msg;
                        StartCoroutine(ShowPromptText());
                        MyUserInfo.GetInstance.ttid = ttid;
                        MyUserInfo.GetInstance.username = (string)ackMsg.data;
                        SceneManager.LoadScene("MainScene");
                    }, null);
                }
                else
                {
                    //登录失败
                    Debug.Log("登录失败! msg:" + ackMsg.msg);
                    Loom.QueueOnMainThread((param) =>
                    {
                        PromptText.text = (string)ackMsg.msg;
                        StartCoroutine(ShowPromptText());
                    }, null);
                }
                return null;
            });
        }catch(ConnectionFailedException e)
        {
            //网络连接失败
            PromptText.text = e.Message;
            StartCoroutine(ShowPromptText());
        }
    }

    public void GoToSignUpScene()
    {
        SceneManager.LoadScene("SignUpScene");
    }
}
