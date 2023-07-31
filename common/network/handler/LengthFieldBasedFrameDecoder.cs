using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;



// 解析消息体, 处理拆包粘包问题

public class LengthFieldBasedFrameDecoder : Handler
{
    public object Read(ChannelContext ctx, object data)
    {
		int currentReadCount = 0;           //已经读了的长度
		int frameLen = 4;                   //要读的长度
		byte[] frameLenByte = new byte[8]; // 存储长度字节的数组
										   // 读取长度
		byte[] readLenByte = new byte[4];
		while (currentReadCount < frameLen) {
			int count = ctx.client.Receive(readLenByte);
			int copyFrom = currentReadCount;
			for (int i = 0; i < count; i++)
			{
				frameLenByte[copyFrom++] = readLenByte[i];
			}
			currentReadCount += count;
		}
		// 更新长度
		frameLen = NetworkUtils.CastBytesToInt(frameLenByte, 0);
	    byte[] rawData = new byte[frameLen]; //存储数据的数组

		NetworkUtils.Copy(rawData, 0, frameLenByte, 0, currentReadCount);

		//循环读取
		byte[] remainData = new byte[frameLen - currentReadCount];

		while(currentReadCount < frameLen){
			int count = ctx.client.Receive(remainData);
			// 拷贝到存储数据的数组
			NetworkUtils.Copy(rawData, currentReadCount, remainData, 0, count);
			currentReadCount += count; //又读了多少个
		}
		// fmt.Printf("解析到数据帧:%v\n", string(rawData))
		return rawData;
	}

    public object Write(ChannelContext ctx, object data)
    {
		ctx.client.Send((byte[])data);
		return null;
    }
}
