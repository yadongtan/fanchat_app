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
            StartCoroutine(ShowPromptText("�绰���벻��Ϊ��"));
        }
        else if(!Regex.Match(phone, "^((13[0-9])|(15[^4,\\D])|(18[0-9]))\\d{8}$").Success)
        {
            StartCoroutine(ShowPromptText("��������ȷ�ĵ绰����"));
        }
        else if(username == null || username.Trim() == "")
        {
            StartCoroutine(ShowPromptText("�û�������Ϊ��"));
        }
        else if(username.Length > 16 || username.Length < 2)
        {
            StartCoroutine(ShowPromptText("�û�������ӦΪ2-16���ַ�"));
        }
        else if(password == null || password.Trim() == "")
        {
            StartCoroutine(ShowPromptText("���벻��Ϊ��"));
        }
        else if(password.Length < 6 || password.Length > 32)
        {
            StartCoroutine(ShowPromptText("���볤��ӦΪ2-16���ַ�"));
        }
        else
        {
            SignUpMessage msg = new SignUpMessage();
            msg.username = username;
            msg.password = password;
            msg.phone = phone;
            FantasticNetworkClient.SyncClient.WriteSyncLambda(msg, ackMsg =>
            {
                //ע��ʧ��
                if (ackMsg.ack != AckMessage.Ok)
                {
                    Loom.QueueOnMainThread((param) =>
                    {
                        StartCoroutine(ShowPromptText("ע��ʧ��\n" + (string)ackMsg.msg));
                    }, null);
                    return null;
                }
                //ע��ɹ�, ��ȡttid, ����ʾ�û����ص�¼
                int ttid = (int)ackMsg.data;
                MyUserInfo.GetInstance.ttid = ttid;
                Loom.QueueOnMainThread((param) =>
                {
                    StartCoroutine(ShowPromptText("ע��ɹ�!"));
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
