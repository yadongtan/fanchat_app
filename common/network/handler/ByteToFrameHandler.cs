using System.Collections;
using System.Threading;
using UnityEngine;

public class ByteToFrameHandler : Handler
{
    Hashtable waitMonitorForAckMessageFrameMap = new Hashtable();
    Hashtable ackMessageFrameMap = new Hashtable();

    public object Read(ChannelContext ctx, object data)
    {
        //字节 ---> 加密的帧 ---> 解密的帧
        Frame f = Frame.ResolveFrame((byte[])data);
        Debug.Log("接收到Frame: " + f.ToString());

        // 是Ack消息, 唤醒write线程
        if (f.FrameType == AckMessage.AckMessageType)
        {
            Frame sentFrame;
            lock (waitMonitorForAckMessageFrameMap)
            {
                sentFrame = (Frame)waitMonitorForAckMessageFrameMap[f.FrameId];
                waitMonitorForAckMessageFrameMap.Remove(f.FrameId);
            }

            lock (ackMessageFrameMap)
            {
                ackMessageFrameMap[f.FrameId] = f;
            }
            //TODO 这里可能有点问题, 为null 的情况
            if(sentFrame == null)
            {
                return f;
            }
            lock (sentFrame)
            {
                Monitor.Pulse(sentFrame); //唤醒
            }
        }

        return f;
    }

    public object Write(ChannelContext ctx, object data)
    {
        // // 加密的帧 ---> 字节
        Frame f = (Frame)data;

        // 如果是Ack消息, 直接发送 
        if(f.FrameType == AckMessage.AckMessageType)
        {
            Frame.GenerateFrameBytesWithFrame(f);
            //发送完消息进入阻塞等待
            ctx.chain.TriggerNextWriteHandler(ctx, this, Frame.GenerateFrameBytesWithFrame(f));
            return null;
        }

       //发送完消息进入阻塞等待
        ctx.chain.TriggerNextWriteHandler(ctx, this, Frame.GenerateFrameBytesWithFrame(f));

        lock (waitMonitorForAckMessageFrameMap)
        {
            waitMonitorForAckMessageFrameMap.Add(f.FrameId, f);
        }


        // 等待唤醒
        lock (f)
        {
            Monitor.Wait(f);        
        }

        Frame ackF;
        // 取到ack结果
        lock (ackMessageFrameMap)
        {
            ackF = (Frame)ackMessageFrameMap[f.FrameId];
            ackMessageFrameMap.Remove(ackF.FrameId); 
        }

        return ackF;
    }
}
