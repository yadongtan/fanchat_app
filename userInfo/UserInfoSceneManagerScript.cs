
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UserInfoSceneManagerScript : MonoBehaviour
{

    public Text UsernameText;    // 我的资料 - Username
    public Text UserTTidText;   // 我的资料 - ttid
    public Image UserHeadImg;   //我的资料 - 头像

    AndroidJavaObject jo;

    public void Awake()
    {
        UsernameText.text = MyUserInfo.GetInstance.username;
        UserTTidText.text = "TT号: " + Convert.ToString(MyUserInfo.GetInstance.ttid);


    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void BackToMainScene()
    {
        SceneManager.LoadScene("MainScene");
    }
}
