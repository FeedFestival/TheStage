using System.Collections;
using System.Collections.Generic;
using Hellmade.Sound;
using UnityEngine;
using UnityEngine.UI;

public class HelloAct_Action_1 : MonoBehaviour, IAction
{
    #region IAction
    //---------------------------------------------------------------------------------------
    private FinishEvent _onFinish;

    void IAction.InitAction(FinishEvent onFinish)
    {
        _onFinish = onFinish;
        InitLines();
    }

    void IAction.Do(IAct act)
    {
        Start_HelloAct_Action_1();
    }

    void IAction.ContinueAction()
    {
        WomenSaysSomethingElse();
    }
    //---------------------------------------------------------------------------------------
    #endregion

    public Image Panel;
    private List<Line> _lines;

    void Start_HelloAct_Action_1()
    {
        MusicManager.Instance.PlayRequiredBackgroundMusic("CreepyMusic1", true, 10f);
        MusicManager.Instance.PlayRequiredAmbient("TrainAmbient", true);

        DialogueController.Instance.Show(_lines[0], true, OldManReply);
        DialogueController.Instance.SayLines();
    }

    void OldManReply()
    {
        DialogueController.Instance.Show(_lines[1], true, RequestContinue);
        DialogueController.Instance.SayLines();
    }

    void RequestContinue()
    {
        ActController.Instance.RequestContinue();
    }

    void WomenSaysSomethingElse()
    {
        DialogueController.Instance.Hide();
    }

    IEnumerator WaitABit()
    {
        yield return new WaitForSeconds(0f);

    }

    private void InitLines()
    {
        _lines = new List<Line>()
        {
            new Line() {
                DialogBoxes = new DialogBox[2] {
                    new DialogBox() {
                        DialogBoxType = DialogBoxType.dbxs,
posX = 562.6f, posY = 801.7f, posZ = 0f, width = 128.7f, height = 116f, sizeX = 1f, sizeY = 1f, sizeZ = 1f,
anchorsMinX = 0f, anchorsMinY = 0f, anchorsMaxX = 0f, anchorsMaxY = 0f, pivotX = 0f, pivotY = 0f,
rotationX = 0f, rotationY = 0f, rotationZ = 0f , childWidth = 72.12f, childHeight = 50f
                    },
                    new DialogBox() {
                        DialogBoxType = DialogBoxType.dbse,
posX = 427f, posY = 860f, posZ = 0f, width = 235.8f, height = 201.9f, sizeX = 1f, sizeY = 1f, sizeZ = 1f,
anchorsMinX = 0f, anchorsMinY = 0f, anchorsMaxX = 0f, anchorsMaxY = 0f, pivotX = 0f, pivotY = 0f,
rotationX = 0f, rotationY = 0f, rotationZ = 0f , childWidth = 157.7f, childHeight = 85.7f
                    }
                },
                Connectors = new DialogBox[1] {
                    new DialogBox() {
                        DialogBoxType = DialogBoxType.connector,
posX = 610.1013f, posY = 874.0718f, posZ = 0f, width = 37.51f, height = 39.75f, sizeX = 1f, sizeY = 1f, sizeZ = 1f,
anchorsMinX = 0f, anchorsMinY = 0f, anchorsMaxX = 0f, anchorsMaxY = 0f, pivotX = 0f, pivotY = 0f,
rotationX = 0f, rotationY = 0f, rotationZ = 55.17437f
                    }
                },
                Sentences = new string[2] {
                    "Hello",
                    "Where \nare you \nheading?"
                }
            },
            new Line() {
                DialogBoxes = new DialogBox[1] {
                    new DialogBox() {
                        DialogBoxType = DialogBoxType.dbsr,
posX = 1204f, posY = 864.91f, posZ = 0f, width = 258.42f, height = 215.09f, sizeX = 1f, sizeY = 1f, sizeZ = 1f,
anchorsMinX = 0f, anchorsMinY = 0f, anchorsMaxX = 0f, anchorsMaxY = 0f, pivotX = 0f, pivotY = 0f,
rotationX = 0f, rotationY = 0f, rotationZ = 0f , childWidth = 194.4f, childHeight = 50f
                    }
                },
                Sentences = new string[1] {
                    "I'm heading \nto Constanta \nCity"
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
}
