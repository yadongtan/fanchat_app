using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewMessageListScript : MonoBehaviour
{

    // 消息列表的Transform
    public Transform NewMessageList;
    // 新消息
    public GameObject NewMessageInfoPrefab;
    // 当前已经有的消息 ttid - GameObject --> SingleMessageScript脚本
    public Hashtable NewMessageTable = new Hashtable();
    // 是否可以刷新获取消息了, 必须先等待获取了好友列表和AI课表
    private static bool UserRefreshable = false;
    private static bool AIRefreshable = false;

    // 刷新好友列表
    public void RefreshFriendList()
    {
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
                }
                UserRefreshable = true;
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
                }
                AIRefreshable = true;
            }
            else
            {
                Debug.Log("获取AI列表失败! msg:" + ackMsg.msg);
            }
            return null;
        });
    }

    // Start is called before the first frame update
    void Start()
    {
        MyUserInfo instance = MyUserInfo.GetInstance;
        //遍历历史记录列表
        Hashtable oldMessages = instance.historyFriendMessage;
        foreach (int oldTTid in oldMessages.Keys)
        {

            LinkedList<FriendDirectMessage> list = (LinkedList<FriendDirectMessage>)oldMessages[oldTTid];
            if (list != null && list.Count != 0)
            {
                // 获取最后一条消息
                FriendDirectMessage msg = list.Last.Value;
                // 查看当前列表是否已经有该用户的历史消息
                GameObject NewMessageInfo = (GameObject)NewMessageTable[oldTTid];
                // 之前已经有了, 显示最新消息和时间以及消息数就行了
                if (NewMessageInfo != null)
                {
                    NewMessageInfo.GetComponent<SingleMessageScript>().SetOldMessage(
                        (MyUserInfo.FriendsInfo)MyUserInfo.GetInstance.friendsInfoTable[oldTTid],
                        instance.getUsername(oldTTid),
                        msg.text,
                        TimeUitls.GetDateTimeMilliseconds(msg.time),
                        oldTTid,
                        msg.account_type
                        );
                }
                else
                {
                    // 之前没有, 要新实例化一个对象到列表中
                    GameObject newMsgObject = Instantiate(NewMessageInfoPrefab, NewMessageList);
                    newMsgObject.GetComponent<SingleMessageScript>().SetOldMessage(
                        (MyUserInfo.FriendsInfo)MyUserInfo.GetInstance.friendsInfoTable[oldTTid],
                        instance.getUsername(oldTTid),
                        msg.text,
                        TimeUitls.GetDateTimeMilliseconds(msg.time),
                        oldTTid,
                        msg.account_type
                        );
                    NewMessageTable[oldTTid] = newMsgObject;
                }
            }
        }
        RefreshFriendList();    //获取好友列表
    }

    private void RefreshMessageList()
    {
        MyUserInfo instance = MyUserInfo.GetInstance;
        Hashtable newMessages = instance.newFriendMessages;
        lock (newMessages)
        {
            foreach (int ttid in newMessages.Keys)
            {

                // 遍历每一个ttid, 生成新消息
                LinkedList<FriendDirectMessage> list = (LinkedList<FriendDirectMessage>)newMessages[ttid];
                if (list != null && list.Count != 0)
                {
                    // 获取最后一条消息
                    FriendDirectMessage msg = list.Last.Value;
                    // 查看当前列表是否已经有该用户的历史消息
                    GameObject NewMessageInfo = (GameObject)NewMessageTable[ttid];
                    // 之前已经有了, 显示最新消息和时间以及消息数就行了
                    if (NewMessageInfo != null)
                    {
                        Debug.Log("ttid = " + ttid);
                        NewMessageInfo.GetComponent<SingleMessageScript>().SetNewMessage(
                            (MyUserInfo.FriendsInfo)MyUserInfo.GetInstance.friendsInfoTable[ttid],
                            instance.getUsername(ttid),
                            msg.text,
                            TimeUitls.GetDateTimeMilliseconds(msg.time),
                            ttid,
                            list.Count,
                            msg.account_type
                            );
                    }
                    else
                    {
                        // 之前没有, 要新实例化一个对象到列表中
                        GameObject newMsgObject = Instantiate(NewMessageInfoPrefab, NewMessageList);
                        Debug.Log("ttid = " + ttid);
                        newMsgObject.GetComponent<SingleMessageScript>().SetNewMessage(
                            (MyUserInfo.FriendsInfo)MyUserInfo.GetInstance.friendsInfoTable[ttid],
                            instance.getUsername(ttid),
                            msg.text,
                            TimeUitls.GetDateTimeMilliseconds(msg.time),
                            ttid,
                            list.Count,
                            msg.account_type
                            );
                        NewMessageTable[ttid] = newMsgObject;
                    }
                }
            }
        }
    }
    // 添加新消息到消息列表
    // Update is called once per frame
    void Update()
    {
        // 可以开始刷新了才刷新
        if (AIRefreshable && UserRefreshable)
        {
            RefreshMessageList(); // 刷新消息列表
        }
    }


}
