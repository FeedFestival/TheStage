using System.Collections.Generic;
using UnityEngine;

public delegate void FinishEvent();

public enum TStyle
{
    None,
    Overlay
}

public interface IAct
{
    void InitAct(FinishEvent onFinish);
    void ContinueAct();
    void ContinueCurrentAction();

    List<IAction> Actions { get; set; }
    GameObject GameObject { get; }
}