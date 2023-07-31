using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TopBarsScript : RefreshableUI
{


    // Start is called before the first frame update

    public Button MyUserHeadPicBtn;
    public Text MyUsernameText;
    public Image MyUserNetworkStatus;
    public Text MyUserNetworkStatusDesc;

    public void Awake()
    {
        this.MyUsernameText.text = MyUserInfo.GetInstance.username;
    }

    public void GoToMyUserInfoScene()
    {
        SceneManager.LoadScene("MyUserInfoScene");
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
