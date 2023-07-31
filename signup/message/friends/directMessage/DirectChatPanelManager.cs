using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DirectChatPanelManager : MonoBehaviour
{
    public Text UsernameText;
    public Text StatusText;
    public GameObject leftBubblePrefab;
    public GameObject rightBubblePrefab;

    private ScrollRect scrollRect;
    private Scrollbar scrollbar;

    private RectTransform content;

    [SerializeField]
    private float stepVertical; //上下两个气泡的垂直间隔
    [SerializeField]
    private float stepHorizontal; //左右两个气泡的水平间隔
    [SerializeField]
    private float maxTextWidth;//文本内容的最大宽度

    private float lastPos; //上一个气泡最下方的位置
    private float halfHeadLength;//头像高度的一半

    public void Start()
    {
        this.Init();
        int friendTTid = MyUserInfo.GetInstance.currentChatTTid;
        Debug.Log("当前正在聊天用户[" + friendTTid + "]");
        // 获取对方信息
        MyUserInfo.FriendsInfo info  = (MyUserInfo.FriendsInfo)MyUserInfo.GetInstance.friendsInfoTable[friendTTid];
        if(info == null)
        {
            UsernameText.text = "陌生用户(该用户不是您的好友)";
            StatusText.text = "未知";
        }
        else
        {
            UsernameText.text = info.name;
            StatusText.text = info.status;
        }
        // 获取历史消息列表
        LinkedList<FriendDirectMessage> list = (LinkedList<FriendDirectMessage>) MyUserInfo.GetInstance.historyFriendMessage[friendTTid];
        if(list != null)
        {
            foreach (FriendDirectMessage msg in list)
            {
                // 是我发送的消息
                if (msg.from_ttid == MyUserInfo.GetInstance.ttid)
                {
                    AddBubble(MyUserInfo.GetInstance.username, msg.text, true);
                }
                // 是别人发送给我的消息
                else
                {
                    MyUserInfo.FriendsInfo fri = (MyUserInfo.FriendsInfo)MyUserInfo.GetInstance.friendsInfoTable[msg.from_ttid];
                    AddBubble(fri.name, msg.text, false);
                }
            }
        }

    }

    public void GoToMainScene()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void Update()
    {
        // 拉取与这个好友相关的消息
        int friendTTid = MyUserInfo.GetInstance.currentChatTTid;
        Hashtable tables = MyUserInfo.GetInstance.newFriendMessages;
        //Debug.Log("Update, newFriendMessages Count: " + tables.Count);
        LinkedList<FriendDirectMessage> msgs = (LinkedList<FriendDirectMessage>)tables[friendTTid];
        if(msgs != null && msgs.Count != 0)
        {
            // 加锁
            lock (msgs)
            {
                foreach (FriendDirectMessage msg in msgs)
                {
                    // 是我发送的消息
                    if (msg.from_ttid == MyUserInfo.GetInstance.ttid)
                    {
                        AddBubble(MyUserInfo.GetInstance.username, msg.text, true);
                    }
                    // 是别人发送给我的消息
                    else
                    {
                        MyUserInfo.FriendsInfo fri = (MyUserInfo.FriendsInfo)MyUserInfo.GetInstance.friendsInfoTable[msg.from_ttid];
                        AddBubble(fri.name, msg.text, false);
                    }
                    // 如果历史列表为null
                    if(MyUserInfo.GetInstance.historyFriendMessage[friendTTid] == null)
                    {
                        lock (MyUserInfo.GetInstance.historyFriendMessage)
                        {
                            if(MyUserInfo.GetInstance.historyFriendMessage[friendTTid] == null)
                            {
                                MyUserInfo.GetInstance.historyFriendMessage[friendTTid] = new LinkedList<FriendDirectMessage>();
                            }
                        }
                    }
                    // 添加至历史列表
                    ((LinkedList<FriendDirectMessage>)MyUserInfo.GetInstance.historyFriendMessage[friendTTid])
                        .AddLast(msg);
                }
                //清空
                msgs.Clear();
            }
        }

    }

    private void ShowMsgBubble()
    {

    }

    public void Init()
    {
        scrollRect = GetComponentInChildren<ScrollRect>();
        scrollbar = GetComponentInChildren<Scrollbar>();
        content = transform.Find("ViewPort").Find("Content").GetComponent<RectTransform>();
        lastPos = 0;
        halfHeadLength = leftBubblePrefab.transform.Find("head").GetComponent<RectTransform>().rect.height / 2;
    }

    public void AddBubble(string username, string content, bool isMy)
    {
        GameObject newBubble = isMy ? Instantiate(rightBubblePrefab, this.content) : Instantiate(leftBubblePrefab, this.content);
        //设置气泡内容
        newBubble.GetComponent<ChatBubbleScript>().SetNameAndMsg(username, content);

        Text text = newBubble.GetComponentInChildren<Text>();
        if (text.preferredWidth > maxTextWidth)
        {
            LayoutElement el = text.GetComponent<LayoutElement>();
            el.preferredWidth = maxTextWidth;
        }
        //计算气泡的水平位置
        float hPos = isMy ? stepHorizontal / 2 : -stepHorizontal / 2;
        Debug.Log("水平位置: " + hPos);
        //计算气泡的垂直位置
        float vPos = -stepVertical - halfHeadLength + lastPos;
        newBubble.transform.localPosition = new Vector2(hPos, vPos);
        //更新lastPos
        Image bubbleImage = newBubble.GetComponentInChildren<Image>();
        float imageLength = GetContentSizeFitterPreferredSize(bubbleImage.GetComponent<RectTransform>(), bubbleImage.GetComponent<ContentSizeFitter>()).y;
        lastPos = vPos - imageLength;
        //更新content的长度
        if (-lastPos > this.content.rect.height)
        {
            this.content.sizeDelta = new Vector2(this.content.rect.width, -lastPos);
        }

        scrollRect.verticalNormalizedPosition = 0;//使滑动条滚轮在最下方
    }

    public Vector2 GetContentSizeFitterPreferredSize(RectTransform rect, ContentSizeFitter contentSizeFitter)
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(rect);
        return new Vector2(HandleSelfFittingAlongAxis(0, rect, contentSizeFitter),
            HandleSelfFittingAlongAxis(1, rect, contentSizeFitter));
    }

    private float HandleSelfFittingAlongAxis(int axis, RectTransform rect, ContentSizeFitter contentSizeFitter)
    {
        ContentSizeFitter.FitMode fitting =
            (axis == 0 ? contentSizeFitter.horizontalFit : contentSizeFitter.verticalFit);
        if (fitting == ContentSizeFitter.FitMode.MinSize)
        {
            return LayoutUtility.GetMinSize(rect, axis);
        }
        else
        {
            return LayoutUtility.GetPreferredSize(rect, axis);
        }
    }
}