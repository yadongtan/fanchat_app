using System.Threading;




public class Frame
{

	public static int ServerVersion = 1;
	public static int frameIdIncrement = 0;

	public int FrameLen;          //帧长
	public int Version;                //版本号
	public int FrameType;              //帧类型
	public int SerializeType;          //序列化类型
	public int EncryptType;            //消息体加密类型
	public string FrameId;                 //帧id
	public object Payload;      //消息体


	// 消息 ---> 加密的帧
	public static Frame GenerateMessageFrame(string frameId, Message msg)
    {
		return GenerateFrame(frameId, msg.getMessageType(), msg, EncryptUtils.AESEncryptType, SerializerUtils.JsonSerializeType);
    }



	// ---> 加密的帧
	public static Frame GenerateFrame(string frameId, int frameType, object payload, int encryptType, int serializeType)
	{
		//序列化
		string serializedPaylod = SerializerUtils.Serialize(serializeType, payload);
		// 加密
		string encryptPayload = EncryptUtils.Encrypt(serializedPaylod, encryptType);

		byte[] payloadBytes = System.Text.Encoding.UTF8.GetBytes(encryptPayload);
		int version = ServerVersion;

		byte[] frameIdBytes = System.Text.Encoding.UTF8.GetBytes(frameId);

		int frameLen = 4 * 5 + frameIdBytes.Length +  payloadBytes.Length;
		Frame f = new Frame();
		f.FrameLen = frameLen;
		f.Version = version;
		f.FrameId = frameId;
		f.FrameType = frameType;
		f.SerializeType = serializeType;
		f.EncryptType = encryptType;
		f.Payload = payloadBytes;

		return f;

	}

	// 加密的帧 ---> 字节
	public static byte[] GenerateFrameBytesWithFrame(Frame frame) {
	    byte[] bytes= new byte[frame.FrameLen];
		NetworkUtils.CastIntToBytes(bytes, 0, frame.FrameLen);
		NetworkUtils.CastIntToBytes(bytes, 4, frame.Version);
		NetworkUtils.CastIntToBytes(bytes, 8, frame.FrameType);
		NetworkUtils.CastIntToBytes(bytes, 12, frame.SerializeType);
		NetworkUtils.CastIntToBytes(bytes, 16, frame.EncryptType);


		byte[] frameIdBytes = System.Text.Encoding.UTF8.GetBytes(frame.FrameId);
		NetworkUtils.Copy(bytes, 20,  frameIdBytes, 0, frameIdBytes.Length);
		NetworkUtils.Copy(bytes, 39, (byte[])(frame.Payload), 0, ((byte[])(frame.Payload)).Length);
		return bytes;
	}




	//字节 ---> 加密的帧 ---> 解密的帧
	public static Frame ResolveFrame(byte[] bytes)
	{
		int frameLen = NetworkUtils.CastBytesToInt(bytes, 0);
		int version = NetworkUtils.CastBytesToInt(bytes, 4);
		int frameType = NetworkUtils.CastBytesToInt(bytes, 8);
		int serializeType = NetworkUtils.CastBytesToInt(bytes, 12);
		int encryptType = NetworkUtils.CastBytesToInt(bytes, 16);


		byte[] frameIdBytes = new byte[19];

		NetworkUtils.Copy(frameIdBytes, 0, bytes, 20, 39);

		byte[] payload = new byte[frameLen - 39];

		NetworkUtils.Copy(payload, 0, bytes, 39, bytes.Length);
		// fmt.Printf("获取到的完整数据:%v\n", bytes)
		// fmt.Printf("获取到的加密后的数据:%v\n", Payload)
		//解密
		string payloadStr = System.Text.Encoding.UTF8.GetString(payload);
		string de = EncryptUtils.Decrypt(payloadStr, encryptType);
		string frameId = System.Text.Encoding.UTF8.GetString(frameIdBytes);
		// 反序列化
		Message msg = (Message)SerializerUtils.UnserializeByType(serializeType, de, frameType);

		Frame frame = new Frame();
		frame.FrameLen = frameLen;
		frame.Version = version;
		frame.FrameId = frameId;
		frame.FrameType = frameType;
		frame.SerializeType = serializeType;
		frame.EncryptType = encryptType;
		frame.Payload = msg;

		return frame;
	}

    public override string ToString()
    {
		return "FrameLen:[" + FrameLen + "]" + " Version:[" + Version + "] FrameType:[" + FrameType + "]"
			+ " SerializeType:[" + SerializeType + "] EncryptType:[" + EncryptType + "] " +
			"FrameId:[" + FrameId + "]" + " Payload:[" + Payload + "]"; 
    }
}
