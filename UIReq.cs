using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class UIReq 
{
    public abstract string Path { get; }

    public abstract bool IsUseable { get; }

    public abstract object[] InitData { get; protected set; }

    public UIBasePanel UI { get; set; }

    public UIReq()
    {
        InitData = null;
    }



}



