using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;



public class NetworkUtils
{
	public static void CastIntToBytes(byte[] bytes, int startIndex, int data)
	{
		bytes[startIndex] = (byte)(data >> 24);
		bytes[startIndex + 1] = (byte)(data >> 16);
		bytes[startIndex + 2] = (byte)(data >> 8);
		bytes[startIndex + 3] = (byte)data;
	}

	public static int CastBytesToInt(byte[] bytes, int startIndex)
	{
		int data = 0;
		data += ((int)(bytes[startIndex])) << 24;
		data += ((int)(bytes[startIndex + 1])) << 16;
		data += ((int)(bytes[startIndex + 2])) << 8;
		data += (int)(bytes[startIndex + 3]);
		return data;
	}

	public static void Copy(byte[] dist, int distStart,
		byte[] src, int srcStart, int srcEnd)
    {
		for(;srcStart < srcEnd; srcStart++, distStart++)
        {
			dist[distStart] = src[srcStart];
        }
    }
}
