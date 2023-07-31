using System;


public class WriteFailedException : NetworkException
{
    public WriteFailedException(string msg) : base(msg)
    {

    }
}
