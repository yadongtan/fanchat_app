using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;




public class FrameToMessageHandler : Handler
{
    public object Read(ChannelContext ctx, object data)
    {
        Frame f = (Frame)data;

        if (f.FrameType != AckMessage.AckMessageType)
        {

            //这里读取到消息
            Message msg = (Message)f.Payload;
            Message ack = msg.Invoke();
            Debug.Log("接收到内容:  " + JsonSerializer.ObjectToJson(f.Payload));
            this.Write(ctx, ack, f.FrameId);
        }
        else
        {
            Debug.Log("收到Ack: " + JsonSerializer.ObjectToJson(f.Payload));
        }
        return null;
	}

    public object Write(ChannelContext ctx, object data)
    {
        Frame f = Frame.GenerateMessageFrame(ctx.GenerateFrameId(), (Message)data);
        // 如果是ack消息, 直接写出
        if(f.FrameType == AckMessage.AckMessageType)
        {
            ctx.chain.TriggerNextWriteHandler(ctx, this, f);
            return null;
        }

        //不是ack消息, 需要等待Ack回复
        Frame ackF = (Frame)ctx.chain.TriggerNextWriteHandler(ctx, this, f);

        if(ackF.FrameType != AckMessage.AckMessageType)
        {
            throw new System.Exception("客户端发送了消息[" + f + "]但收到了非ack类型的消息回复!\n");
        }
        //返回应答消息
        Debug.Log("发送Frame: " + f.ToString());
        return (AckMessage) ackF.Payload;
    }


    public object Write(ChannelContext ctx, object data, string frameId)
    {
        Frame f = Frame.GenerateMessageFrame(frameId, (Message)data);
        // 如果是ack消息, 直接写出
        if (f.FrameType == AckMessage.AckMessageType)
        {
            ctx.chain.TriggerNextWriteHandler(ctx, this, f);
            return null;
        }

        //不是ack消息, 需要等待Ack回复
        Frame ackF = (Frame)ctx.chain.TriggerNextWriteHandler(ctx, this, f);

        if (ackF.FrameType != AckMessage.AckMessageType)
        {
            throw new System.Exception("客户端发送了消息[" + f + "]但收到了非ack类型的消息回复!\n");
        }
        //返回应答消息
        Debug.Log("发送Frame: " + f.ToString());
        return (AckMessage)ackF.Payload;
    }
}
