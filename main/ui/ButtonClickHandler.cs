using UnityEngine;
using UnityEngine.UI;

public class ButtonClickHandler : MonoBehaviour
{
    public GameObject menuPanel; // 弹出菜单面板的引用
    public GameObject moreMenuBg;   //除了菜单面板的其他地方, 点击就收起菜单
    private bool isMenuOpen = false; // 记录菜单是否已经打开的标志

    public void Start()
    {
        menuPanel.SetActive(false);
    }
    public void ToggleMenu()
    {

        // 切换菜单的显示状态
        isMenuOpen = !isMenuOpen;
        menuPanel.SetActive(isMenuOpen);
        moreMenuBg.SetActive(isMenuOpen);
    }

    public void HideMenu()
    {
        if (isMenuOpen)
        {
            // 隐藏菜单
            isMenuOpen = false;
            menuPanel.SetActive(false);
            moreMenuBg.SetActive(false);
        }
    }
}
