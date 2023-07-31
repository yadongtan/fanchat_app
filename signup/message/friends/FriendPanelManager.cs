using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FriendPanelManager : MonoBehaviour
{
    public GameObject friendInfoPrefab;
    public Texture2D GPT_3_5_Turbo_Texture;
    public float SingleFriendInfoHeight;

    private ScrollRect scrollRect;
    private Scrollbar scrollbar;

    private RectTransform content;

    [SerializeField]
    private float stepVertical; 
    [SerializeField]
    private float stepHorizontal; 
    [SerializeField]
    private float maxTextWidth;

    private GameObject lastOne = null;

    private float halfHeadLength;

    public LinkedList<GameObject> FriendsListUI = new LinkedList<GameObject>();
    public void Start()
    {
        this.Init();

        // 进入到此页面时, 从服务器上拉取最新的好友列表
        GetFriendsListMessage msg = new GetFriendsListMessage(MyUserInfo.GetInstance.ttid);
        // 异步调用
        FantasticNetworkClient.SyncClient.WriteSyncLambda(msg, (ackMsg) =>
        {
            if (ackMsg.ack == AckMessage.Ok)
            {
                Debug.Log("获取好友列表成功! msg:" + ackMsg.msg);
                GetFriendsListMessage.userFriendDetails[] friendList = (GetFriendsListMessage.userFriendDetails[])JsonSerializer.JsonToObject((string)ackMsg.data, new GetFriendsListMessage.userFriendDetails[1]);
                lock (MyUserInfo.GetInstance.friendsInfoTable)
                {
                    MyUserInfo.FriendsInfo friend;
                    for (int i = 0; i < friendList.Length; i++)
                    {
                        GetFriendsListMessage.userFriendDetails info = friendList[i];

                        if (MyUserInfo.GetInstance.friendsInfoTable[info.friend_ttid] == null)
                        {
                            friend = new MyUserInfo.FriendsInfo();
                            friend.ttid = info.friend_ttid;
                            friend.name = info.friend_username;
                            friend.status = info.status;
                            friend.type = AccountType.User;
                            MyUserInfo.GetInstance.friendsInfoTable[info.friend_ttid] = friend;
                        }
                    }
                    foreach (MyUserInfo.FriendsInfo info in MyUserInfo.GetInstance.friendsInfoTable.Values)
                    {
                        Loom.QueueOnMainThread((param) =>
                        {
                            ShowFriendsList();
                        }, null);
                    }
                }
            }
            else
            {
                Debug.Log("获取好友列表失败! msg:" + ackMsg.msg);
            }
            return null;
        });
        GetAIFriendsListMessage getAIFriendsListMessage = new GetAIFriendsListMessage(MyUserInfo.GetInstance.ttid);
        FantasticNetworkClient.SyncClient.WriteSyncLambda(getAIFriendsListMessage, (ackMsg) =>
        {
            if (ackMsg.ack == AckMessage.Ok)
            {
                Debug.Log("获取AI列表成功! msg:" + ackMsg.msg);
                AIFriendAccount[] friendList = (AIFriendAccount[])JsonSerializer.JsonToObject((string)ackMsg.data, new AIFriendAccount[1]);
                lock (MyUserInfo.GetInstance.friendsInfoTable)
                { 
                    MyUserInfo.FriendsInfo friend;
                    for (int i = 0; i < friendList.Length; i++)
                    {
                        AIFriendAccount aifa = friendList[i];

                        if (MyUserInfo.GetInstance.friendsInfoTable[aifa.ai_ttid] == null)
                        {
                            friend = new MyUserInfo.FriendsInfo();
                            friend.ttid = aifa.ai_ttid;
                            friend.name = aifa.name;
                            friend.status = "在线";
                            friend.type = AccountType.AI;
                            MyUserInfo.GetInstance.friendsInfoTable[aifa.ai_ttid] = friend;
                        }
                    }
                    foreach(MyUserInfo.FriendsInfo info in MyUserInfo.GetInstance.friendsInfoTable.Values)
                    {
                        Loom.QueueOnMainThread((param) =>
                        {
                            ShowFriendsList();
                        }, null);
                    }
                    }
                }
            else
            {
                Debug.Log("获取AI列表失败! msg:" + ackMsg.msg);
            }
            return null;
        });
    }

    public void Update()
    {

    }

    private void ShowMsgBubble()
    {

    }

    public void Init()
    {
        scrollRect = GetComponentInChildren<ScrollRect>();
        scrollbar = GetComponentInChildren<Scrollbar>();
        content = transform.Find("ViewPort").Find("Content").GetComponent<RectTransform>();
        halfHeadLength = friendInfoPrefab.GetComponent<FriendInfoScript>().HeadImg.GetComponent<RectTransform>().rect.height / 2;
    }

    public void AddFriend(string username, string status, int ttid, AccountType accountType)
    {
        GameObject friendInfo = Instantiate(friendInfoPrefab, this.content);
        friendInfo.GetComponent<FriendInfoScript>().setInfo(null, username, status, ttid, accountType);
        // 这是第一个
        if (lastOne == null)
        {
            lastOne = friendInfo;
        }
        else
        {
            Vector3 lastPos = lastOne.transform.localPosition;
            lastPos.y -= SingleFriendInfoHeight;
            Debug.Log("Height : " + lastPos.y);
            friendInfo.transform.localPosition = lastPos;
            lastOne = friendInfo;
        }
        FriendsListUI.AddLast(lastOne);
        scrollRect.verticalNormalizedPosition = 0;//使滑动条滚轮在最下方
    }
    public void ShowFriendsList()
    {
        foreach(GameObject fui in FriendsListUI)
        {
            fui.SetActive(false);
            DestroyImmediate(fui);
        }
        FriendsListUI.Clear();
        foreach (MyUserInfo.FriendsInfo info in MyUserInfo.GetInstance.friendsInfoTable.Values)
        {
            AddFriend(info.name, info.status, info.ttid, info.type);
        }
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