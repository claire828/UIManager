using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class UIBasePanel : MonoBehaviour {

    public BasePanelStruct PanelStruct { get; set;}
    private bool InWaitingState { get; set; }
    private bool WaitingForFinish { get; set; }

    public void Initial(UIReq req)
    {
        WaitingForFinish = false;
        this.PanelStruct = new BasePanelStruct(req);
    }

    #region override funcs If You need to manipulate data before Its Action
    public virtual IEnumerator ImplementUIBeforeAppearance()
    {
        yield break;
    }

    public virtual IEnumerator ImplementUIBeforeHide()
    {
        yield break;
    }

    public virtual IEnumerator ImplementUIBeforeDestroy()
    {
        yield break;
    }
    #endregion




    public IEnumerator Show()
    {
        yield return StartCoroutine(ImplementUIBeforeAppearance());
        gameObject.SetActive(true);
        yield break;
    }

    public IEnumerator Wait()
    {
        InWaitingState = true;
        WaitingForFinish = true;
        if (WaitingForFinish)
        {
            yield return null;
        }
        InWaitingState = false;
    }


    public IEnumerator Hide()
    {
        yield return StartCoroutine(ImplementUIBeforeHide());
        gameObject.SetActive(false);
        yield break;
    }

    void OnDestroy()
    {
        ImplementUIBeforeDestroy();
    }

    public void Close()
    {
        if (InWaitingState)
        {
            this.WaitingForFinish = false;
        }
        else
        {
            UIHandlerSingleton.Instance.Hide(this);
        }  
    
    }




}



public struct BasePanelStruct
{
    public string Path { get; private set; }

    public object[] Datas { get; private set; }

    public BasePanelStruct(UIReq req)
    {
        this.Path = req.Path;
        this.Datas = req.InitData;
    }

}