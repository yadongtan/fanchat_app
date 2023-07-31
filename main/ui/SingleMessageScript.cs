using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SingleMessageScript : MonoBehaviour
{
    public Text MessageUsername;
    public Text MessageContent;
    public Text TimeText;
    public Image MessageCountPrompt;
    public Text MessageCountText;
    public Image HeadImg;
    public Texture2D aiTexture2D;

    private int _TTid;
    private AccountType accountType = AccountType.User;
    private int _MessageCount;

    public int TTid
    {
        get { return _TTid; }
        set { _TTid = value; }
    }
    public int MessageCount
    {
        get { return _MessageCount; }
        set { _MessageCount = value; }
    }

    // �����þ���Ϣ��ʾ���б�, ���Ҳ���ʾ����Ϣ����
    public void SetOldMessage(MyUserInfo.FriendsInfo account, string username, string content, DateTime time, int ttid, AccountType accountType)
    {
        if (account.type == AccountType.AI && account != null)
        {
            UnityUtils.SetTexture2DForImage(this.HeadImg, aiTexture2D);
            this.accountType = AccountType.AI;
        }

        this.accountType = accountType;
        MessageCountPrompt.gameObject.SetActive(false);
        _MessageCount = 0;

        string t = time.ToString();
        this.TimeText.text = t;

        this.MessageUsername.text = username;
        this.MessageContent.text = content;

        this._TTid = ttid;
    }

    // ��������Ϣ����ʾδ����Ϣ����
    public void SetNewMessage(MyUserInfo.FriendsInfo account, string username, string content, DateTime time, int ttid, AccountType accountType)
    {
        if(account.type == AccountType.AI && account != null)
        {
            UnityUtils.SetTexture2DForImage(this.HeadImg, aiTexture2D);
            this.accountType = AccountType.AI;
        }
        this.accountType = accountType;
        _MessageCount++;
        SetNewMessage(account, username, content,time, ttid, _MessageCount, accountType);
    }

    // ��������Ϣ����ʾδ����Ϣ����
    public void SetNewMessage(MyUserInfo.FriendsInfo account, string username, string content, DateTime time, int ttid, int count, AccountType accountType)
    {
        if (account != null && account.type == AccountType.AI)
        {
            UnityUtils.SetTexture2DForImage(this.HeadImg, aiTexture2D);
            this.accountType = AccountType.AI;
        }
        this.accountType = accountType;
        MessageCountPrompt.gameObject.SetActive(true);
        string t = time.ToString();
        // ͬһ����Ϣ, ������
        if (t.Equals(TimeText.text))
        {
            return;
        }
        this.TimeText.text = t;

        this.MessageUsername.text = username;
        this.MessageContent.text = content;

        this._TTid = ttid;
        this._MessageCount = count;
        this.MessageCountText.text = Convert.ToString(MessageCount);
    }

    //���������촰��, �����ѽ���˽��
    public void ClickSendMessageToFriends()
    {
        MyUserInfo.GetInstance.currentChatTTid = this._TTid;
        MyUserInfo.GetInstance.cururentChatAcountType = this.accountType;
        SceneManager.LoadScene("FriendChatScene");
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
