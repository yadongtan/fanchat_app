using UnityEngine;
using UnityEngine.UI;

public class ButtonClickHandler : MonoBehaviour
{
    public GameObject menuPanel; // �����˵���������
    public GameObject moreMenuBg;   //���˲˵����������ط�, ���������˵�
    private bool isMenuOpen = false; // ��¼�˵��Ƿ��Ѿ��򿪵ı�־

    public void Start()
    {
        menuPanel.SetActive(false);
    }
    public void ToggleMenu()
    {

        // �л��˵�����ʾ״̬
        isMenuOpen = !isMenuOpen;
        menuPanel.SetActive(isMenuOpen);
        moreMenuBg.SetActive(isMenuOpen);
    }

    public void HideMenu()
    {
        if (isMenuOpen)
        {
            // ���ز˵�
            isMenuOpen = false;
            menuPanel.SetActive(false);
            moreMenuBg.SetActive(false);
        }
    }
}
