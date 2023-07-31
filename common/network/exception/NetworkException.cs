using System;


public abstract class NetworkException : SystemException 
{
    public NetworkException(string msg) : base(msg)
    {
    }
}
