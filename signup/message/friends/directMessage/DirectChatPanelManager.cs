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
    private float stepVertical; //�����������ݵĴ�ֱ���
    [SerializeField]
    private float stepHorizontal; //�����������ݵ�ˮƽ���
    [SerializeField]
    private float maxTextWidth;//�ı����ݵ������

    private float lastPos; //��һ���������·���λ��
    private float halfHeadLength;//ͷ��߶ȵ�һ��

    public void Start()
    {
        this.Init();
        int friendTTid = MyUserInfo.GetInstance.currentChatTTid;
        Debug.Log("��ǰ���������û�[" + friendTTid + "]");
        // ��ȡ�Է���Ϣ
        MyUserInfo.FriendsInfo info  = (MyUserInfo.FriendsInfo)MyUserInfo.GetInstance.friendsInfoTable[friendTTid];
        if(info == null)
        {
            UsernameText.text = "İ���û�(���û��������ĺ���)";
            StatusText.text = "δ֪";
        }
        else
        {
            UsernameText.text = info.name;
            StatusText.text = info.status;
        }
        // ��ȡ��ʷ��Ϣ�б�
        LinkedList<FriendDirectMessage> list = (LinkedList<FriendDirectMessage>) MyUserInfo.GetInstance.historyFriendMessage[friendTTid];
        if(list != null)
        {
            foreach (FriendDirectMessage msg in list)
            {
                // ���ҷ��͵���Ϣ
                if (msg.from_ttid == MyUserInfo.GetInstance.ttid)
                {
                    AddBubble(MyUserInfo.GetInstance.username, msg.text, true);
                }
                // �Ǳ��˷��͸��ҵ���Ϣ
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
        // ��ȡ�����������ص���Ϣ
        int friendTTid = MyUserInfo.GetInstance.currentChatTTid;
        Hashtable tables = MyUserInfo.GetInstance.newFriendMessages;
        //Debug.Log("Update, newFriendMessages Count: " + tables.Count);
        LinkedList<FriendDirectMessage> msgs = (LinkedList<FriendDirectMessage>)tables[friendTTid];
        if(msgs != null && msgs.Count != 0)
        {
            // ����
            lock (msgs)
            {
                foreach (FriendDirectMessage msg in msgs)
                {
                    // ���ҷ��͵���Ϣ
                    if (msg.from_ttid == MyUserInfo.GetInstance.ttid)
                    {
                        AddBubble(MyUserInfo.GetInstance.username, msg.text, true);
                    }
                    // �Ǳ��˷��͸��ҵ���Ϣ
                    else
                    {
                        MyUserInfo.FriendsInfo fri = (MyUserInfo.FriendsInfo)MyUserInfo.GetInstance.friendsInfoTable[msg.from_ttid];
                        AddBubble(fri.name, msg.text, false);
                    }
                    // �����ʷ�б�Ϊnull
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
                    // �������ʷ�б�
                    ((LinkedList<FriendDirectMessage>)MyUserInfo.GetInstance.historyFriendMessage[friendTTid])
                        .AddLast(msg);
                }
                //���
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
        //������������
        newBubble.GetComponent<ChatBubbleScript>().SetNameAndMsg(username, content);

        Text text = newBubble.GetComponentInChildren<Text>();
        if (text.preferredWidth > maxTextWidth)
        {
            LayoutElement el = text.GetComponent<LayoutElement>();
            el.preferredWidth = maxTextWidth;
        }
        //�������ݵ�ˮƽλ��
        float hPos = isMy ? stepHorizontal / 2 : -stepHorizontal / 2;
        Debug.Log("ˮƽλ��: " + hPos);
        //�������ݵĴ�ֱλ��
        float vPos = -stepVertical - halfHeadLength + lastPos;
        newBubble.transform.localPosition = new Vector2(hPos, vPos);
        //����lastPos
        Image bubbleImage = newBubble.GetComponentInChildren<Image>();
        float imageLength = GetContentSizeFitterPreferredSize(bubbleImage.GetComponent<RectTransform>(), bubbleImage.GetComponent<ContentSizeFitter>()).y;
        lastPos = vPos - imageLength;
        //����content�ĳ���
        if (-lastPos > this.content.rect.height)
        {
            this.content.sizeDelta = new Vector2(this.content.rect.width, -lastPos);
        }

        scrollRect.verticalNormalizedPosition = 0;//ʹ���������������·�
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