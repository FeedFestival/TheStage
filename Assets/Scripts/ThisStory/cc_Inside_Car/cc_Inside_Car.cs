using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class cc_Inside_Car : MonoBehaviour, IAct
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

    public RectTransform Panel;

    public Sprite Img1;
    public Sprite Img2;
    public Sprite Img3;
    public Sprite Img4;
    public Sprite Img5;

    private List<IAction> DefineActions()
    {
        return new List<IAction>
        {
            (new cc_Inside_Car_Action() as IAction)
        };
    }
}

public class cc_Inside_Car_Action : IAction
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
        _cc_Inside_Car = (act as cc_Inside_Car);
        InitActions();
        Animate();
    }

    void IAction.ContinueAction()
    {
        Debug.Log("Continue");
    }
    //---------------------------------------------------------------------------------------
    #endregion

    private cc_Inside_Car _cc_Inside_Car;
    private List<Line> _lines;

    private void InitActions()
    {
        _cc_Inside_Car.Panel.GetComponent<Image>().sprite = _cc_Inside_Car.Img1;
        _cc_Inside_Car.Panel.gameObject.SetActive(true);
        InitLines();
    }

    private void InitLines()
    {
        _lines = new List<Line>()
        {
            new Line() {
                DialogBoxes = new DialogBox[1] {
                    new DialogBox() {
DialogBoxType = DialogBoxType.dbxs,
posX = 1266f, posY = 657f, posZ = 0f, 
width = 372.4297f, height = 315.8203f, 
sizeX = -1f, sizeY = 1f, sizeZ = 1f,
anchorsMinX = 0f, anchorsMinY = 0f, 
anchorsMaxX = 0f, anchorsMaxY = 0f, 
pivotX = 0f, pivotY = 0f, 
rotationX = 0f, rotationY = 0f, rotationZ = 0f , childWidth = 275.2627f, childHeight = 180.2074f, 
childSizeX = -1f, childSizeY = 1f, fontSize = 34.44f
                    }
                },
                Sentences = new string[1] {
                    "Let me take\n you where\n you want to\n goo",
                }
            }
        };

        var index = 0;
        _lines.ForEach(l =>
        {
            for (var i = 0; i < l.DialogBoxes.Length; i++)
            {
                l.DialogBoxes[i].Index = index;
                index++;
            }
        });
    }

    private void Animate()
    {
        TimeService.Instance.Wait(() =>
        {
            _cc_Inside_Car.Panel.GetComponent<Image>().sprite = _cc_Inside_Car.Img2;
            TimeService.Instance.Wait(() =>
            {
                _cc_Inside_Car.Panel.GetComponent<Image>().sprite = _cc_Inside_Car.Img3;
                TimeService.Instance.Wait(() =>
                {
                    _cc_Inside_Car.Panel.GetComponent<Image>().sprite = _cc_Inside_Car.Img4;
                    TimeService.Instance.Wait(() =>
                    {
                        _cc_Inside_Car.Panel.GetComponent<Image>().sprite = _cc_Inside_Car.Img5;
                        DialogueController.Instance.Show(_lines[0], true, () => {});
                        DialogueController.Instance.SayLines();
                    }, 1.5f);
                }, 1.5f);
            }, 1f);
        }, 2f);
    }
}
