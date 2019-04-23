using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHandlerSingleton : MonoBehaviour
{
    #region Initial SingleTon

    static private UIHandlerSingleton _instance;
    static public UIHandlerSingleton Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject obj = new GameObject("UIPool", typeof(UIHandlerSingleton));
                DontDestroyOnLoad(obj);
                _instance = obj.GetComponent<UIHandlerSingleton>();
            }
            return Instance;
        }
    }
    #endregion


    #region UI Action
  
    public IEnumerator CreateThenShow(GameObject parent, UIReq req)
    {
        yield return StartCoroutine(Create(parent, req));
        yield return StartCoroutine(Show(req));
    }

    public IEnumerator CreateThenShowWaiting(GameObject parent, UIReq req)
    {
        yield return StartCoroutine(CreateThenShow(parent, req));
        yield return StartCoroutine(req.UI.Wait());
        yield return StartCoroutine(Hide(req.UI));
        Remove(req.UI);
    }


    private IEnumerator Create(GameObject parent, UIReq req)
    {
        if (UIPoolSingleton.Instance.Contain(req))
        {
            req.UI = FetchUIFromPool(req);
        }
        else
        {
            ResourceRequest resource = Resources.LoadAsync(req.Path, typeof(GameObject));
            yield return resource;
            if (resource.asset == null) throw new InvalidCastException("Unable To Load UI: Action Failed");
            req.UI = FetchUIFromAsset(resource);
            req.UI.Initial(req);
            if (req.IsUseable) UIPoolSingleton.Instance.AddUItoPool(req);
        }
        req.UI.transform.SetParent(parent.transform, false);
        yield break;
    }

    private IEnumerator Show(UIReq req)
    {
        req.UI.Show();
        yield break;
    }




    public IEnumerator Hide(UIBasePanel panel)
    {
        yield return StartCoroutine(panel.Hide());
        Remove(panel);
    }



    public void Remove(UIBasePanel panel)
    {
        if (UIPoolSingleton.Instance.Contain(panel.PanelStruct.Path))
        {
            panel.transform.SetParent(transform, false);
        }
        else
        {
            GameObject.Destroy(panel.gameObject);
        }
    }


  


    #endregion


    #region The Ways Of Fetching UI

    private UIBasePanel FetchUIFromAsset(ResourceRequest resource)
    {
        var child = GameObject.Instantiate(resource.asset) as GameObject;
        Transform trans = child.transform;
        return trans.GetComponent<UIBasePanel>();
    }


    private UIBasePanel FetchUIFromPool(UIReq req)
    {
        return UIPoolSingleton.Instance.FetchPanelFromPool(req);
    }

    #endregion

 


}
