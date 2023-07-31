using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RefreshableUI : MonoBehaviour
{

    public virtual void Awake()
    {
        SetActiveAndRefresh(true);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetActiveAndRefresh(bool active)
    {
        this.gameObject.SetActive(active);
        if(active == true)
        {
            Refresh();
        }
    }

    public virtual void Refresh()
    {
    }



}
