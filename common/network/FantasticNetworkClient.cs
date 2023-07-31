using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;



// NetworkClient的单例, 负责发送和接收消息

public class FantasticNetworkClient : MonoBehaviour
{
    private static FantasticNetworkClient instance;
    Thread readThread;
    // private static string ip = "120.26.76.100";
    private static string ip = "127.0.0.1";

    //private static string ip = "192.168.43.23";
    private static int port = 8081;

    private SocketChannel channel;

    public bool Connected()
    {
        if (channel == null || channel.Connected() == false)
        {
            return false;
        }
        return true;
    }

    public void Close()
    {
        channel.Close();
    }

    public interface AckMessagehHandler
    {
        void AfterReceivedAck(AckMessage ack);
    }

    public class SyncClient
    {


        AckMessagehHandler ackMessagehHandler;
        Func<AckMessage, object> ackFunc;

        public static void WriteSync(Message msg, AckMessagehHandler handler)
        {
            if (FantasticNetworkClient.GetInstance.Connected() == false)
            {
                GetInstance.channel = GetInstance.OpenNewSocketChannelAndKeep();
                throw new WriteFailedException("连接到服务器失败, 请稍后重试!");
            }
            Debug.Log("发送消息: " + JsonSerializer.ObjectToJson(msg));
            SyncClient client = new SyncClient(handler, null);
            client.WriteSync(msg);
        }

        public static void WriteSyncLambda(Message msg, Func<AckMessage, object> ackFunc)
        {
            if (FantasticNetworkClient.GetInstance.Connected() == false)
            {
                GetInstance.channel = GetInstance.OpenNewSocketChannelAndKeep();
                throw new WriteFailedException("连接到服务器失败, 请稍后重试!");
            }
            Debug.Log("发送消息: " + JsonSerializer.ObjectToJson(msg));



            SyncClient client = new SyncClient(null, ackFunc);
            client.WriteSyncLambda(msg);
        }


        public SyncClient(AckMessagehHandler handler, Func<AckMessage, object> ackFunc)
        {

            this.ackMessagehHandler = handler;
            this.ackFunc = ackFunc;
        }


        // 异步写, 调用回调函数
        public void WriteSync(Message msg)
        {
            if (FantasticNetworkClient.GetInstance.channel.Connected() == false)
            {
                GetInstance.channel = GetInstance.OpenNewSocketChannelAndKeep();
                throw new WriteFailedException("连接到服务器失败, 请稍后重试!");
            }
            Func<Message, AckMessage> func = FantasticNetworkClient.GetInstance.Write;
            func.BeginInvoke(msg, WriteSyncCallBack, func);
        }

        // 异步写, 调用回调函数
        public void WriteSyncLambda(Message msg)
        {
            if (FantasticNetworkClient.GetInstance.Connected() == false)
            {
                GetInstance.channel = GetInstance.OpenNewSocketChannelAndKeep();
                throw new WriteFailedException("连接到服务器失败, 请稍后重试!");
            }
            Func<Message, AckMessage> func = FantasticNetworkClient.GetInstance.Write;
            func.BeginInvoke(msg, WriteSyncCallBackLambda, func);
        }

        private void WriteSyncCallBack(IAsyncResult fun)
        {
            if (FantasticNetworkClient.GetInstance.Connected() == false)
            {
                GetInstance.channel = GetInstance.OpenNewSocketChannelAndKeep();
                throw new WriteFailedException("连接到服务器失败, 请稍后重试!");
            }
            if (fun == null)
            {
                throw new ArgumentNullException("fun");
            }
            Func<Message, AckMessage> dl = (Func<Message, AckMessage>)fun.AsyncState;
            AckMessage ackMessage = dl.EndInvoke(fun);
            ackMessagehHandler.AfterReceivedAck(ackMessage);
        }

        private void WriteSyncCallBackLambda(IAsyncResult fun)
        {
            if (FantasticNetworkClient.GetInstance.Connected() == false)
            {
                GetInstance.channel = GetInstance.OpenNewSocketChannelAndKeep();
                throw new WriteFailedException("连接到服务器失败, 请稍后重试!");
            }
            if (fun == null)
            {
                throw new ArgumentNullException("fun");
            }
            Func<Message, AckMessage> dl = (Func<Message, AckMessage>)fun.AsyncState;
            AckMessage ackMessage = dl.EndInvoke(fun);
            ackFunc(ackMessage);
        }

    }

    public static FantasticNetworkClient GetInstance
    {
        get
        {
            if (instance == null)
            {
                
                // 新建单例挂在新建Obj上 
                GameObject obj = new GameObject("FantasticNetworkClientObject");
                instance = obj.AddComponent<FantasticNetworkClient>();
                instance.OpenNewSocketChannelAndKeep();
                DontDestroyOnLoad(obj);
            }
            return instance;
        }
    }

    public void OpenNewNetwork()
    {
        OpenNewSocketChannelAndKeep();
    }

    // 创建新的channel并保持连接, 外部通过调用这个方法创建连接!!!!!!!!!!
    public SocketChannel OpenNewSocketChannelAndKeep()
    {
        lock (this)
        {
            if (this.channel != null)
            {
                this.channel.Close();
                // 读线程仍然存活, 关掉它
                if (readThread.IsAlive)
                {
                    readThread.Abort();
                }
            }
            SocketChannel channel;
            try
            {
                channel = SocketChannel.Open(ip, port)
                .AddHandler(new LengthFieldBasedFrameDecoder())
                .AddHandler(new ByteToFrameHandler())
                .AddHandler(new FrameToMessageHandler());//只是启动了客户端但不处理读写现在
                                                         // 将挂载单例Obj设为不销毁的对象
            }
            catch (ConnectionFailedException e)
            {
                throw e;
            }

            this.channel = channel;
            RunAndReadSync();
            return channel;
        }
       
    }


    public void Init()
    {
        Debug.Log("FantasticNetworkClient.Init");
    }

    //会阻塞当前线程, 需要额外启动线程进行读
    public void RunAndRead()
    {
        try
        {
            Debug.Log("启动Client Reader 成功!\n");
            this.channel.RunAndKeep();
        }catch(Exception e)
        {
            Debug.Log("与服务器连接出现问题! Error: " + e.Message);
            Thread thread = new Thread(this.OpenNewNetwork);
            thread.Start();
        }

    }


    public void RunAndReadSync()
    {
        readThread = new Thread(RunAndRead);
        readThread.Start();
    }



    // 写, 阻塞等待响应结果
    public AckMessage Write(Message msg)
    {
        if (FantasticNetworkClient.GetInstance.Connected() == false)
        {
            GetInstance.channel = GetInstance.OpenNewSocketChannelAndKeep();
            throw new WriteFailedException("连接到服务器失败, 请稍后重试!");
        }
        return (AckMessage)this.channel.Write(msg);
    }


  

 
}
