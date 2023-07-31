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
        SignUpSuccessText.text = "ע��ɹ�!\n" +
            "�����˺�Ϊ: " + MyUserInfo.GetInstance.ttid + "\n"
            + "���ȷ�Ϸ��ص�¼";
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
