
using UnityEngine;




public class SerializerUtils
{
	// JsonSerializeType json序列化方式
	public const int JsonSerializeType = 1;

	public static T Unserialize<T>(int serializeType, byte[] payload, T v) {
		return Unserialize<T>(serializeType, System.Text.Encoding.UTF8.GetString(payload), v);
	}

	//反序列化
	public static object UnserializeByType(int serializeType, byte[] payload, int messageType)
	{
		return Unserialize(serializeType, System.Text.Encoding.UTF8.GetString(payload), messageType);
	}

	public static T Unserialize<T>(int serializeType, string payload, T v)
	{
		switch (serializeType)
		{
			case JsonSerializeType:
				return (T)JsonSerializer.JsonToObject(payload, v);
		}
		throw new System.Exception("错误! 未知的反序列化类型!");
	}

	//反序列化
	public static object UnserializeByType(int serializeType, string payload, int messageType)
	{
		switch (serializeType)
		{
			case JsonSerializeType:
				object msgObj = Message.getMessageObjByType(messageType);
				return JsonSerializer.JsonToObject(payload, msgObj);
		}

		throw new System.Exception("错误! 未知的反序列化类型");
	}

	public static string Serialize(int serializeType, object data)

	{
		switch (serializeType)
		{
			case JsonSerializeType:
				return JsonSerializer.ObjectToJson(data);
		}
		throw new System.Exception("错误! 未知的序列化类型");
	}
}