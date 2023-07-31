using System.Collections;
using System.Collections.Generic;

public class MyUserInfo 
{
    public int ttid;
    public string username;

    public int currentChatTTid; //��ǰ����ĶԷ���ttid
    public AccountType cururentChatAcountType = AccountType.User; //��ǰ����Է�������

    private static MyUserInfo instance;

    // ����ͼƬ
    //Texture2D texture2D = new Texture2D(image.Width, image.Height);
    //texture2D.LoadImage();

    //�յ�����������Ϣ
    public LinkedList<PublicChatTextMessage> receivedMessage = new LinkedList<PublicChatTextMessage>();

    // �洢�½��յ����ѵ���Ϣ, ttid - LinkedList<FriendDirectMessage>
    public Hashtable newFriendMessages = new Hashtable();

    // �洢���ѵ���ʷ��Ϣ, ttid - LinkedList<FriendDirectMessage>
    public Hashtable historyFriendMessage = new Hashtable();


    //�����б� ttid - FriendsInfo
    public Hashtable friendsInfoTable = new Hashtable();
    public LinkedList<FriendsInfo> friendsInfos = new LinkedList<FriendsInfo>();


    public class FriendsInfo
    {
        public AccountType type = AccountType.User;
        public int ttid;
        public string name;
        public string status;
    }

    public string getUsername(int ttid)
    {
        FriendsInfo info = (FriendsInfo)friendsInfoTable[ttid];
        if( info != null)
        {
            return info.name;
        }
        else
        {
            return "İ���û�(���û��������ĺ���)";
        }
    }


    public static MyUserInfo GetInstance
    {
        get
        {
            if (instance == null)
            {
                MyUserInfo info = new MyUserInfo();
                instance = info;
            }
            return instance;
        }
    }


}
