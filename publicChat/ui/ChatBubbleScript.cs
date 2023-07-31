using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatBubbleScript : MonoBehaviour
{
    public Text name;
    public Text msg;
    public Image headImg;

    public void SetNameAndMsg(string username, string msg)
    {
        this.name.text = username;
        this.msg.text = msg;
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
