using UnityEngine;

public class HideMenuOnClick : MonoBehaviour
{
    public ButtonClickHandler clickHandler;
    public GameObject bg;

    public void Start()
    {
        bg.SetActive(false);
    }

    private void Update()
    {
    }

    public void ClickHideMenu()
    {
        clickHandler.HideMenu();
    }
}
