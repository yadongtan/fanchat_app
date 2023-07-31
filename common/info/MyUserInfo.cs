using System.Collections;
using System.Collections.Generic;

public class MyUserInfo 
{
    public int ttid;
    public string username;

    public int currentChatTTid; //当前聊天的对方的ttid
    public AccountType cururentChatAcountType = AccountType.User; //当前聊天对方的类型

    private static MyUserInfo instance;

    // 加载图片
    //Texture2D texture2D = new Texture2D(image.Width, image.Height);
    //texture2D.LoadImage();

    //收到的所有人消息
    public LinkedList<PublicChatTextMessage> receivedMessage = new LinkedList<PublicChatTextMessage>();

    // 存储新接收到朋友的消息, ttid - LinkedList<FriendDirectMessage>
    public Hashtable newFriendMessages = new Hashtable();

    // 存储朋友的历史消息, ttid - LinkedList<FriendDirectMessage>
    public Hashtable historyFriendMessage = new Hashtable();


    //朋友列表 ttid - FriendsInfo
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
            return "陌生用户(该用户不是您的好友)";
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
