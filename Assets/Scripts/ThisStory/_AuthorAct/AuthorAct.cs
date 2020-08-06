using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuthorAct : MonoBehaviour, IAct
{
    #region IAct
    //---------------------------------------------------------------------------------------
    private FinishEvent _onFinish;
    private List<IAction> _actions;
    List<IAction> IAct.Actions { get => _actions; set => _actions = value; }
    GameObject IAct.GameObject { get => this.gameObject; }

    private int _actionIndex;
    private int _actionsLength;

    void IAct.InitAct(FinishEvent onFinish)
    {
        _onFinish = onFinish;

        _actions = DefineActions();
        _actions.ForEach(a =>
        {
            a.InitAction((this as IAct).ContinueAct);
        });
        _actionIndex = -1;
        _actionsLength = _actions.Count;
    }

    void IAct.ContinueAct()
    {
        _actionIndex++;
        if (_actionIndex == _actionsLength)
        {
            _onFinish();
            return;
        }
        _actions[_actionIndex].Do();
    }

    void IAct.ContinueCurrentAction() {
        _actions[_actionIndex].ContinueAction();
    }
    //---------------------------------------------------------------------------------------
    #endregion

    public AuthorAction AuthorAction;

    private List<IAction> DefineActions()
    {
        return new List<IAction>
        {
            (AuthorAction as IAction)
        };
    }
}
