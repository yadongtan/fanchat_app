using System.Collections.Generic;




public class HandlerChain
{
    List<Handler> handlers;

    public HandlerChain()
    {
        handlers = new List<Handler>();
    }

    public void AddHandler(Handler handler)
    {
        handlers.Add(handler);
    }

    //启动链路
    public void TriggerHandler(ChannelContext ctx, object obj)
    {
        for(int i = 0; i < handlers.Count; i++)
        {
            Handler handler = handlers[i];
            obj = handler.Read(ctx, obj);
        }
    }


    public void Read(ChannelContext ctx)
    {
        TriggerHandler(ctx, null);
    }

    public object Write(ChannelContext ctx, object obj)
    {
        return TriggerNextWriteHandler(ctx, null, obj);
    }

    public object TriggerNextWriteHandler(ChannelContext ctx, Handler handler, object obj)
    {
        int index = 0;
        if (handler == null)
        {
            return handlers[handlers.Count - 1].Write(ctx, obj);
        }
        else
        {
            //先找到当前的
            for (int i = handlers.Count - 1; i >= 0; i--)
            {
                if (this.handlers[i] == handler)
                {
                    index = i - 1;
                    break;
                }
            }
        }

        if (index >= 0)
        {
            return this.handlers[index].Write(ctx, obj);

        }
        else
        {
            return null;
        }
    }
}
