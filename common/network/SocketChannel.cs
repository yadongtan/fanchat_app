using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using UnityEngine;



// Socket的channel, 负责发送和接收消息处理

public class SocketChannel
{
    private Socket client; //连接服务器的客户端
    private ChannelContext context; //通道上下文
    
    public void Close()
    {
        client.Close();
    }

    public bool Connected()
    {
        return client.Connected;
    }

    private static byte[] GetKeepAliveData()
    {
        uint dummy = 0;
        byte[] inOptionValues = new byte[Marshal.SizeOf(dummy) * 3];
        BitConverter.GetBytes((uint)1).CopyTo(inOptionValues, 0);
        BitConverter.GetBytes((uint)3000).CopyTo(inOptionValues, Marshal.SizeOf(dummy));//keep-alive间隔
        BitConverter.GetBytes((uint)500).CopyTo(inOptionValues, Marshal.SizeOf(dummy) * 2);// 尝试间隔
        return inOptionValues;
    }

    public static SocketChannel Open(string ip, int port)
    {
        Socket client;
        try
        {
            client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint ie = new IPEndPoint(IPAddress.Parse(ip), port);
            client.Connect(ie);
            client.IOControl(IOControlCode.KeepAliveValues, GetKeepAliveData(), null);
            client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);
        }
        catch(System.Exception e)
        {
            throw new ConnectionFailedException("连接服务器失败, 请检查网络连接");
        }

        if (client == null || client.Connected == false)
        {
            throw new ConnectionFailedException("连接服务器失败, 请检查网络连接");
        }

        SocketChannel channel = new SocketChannel();
        channel.client = client;
        channel.context = new ChannelContext(client);
        return channel;
    }

    //向服务器发送消息
    public AckMessage Write(Message message)
    {
        AckMessage msg;
        try
        {
            msg = (AckMessage)context.Write(message);
        }catch(System.Exception e)
        {
            throw new WriteFailedException("网络连接失败, 请检查网络设置!");
        }
        return msg;
    }

    public SocketChannel AddHandler(Handler handler)
    {
        this.context.AddHandler(handler);
        return this;
    }

    public void RunAndKeep()
    {
        // 处理服务器的读事件
        for (; ; )
        {
            this.context.chain.Read(context);
        }
    }


}
