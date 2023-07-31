
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UserInfoSceneManagerScript : MonoBehaviour
{

    public Text UsernameText;    // �ҵ����� - Username
    public Text UserTTidText;   // �ҵ����� - ttid
    public Image UserHeadImg;   //�ҵ����� - ͷ��

    AndroidJavaObject jo;

    public void Awake()
    {
        UsernameText.text = MyUserInfo.GetInstance.username;
        UserTTidText.text = "TT��: " + Convert.ToString(MyUserInfo.GetInstance.ttid);


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
