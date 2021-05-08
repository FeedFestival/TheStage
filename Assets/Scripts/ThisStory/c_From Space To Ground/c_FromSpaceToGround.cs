using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.utils;
using UnityEngine;
using UnityEngine.UI;

public class c_FromSpaceToGround : MonoBehaviour, IAct
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
        _actions[_actionIndex].Do(this);
    }

    void IAct.ContinueCurrentAction()
    {
        _actions[_actionIndex].ContinueAction();
    }
    //---------------------------------------------------------------------------------------
    #endregion

    public RectTransform MainPanelToMove;
    public RectTransform Car;
    public RectTransform CarPanel1;
    public RectTransform CarPanel2;
    public RectTransform CarPanel3;
    public RectTransform CarPanel4;

    private List<IAction> DefineActions()
    {
        return new List<IAction>
        {
            (new c_FromSpaceToGround_Action() as IAction)
        };
    }
}

public class c_FromSpaceToGround_Action : IAction
{
    #region IAction
    //---------------------------------------------------------------------------------------
    private FinishEvent _onFinish;

    void IAction.InitAction(FinishEvent onFinish)
    {
        _onFinish = onFinish;
    }

    void IAction.Do(IAct act)
    {
        _c_FromSpaceToGround = (act as c_FromSpaceToGround);
        InitActions();
        MovePanelSlowly(PanelPosition.First);
    }

    void IAction.ContinueAction() { }
    //---------------------------------------------------------------------------------------
    #endregion

    private c_FromSpaceToGround _c_FromSpaceToGround;

    private float _movePanelTime = 10;
    private float _movePanelSecondTime = 5f;
    private int? _movePanelId;

    private enum PanelPosition
    {
        First, Second
    }

    private float _zoomPanelTime = 4f;

    private void InitActions()
    {
        _c_FromSpaceToGround.MainPanelToMove.gameObject.SetActive(true);
        var tChanges = _c_FromSpaceToGround.MainPanelToMove.GetComponent<TransformChanges>();
        _c_FromSpaceToGround.MainPanelToMove.anchoredPosition = tChanges.OriginalAnchorPos;

        _c_FromSpaceToGround.Car.gameObject.SetActive(true);
        __utils.SetImageAlpha(_c_FromSpaceToGround.Car.GetComponent<Image>(), 0f);
        _c_FromSpaceToGround.CarPanel1.gameObject.SetActive(true);
        __utils.SetImageAlpha(_c_FromSpaceToGround.CarPanel1.GetComponent<Image>(), 0f);

        _c_FromSpaceToGround.CarPanel2.gameObject.SetActive(true);
        __utils.SetImageAlpha(_c_FromSpaceToGround.CarPanel2.GetComponent<Image>(), 0f);
        tChanges = _c_FromSpaceToGround.CarPanel2.GetComponent<TransformChanges>();
        _c_FromSpaceToGround.CarPanel2.anchoredPosition = tChanges.OriginalAnchorPos;
        _c_FromSpaceToGround.CarPanel2.sizeDelta = tChanges.OriginalScaleDelta;
        _c_FromSpaceToGround.CarPanel2.eulerAngles = tChanges.OriginalRotation;

        _c_FromSpaceToGround.CarPanel3.gameObject.SetActive(true);
        tChanges = _c_FromSpaceToGround.CarPanel3.GetComponent<TransformChanges>();
        _c_FromSpaceToGround.CarPanel3.anchoredPosition = tChanges.OriginalAnchorPos;
        _c_FromSpaceToGround.CarPanel3.sizeDelta = tChanges.OriginalScaleDelta;
        __utils.SetImageAlpha(_c_FromSpaceToGround.CarPanel3.GetComponent<Image>(), 0f);

        _c_FromSpaceToGround.CarPanel4.gameObject.SetActive(true);
        __utils.SetImageAlpha(_c_FromSpaceToGround.CarPanel4.GetComponent<Image>(), 0f);
    }

