using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FriendInfoScript : MonoBehaviour
{
    public Image HeadImg;
    public Texture2D GPT_3_5_Turbo_Texture;
    public AccountType AccountType = AccountType.User;
    public Text Name;
    public Text Status;
    public Image BubbleImgae;
    public int ttid;
    public AccountType accountType = AccountType.User;

    public static string offline = "[离线]";
    public static string online = "[在线]";


    //点击后打开聊天窗口, 与朋友进行私聊
    public void ClickSendMessageToFriends()
    {
        MyUserInfo.GetInstance.currentChatTTid = ttid;
        MyUserInfo.GetInstance.cururentChatAcountType = accountType;
        SceneManager.LoadScene("FriendChatScene");
    }

    public void setInfo(Image img, string name, string status, int ttid, AccountType accountType)
    {

        if(img != null)
        {
            HeadImg = img;
        }else
        {
            if (img == null && accountType == AccountType.AI)
            {
                this.accountType = accountType;
                UnityUtils.SetTexture2DForImage(HeadImg, GPT_3_5_Turbo_Texture);
            }
        }
        Name.text = name;
        Status.text = status;
        this.ttid = ttid;
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
