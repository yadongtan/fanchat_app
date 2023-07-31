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
            PromptText.text = "��������ȷ��TTid";
            StartCoroutine(ShowPromptText());
            return;
        }
        if(ttid == 0 || ttid <0)
        {
            PromptText.text = "TTid����Ϊ��";
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
            PromptText.text = "���벻��Ϊ��";
            StartCoroutine(ShowPromptText());
            return;
        }
        // ���Ե�¼
        try
        {
            FantasticNetworkClient.SyncClient.WriteSyncLambda(msg, (ackMsg) =>
            {
                if (ackMsg.ack == AckMessage.Ok)
                {
                    // ��¼�ɹ�!
                    Debug.Log("��¼�ɹ�! msg:" + ackMsg.msg);
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
                    //��¼ʧ��
                    Debug.Log("��¼ʧ��! msg:" + ackMsg.msg);
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
            //��������ʧ��
            PromptText.text = e.Message;
            StartCoroutine(ShowPromptText());
        }
    }

    public void GoToSignUpScene()
    {
        SceneManager.LoadScene("SignUpScene");
    }
}
