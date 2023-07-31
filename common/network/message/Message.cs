using System;


// handler
[Serializable()]
public abstract class Message
{


    public virtual Message Invoke() {

        throw new System.Exception("错误! Message.Invoke() 未实现\n"); 
    }


    public static Message getMessageObjByType(int messageType)
    {
        //登录和Ack
        switch (messageType)
        {
            case SignInMessage.SignInMessageType:
                return new SignInMessage();
            case AckMessage.AckMessageType:
                return new AckMessage();

        }

        // public chat 所有人聊天信息
        switch (messageType)
        {
            case GetOnlinePersonCountMessage.type:
                return new GetOnlinePersonCountMessage();
            case PublicChatImgMessage.type:
                return new PublicChatImgMessage();
            case PublicChatTextMessage.type:
                return new PublicChatTextMessage();
            case PublicChatVideoMessage.type:
                return new PublicChatVideoMessage();
        }
        // 注册消息
        switch (messageType)
        {
            case SignUpMessage.type:
                return new SignUpMessage();
        }

        // 联系人
        switch(messageType)
        {
            case AddFriendMessage.type:
                return new AddFriendMessage();
            case GetFriendsListMessage.type:
                return new GetFriendsListMessage();
            case FriendDirectMessage.type:
                return new FriendDirectMessage();
        }


        // OpenAI
        switch (messageType)
        {
            case CreateChatMessage.type:
                return new CreateChatMessage();
            case GetAIFriendsListMessage.type:
                return new GetAIFriendsListMessage();
        }
        throw new System.Exception("错误!  未知的消息类型!\n");

    }


    public virtual int getMessageType()
    {
        throw new System.Exception("错误! 获取消息类型时使用了未实现的函数\n");
    }

}
