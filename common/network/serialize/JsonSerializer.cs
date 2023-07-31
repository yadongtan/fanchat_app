using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using UnityEngine;

public class JsonSerializer
{
    /// <summary>
    /// 内存对象转换为json字符串
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static string ObjectToJson(object obj)
    {
        if(obj == null)
        {
            return "";
        }
        DataContractJsonSerializer serializer = new DataContractJsonSerializer(obj.GetType());
        MemoryStream stream = new MemoryStream();
        serializer.WriteObject(stream, obj);
        byte[] dataBytes = new byte[stream.Length];
        stream.Position = 0;
        stream.Read(dataBytes, 0, (int)stream.Length);
        string json = Encoding.UTF8.GetString(dataBytes);
        return json;
    }
    /// <summary>
    /// Json字符串转内存对象
    /// </summary>
    /// <param name="jsonString"></param>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static object JsonToObject(string jsonString, object obj)
    {
        DataContractJsonSerializer serializer = new DataContractJsonSerializer(obj.GetType());
        MemoryStream mStream = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));
        return serializer.ReadObject(mStream);
    }

    //public static string ObjectToJson(object obj)
    //{

    //    return "test";
    //}
    ///// <summary>
    ///// Json字符串转内存对象
    ///// </summary>
    ///// <param name="jsonString"></param>
    ///// <param name="obj"></param>
    ///// <returns></returns>
    //public static object JsonToObject(string jsonString, object obj)
    //{
    //    return null;
    //}
}