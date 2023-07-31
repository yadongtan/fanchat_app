using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateOpenAIServiceScripts : MonoBehaviour
{

    public Text Prompt;
    public void CreateGPT3_5_Turbo()
    {
        string model = OpenAIServiceInfo.GPT3_5_Turbo;  //创建模型
        CreateChatMessage msg = new CreateChatMessage();
        msg.ttid = MyUserInfo.GetInstance.ttid;
        msg.username = MyUserInfo.GetInstance.username;
        msg.model = model;

        FantasticNetworkClient.SyncClient.WriteSyncLambda(msg, (ackMsg) =>
        {
            if (ackMsg.ack == AckMessage.Ok)
            {
                Debug.Log("创建OpenAI模型 " + model + " 成功! msg:" + ackMsg.msg);
                Loom.QueueOnMainThread((param) =>
                {
                    Prompt.text = "创建模型[" + model + "]成功!";
                    StartCoroutine(ShowPromptText());
                }, null);
            }
            else
            {
                Debug.Log("创建OpenAI模型 " + model + " 失败! msg:" + ackMsg.msg);
                Loom.QueueOnMainThread((param) =>
                {
                    Prompt.text = "创建模型[" + model + "]失败!";
                    StartCoroutine(ShowPromptText());
                }, null);
            }
            return null;
        });
    }


    public IEnumerator ShowPromptText()
    {
        Prompt.gameObject.SetActive(true);
        for (float timeSet = 5; timeSet >= 0; timeSet -= Time.deltaTime)
            yield return null;
        Prompt.gameObject.SetActive(false);
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
