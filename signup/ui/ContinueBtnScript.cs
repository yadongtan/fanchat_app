using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ContinueBtnScript : MonoBehaviour
{

    public Text SignUpSuccessText;
    // Start is called before the first frame update
    void Start()
    {
        SignUpSuccessText.text = "注册成功!\n" +
            "您的账号为: " + MyUserInfo.GetInstance.ttid + "\n"
            + "点击确认返回登录";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GoToLoginScene()
    {
        SceneManager.LoadScene("LoginScene");
    }


}
