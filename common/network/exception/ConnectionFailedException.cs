using System;


public class ConnectionFailedException : NetworkException
{
    public ConnectionFailedException(string msg) : base(msg)
    {
    }
}
