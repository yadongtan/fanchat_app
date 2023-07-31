using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnlinePersonCountTextScript : RefreshableUI
{

    public override void Refresh()
    {
        FantasticNetworkClient.SyncClient.WriteSyncLambda(
        new GetOnlinePersonCountMessage(), (ackMsg) =>
        {
            Loom.QueueOnMainThread((param) =>
            {
                try
                {
                    if (ackMsg.ack == AckMessage.Ok)
                    {
                        this.GetComponent<Text>().text = "当前在线人数: " + Convert.ToInt32(ackMsg.data);
                    }
                    else
                    {
                        this.GetComponent<Text>().text = "当前在线人数: -1";
                    }
                }
                catch (Exception e)
                {
                    this.GetComponent<Text>().text = "当前在线人数: -1";
                }

            }, null);
            return null;
        });
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
