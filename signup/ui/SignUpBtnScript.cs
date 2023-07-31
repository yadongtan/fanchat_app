using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SignUpBtnScript : MonoBehaviour
{
    public InputField PhoneText;
    public InputField UsernameText;
    public InputField PasswordText;
    public Text PromptText;

    public void Awake()
    {
        DontDestroyOnLoad(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public IEnumerator ShowPromptText()
    {
        PromptText.gameObject.SetActive(true);
        for (float timeSet = 5; timeSet >= 0; timeSet -= Time.deltaTime)
            yield return null;
        PromptText.gameObject.SetActive(false);
    }

    public IEnumerator ShowPromptText(string text)
    {
        PromptText.gameObject.SetActive(true);
        PromptText.text = text;
        for (float timeSet = 5; timeSet >= 0; timeSet -= Time.deltaTime)
            yield return null;
        PromptText.gameObject.SetActive(false);
    }

    public void OnClick()
    {
        string username = UsernameText.text;
        string password = PasswordText.text;
        string phone = PhoneText.text;
        if(phone == null || phone.Trim() == "")
        {
            StartCoroutine(ShowPromptText("电话号码不能为空"));
        }
        else if(!Regex.Match(phone, "^((13[0-9])|(15[^4,\\D])|(18[0-9]))\\d{8}$").Success)
        {
            StartCoroutine(ShowPromptText("请输入正确的电话号码"));
        }
        else if(username == null || username.Trim() == "")
        {
            StartCoroutine(ShowPromptText("用户名不能为空"));
        }
        else if(username.Length > 16 || username.Length < 2)
        {
            StartCoroutine(ShowPromptText("用户名长度应为2-16个字符"));
        }
        else if(password == null || password.Trim() == "")
        {
            StartCoroutine(ShowPromptText("密码不能为空"));
        }
        else if(password.Length < 6 || password.Length > 32)
        {
            StartCoroutine(ShowPromptText("密码长度应为2-16个字符"));
        }
        else
        {
            SignUpMessage msg = new SignUpMessage();
            msg.username = username;
            msg.password = password;
            msg.phone = phone;
            FantasticNetworkClient.SyncClient.WriteSyncLambda(msg, ackMsg =>
            {
                //注册失败
                if (ackMsg.ack != AckMessage.Ok)
                {
                    Loom.QueueOnMainThread((param) =>
                    {
                        StartCoroutine(ShowPromptText("注册失败\n" + (string)ackMsg.msg));
                    }, null);
                    return null;
                }
                //注册成功, 获取ttid, 并提示用户返回登录
                int ttid = (int)ackMsg.data;
                MyUserInfo.GetInstance.ttid = ttid;
                Loom.QueueOnMainThread((param) =>
                {
                    StartCoroutine(ShowPromptText("注册成功!"));
                    SceneManager.LoadScene("SignUpSuccessScene");
                }, null);

                return null;
            });
        }
        
    }

    public void GoToLoginScene()
    {
        SceneManager.LoadScene("LoginScene");
    }


}