    private void MovePanelSlowly(PanelPosition panelPosition)
    {
        var tChanges = _c_FromSpaceToGround.MainPanelToMove.GetComponent<TransformChanges>();
        if (panelPosition == PanelPosition.First)
        {
            _movePanelId = LeanTween.move(_c_FromSpaceToGround.MainPanelToMove, tChanges.SecondAnchorPos, _movePanelTime).id;
            LeanTween.descr(_movePanelId.Value).setEase(LeanTweenType.easeInOutQuad);
            var panelTp = percent.Find(_percent: 80, _of: _movePanelTime);
            TimeService.Instance.Wait(ShowCar, panelTp);
            LeanTween.descr(_movePanelId.Value).setOnComplete(() =>
            {
                LeanTween.cancel(_movePanelId.Value);
                _movePanelId = null;
                TimeService.Instance.Wait(() =>
                {
                    MovePanelSlowly(PanelPosition.Second);
                }, 1f);
            });
        }
        else
        {
            _movePanelId = LeanTween.move(_c_FromSpaceToGround.MainPanelToMove, tChanges.ThirdAnchorPos, _movePanelSecondTime).id;
            var panelTp = percent.Find(_percent: 20, _of: _movePanelSecondTime);
            TimeService.Instance.Wait(ShowPanelCar1, panelTp);
            LeanTween.descr(_movePanelId.Value).setOnComplete(() =>
            {
                LeanTween.cancel(_movePanelId.Value);
                _movePanelId = null;
            });
        }
    }

    private void ShowCar()
    {
        LeanTween.alpha(_c_FromSpaceToGround.Car, 1f, 1f)
            .setEase(LeanTweenType.linear);
    }

    private void ShowPanelCar1()
    {
        LeanTween.alpha(_c_FromSpaceToGround.CarPanel1, 1f, 0.5f)
                    .setEase(LeanTweenType.linear)
                    .setOnComplete(() =>
                    {
                        TimeService.Instance.Wait(ShowPanelCar2, 2f);
                    });
    }

    private void ShowPanelCar2()
    {
        LeanTween.alpha(_c_FromSpaceToGround.CarPanel2, 1f, 0.5f)
                            .setEase(LeanTweenType.linear)
                            .setOnComplete(ZoomInPanelCar2);
    }

    private void ZoomInPanelCar2()
    {
        TimeService.Instance.Wait(() =>
        {
            var tChanges = _c_FromSpaceToGround.CarPanel2.GetComponent<TransformChanges>();
            LeanTween.move(_c_FromSpaceToGround.CarPanel2, tChanges.SecondAnchorPos, _zoomPanelTime)
                .setEase(LeanTweenType.easeOutCirc);
            LeanTween.value(_c_FromSpaceToGround.CarPanel2.gameObject, tChanges.OriginalScaleDelta, tChanges.SecondScaleDelta, _zoomPanelTime)
                .setEase(LeanTweenType.easeOutCirc)
                .setOnUpdate((Vector2 newValue) =>
                {
                    _c_FromSpaceToGround.CarPanel2.sizeDelta = new Vector2(newValue.x, newValue.y);
                });
            LeanTween.value(_c_FromSpaceToGround.CarPanel2.gameObject, tChanges.OriginalRotation, tChanges.SecondRotation, _zoomPanelTime)
                .setEase(LeanTweenType.easeOutCirc)
                .setOnUpdate((Vector3 newValue) =>
                {
                    _c_FromSpaceToGround.CarPanel2.eulerAngles = newValue;
                });

            var timeP = percent.Find(_percent: 70, _of: _zoomPanelTime);
            TimeService.Instance.Wait(ShowCarPanel3, timeP);
        }, 0.5f);
    }

    private void ShowCarPanel3()
    {
        float timeToShow = 2f;
        LeanTween.alpha(_c_FromSpaceToGround.CarPanel3, 1f, timeToShow)
            .setEase(LeanTweenType.linear);
        var timeP = percent.Find(_percent: 60, _of: timeToShow);

        TimeService.Instance.Wait(() =>
        {
            var panel3Time = 4f;
            var panel3P = percent.Find(_percent: 90, _of: panel3Time);
            var tChanges = _c_FromSpaceToGround.CarPanel3.GetComponent<TransformChanges>();

            LeanTween.value(_c_FromSpaceToGround.CarPanel3.gameObject,
                tChanges.OriginalScaleDelta,
                tChanges.SecondScaleDelta, panel3Time)
                .setEase(LeanTweenType.linear)
                .setOnUpdate((Vector2 newValue) =>
                {
                    _c_FromSpaceToGround.CarPanel3.sizeDelta = new Vector2(newValue.x, newValue.y);
                });
            LeanTween.move(_c_FromSpaceToGround.CarPanel3,
                new Vector2(
                    tChanges.SecondAnchorPos.x,
                    tChanges.SecondAnchorPos.y), panel3Time)
                .setEase(LeanTweenType.linear);

            TimeService.Instance.Wait(() =>
            {
                ShowCarPanel4();
            }, panel3P);
        }, timeP);
    }

    private void ShowCarPanel4()
    {
        LeanTween.alpha(_c_FromSpaceToGround.CarPanel4, 1f, 1f)
            .setEase(LeanTweenType.linear)
            .setOnComplete(() => { _onFinish(); });
    }
}