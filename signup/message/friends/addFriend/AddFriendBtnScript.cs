using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AddFriendBtnScript : MonoBehaviour
{
    public void GoToAddFriend()
    {
        SceneManager.LoadScene("AddFriendScene");
    }
}
