using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BottomBarsScript : RefreshableUI
{


    public void OnClickMessageBtn()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void OnClickFriendsBtn()
    {
        SceneManager.LoadScene("FriendsScene");
    }

    public void OnClickPublicChatBtn()
    {
        SceneManager.LoadScene("PublicChatScene");
    }




    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void Refresh()
    {
    }

}
