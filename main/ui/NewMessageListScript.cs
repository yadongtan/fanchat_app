using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewMessageListScript : MonoBehaviour
{

    // ��Ϣ�б��Transform
    public Transform NewMessageList;
    // ����Ϣ
    public GameObject NewMessageInfoPrefab;
    // ��ǰ�Ѿ��е���Ϣ ttid - GameObject --> SingleMessageScript�ű�
    public Hashtable NewMessageTable = new Hashtable();
    // �Ƿ����ˢ�»�ȡ��Ϣ��, �����ȵȴ���ȡ�˺����б��AI�α�
    private static bool UserRefreshable = false;
    private static bool AIRefreshable = false;

    // ˢ�º����б�
    public void RefreshFriendList()
    {
        // ���뵽��ҳ��ʱ, �ӷ���������ȡ���µĺ����б�
        GetFriendsListMessage msg = new GetFriendsListMessage(MyUserInfo.GetInstance.ttid);
        // �첽����
        FantasticNetworkClient.SyncClient.WriteSyncLambda(msg, (ackMsg) =>
        {
            if (ackMsg.ack == AckMessage.Ok)
            {
                Debug.Log("��ȡ�����б�ɹ�! msg:" + ackMsg.msg);
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
                Debug.Log("��ȡ�����б�ʧ��! msg:" + ackMsg.msg);
            }
            return null;
        });
        GetAIFriendsListMessage getAIFriendsListMessage = new GetAIFriendsListMessage(MyUserInfo.GetInstance.ttid);
        FantasticNetworkClient.SyncClient.WriteSyncLambda(getAIFriendsListMessage, (ackMsg) =>
        {
            if (ackMsg.ack == AckMessage.Ok)
            {
                Debug.Log("��ȡAI�б�ɹ�! msg:" + ackMsg.msg);
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
                            friend.status = "����";
                            friend.type = AccountType.AI;
                            MyUserInfo.GetInstance.friendsInfoTable[aifa.ai_ttid] = friend;
                        }
                    }
                }
                AIRefreshable = true;
            }
            else
            {
                Debug.Log("��ȡAI�б�ʧ��! msg:" + ackMsg.msg);
            }
            return null;
        });
    }

    // Start is called before the first frame update
    void Start()
    {
        MyUserInfo instance = MyUserInfo.GetInstance;
        //������ʷ��¼�б�
        Hashtable oldMessages = instance.historyFriendMessage;
        foreach (int oldTTid in oldMessages.Keys)
        {

            LinkedList<FriendDirectMessage> list = (LinkedList<FriendDirectMessage>)oldMessages[oldTTid];
            if (list != null && list.Count != 0)
            {
                // ��ȡ���һ����Ϣ
                FriendDirectMessage msg = list.Last.Value;
                // �鿴��ǰ�б��Ƿ��Ѿ��и��û�����ʷ��Ϣ
                GameObject NewMessageInfo = (GameObject)NewMessageTable[oldTTid];
                // ֮ǰ�Ѿ�����, ��ʾ������Ϣ��ʱ���Լ���Ϣ��������
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
                    // ֮ǰû��, Ҫ��ʵ����һ�������б���
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
        RefreshFriendList();    //��ȡ�����б�
    }

    private void RefreshMessageList()
    {
        MyUserInfo instance = MyUserInfo.GetInstance;
        Hashtable newMessages = instance.newFriendMessages;
        lock (newMessages)
        {
            foreach (int ttid in newMessages.Keys)
            {

                // ����ÿһ��ttid, ��������Ϣ
                LinkedList<FriendDirectMessage> list = (LinkedList<FriendDirectMessage>)newMessages[ttid];
                if (list != null && list.Count != 0)
                {
                    // ��ȡ���һ����Ϣ
                    FriendDirectMessage msg = list.Last.Value;
                    // �鿴��ǰ�б��Ƿ��Ѿ��и��û�����ʷ��Ϣ
                    GameObject NewMessageInfo = (GameObject)NewMessageTable[ttid];
                    // ֮ǰ�Ѿ�����, ��ʾ������Ϣ��ʱ���Լ���Ϣ��������
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
                        // ֮ǰû��, Ҫ��ʵ����һ�������б���
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
    // �������Ϣ����Ϣ�б�
    // Update is called once per frame
    void Update()
    {
        // ���Կ�ʼˢ���˲�ˢ��
        if (AIRefreshable && UserRefreshable)
        {
            RefreshMessageList(); // ˢ����Ϣ�б�
        }
    }


}
