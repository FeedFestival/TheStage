using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueController : MonoBehaviour
{
    private static DialogueController _instance;
    public static DialogueController Instance { get { return _instance; } }
    void Awake()
    {
        _instance = this;
    }

    public Transform DialogueBoxesParent;
    private Dictionary<string, GameObject> _dialoguePrefabs;
    private List<SpeechBuble> _speechBublePool;
    private List<SpeechBuble> _connectorPool;
    public Text CurrentDialogueBox;
    private Line _currentLine;
    private int _dialogBoxIndex;
    private int _index;
    private int _sentenceIndex;
    private IEnumerator _startSayLine;
    private SayLineFinished _onSayLinesFinished;

    [Header("DialoguePrefabs")]
    public GameObject dbs;
    public GameObject dbsr;
    public GameObject dbse;
    public GameObject dbxs;
    public GameObject connector;

    //-------------------------------------

    public void Init()
    {
        foreach (Transform child in DialogueBoxesParent)
        {
            GameObject.Destroy(child.gameObject);
        }
        _dialoguePrefabs = new Dictionary<string, GameObject>()
        {
            { "dbs", dbs },
            { "dbsr", dbsr },
            { "dbse", dbse },
            { "dbxs", dbxs },
            { "connector", connector }
        };
    }

    public void Show(Line line = null, bool initial = false, SayLineFinished onSayLinesFinished = null)
    {
        if (initial)
        {
            Reset(line, onSayLinesFinished);
            foreach (var dialogBox in line.DialogBoxes)
            {
                CreateDialogueBoxes(dialogBox);
                _sentenceIndex++;
            }
            if (line.Connectors != null)
            {
                foreach (var conn in line.Connectors)
                {
                    CreateConnector(conn);
                }
            }
        }
    }

    public void Hide()
    {
        Reset();
        foreach (var speechBuble in _speechBublePool)
        {
            speechBuble.SetDisabled();
        }
        foreach (var connector in _connectorPool)
        {
            connector.SetDisabled();
        }
    }

    private void Reset(Line line = null, SayLineFinished onSayLinesFinished = null)
    {
        _currentLine = line;
        _dialogBoxIndex = 0;
        _onSayLinesFinished = onSayLinesFinished;
        _sentenceIndex = 0;
    }

    internal void SayLines()
    {
        if (_startSayLine != null)
        {
            StopCoroutine(_startSayLine);
            _startSayLine = null;
        }
        _startSayLine = StartSayLine();
        StartCoroutine(_startSayLine);
    }

    IEnumerator StartSayLine()
    {
        yield return new WaitForSeconds(GameHiddenOptions.Instance.TimeBeforeSayLine);
        var speechBuble = GetDialoguePrefab();
        speechBuble.SayLine();
    }

    void SayLineFinished()
    {
        _dialogBoxIndex++;
        if (_dialogBoxIndex >= _currentLine.DialogBoxes.Length)
        {
            _currentLine = null;
            if (_onSayLinesFinished != null)
            {
                _onSayLinesFinished();
            }
            return;
        }
        SayLines();
    }

    private SpeechBuble GetDialoguePrefab()
    {
        DialogBox dialogBox = _currentLine.DialogBoxes[_dialogBoxIndex];
        return _speechBublePool.FirstOrDefault(s =>
            s.gameObject.name == dialogBox.DialogBoxType.ToString()
            && s.gameObject.activeSelf == true
            && s.Index == dialogBox.Index
            );
    }

    void CreateDialogueBoxes(DialogBox dialogBox)
    {
        GameObject go = GetPoolGo(dialogBox, ref _speechBublePool);

        SetProperties(ref go, ref dialogBox, true);

        go.GetComponent<SpeechBuble>().InitLine(_currentLine.Sentences[_sentenceIndex], dialogBox.Index, SayLineFinished);
        go.GetComponent<SpeechBuble>().SetActive();
    }

    void CreateConnector(DialogBox connector)
    {
        GameObject go = GetPoolGo(connector, ref _connectorPool);

        SetProperties(ref go, ref connector);
        go.GetComponent<SpeechBuble>().SetActive();
    }

    private GameObject GetPoolGo(DialogBox dialogBox, ref List<SpeechBuble> pool)
    {
        string name = dialogBox.DialogBoxType.ToString();
        if (_dialoguePrefabs.ContainsKey(name))
        {
            if (pool == null)
            {
                pool = new List<SpeechBuble>();
            }
            var existingGo = pool.FirstOrDefault(s => s.gameObject.name == name && s.gameObject.activeSelf == false);
            if (existingGo != null)
            {
                return existingGo.gameObject;
            }
            else
            {
                var go = GameHiddenOptions.Instance.GetAnInstantiated(_dialoguePrefabs[name]);
                go.SetActive(false);
                pool.Add(go.GetComponent<SpeechBuble>());
                return go;
            }
        }
        return null;
    }

    void SetProperties(ref GameObject go, ref DialogBox dialogBox, bool hasChild = false)
    {
        go.name = dialogBox.DialogBoxType.ToString();
        go.transform.SetParent(DialogueBoxesParent, false);
        var rT = go.GetComponent<RectTransform>();
        rT.anchoredPosition3D = new Vector3(dialogBox.posX, dialogBox.posY, dialogBox.posZ);
        rT.sizeDelta = new Vector2(dialogBox.width, dialogBox.height);
        rT.anchorMin = new Vector2(dialogBox.anchorsMinX, dialogBox.anchorsMinY);
        rT.anchorMax = new Vector2(dialogBox.anchorsMaxX, dialogBox.anchorsMaxY);
        rT.pivot = new Vector2(dialogBox.pivotX, dialogBox.pivotY);
        rT.eulerAngles = new Vector3(dialogBox.rotationX, dialogBox.rotationY, dialogBox.rotationZ);
        rT.localScale = new Vector3(dialogBox.sizeX, dialogBox.sizeY, dialogBox.sizeZ);

        if (hasChild)
        {
            var crT = rT.GetChild(0).GetComponent<RectTransform>();
            crT.sizeDelta = new Vector2(dialogBox.childWidth, dialogBox.childHeight);
            crT.localScale = new Vector2(dialogBox.childSizeX, dialogBox.childSizeY);
            crT.GetComponent<TextMeshProUGUI>().fontSize = dialogBox.fontSize;
        }
    }
}
