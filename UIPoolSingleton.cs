using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPoolSingleton
{
    private Dictionary<string, UIBasePanel> Pool { get; set; }

    static private UIPoolSingleton _instance;
    static public UIPoolSingleton Instance
    {
        get 
        {
            if (_instance == null)
            {
                _instance = new UIPoolSingleton();
                _instance.Initial();
            } 
            return _instance;
        }
    }

    public void Initial()
    {
        Pool = new Dictionary<string, UIBasePanel>();
    }

    public bool Contain(UIReq req)
    {
        return Pool.ContainsKey(req.Path);
    }

    public bool Contain(string path)
    {
        return Pool.ContainsKey(path);
    }

    public UIBasePanel FetchPanelFromPool(UIReq req)
    {
        if (Contain(req))  return Pool[req.Path];
       
        throw new InvalidOperationException("UIPool doesn't contain it!");
    }

    public void AddUItoPool(UIReq req)
    {
        this.Pool[req.Path] = req.UI;
    }

    public void RemoveUIfromPool(UIReq req)
    {
        if (!Contain(req)) return;
        this.Pool.Remove(req.Path);
    }

    public void RemoveUIfromPool(string path)
    {
        if (!Pool.ContainsKey(path)) return;
        this.Pool.Remove(path);
    }


	
}
