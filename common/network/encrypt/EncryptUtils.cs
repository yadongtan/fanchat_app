



public class EncryptUtils
{
    public const int AESEncryptType = 1;

    public static string Encrypt(string data, int encryptType)
    {
        switch (encryptType)
        {
            case AESEncryptType:
                return AES.Encrypt(data);
        }
        throw new System.Exception("错误, 未指定加密类型");
    }

    public static string Decrypt(string data, int encryptType) {
        switch (encryptType)
        {
            case AESEncryptType:
                return AES.Decrypt(data);
        }

        throw new System.Exception("错误, 未指定解密类型");
    }



}
