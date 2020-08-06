using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;
using TMPro;

public class SayItService : MonoBehaviour
{
    private static SayItService _instance;
    public static SayItService Instance()
    {
        return _instance;
    }
    void Awake()
    {
        _instance = this;
    }

    public bool doDebug;
    public bool WhiteColor;
    public delegate void OnLineEnd();
    private OnLineEnd _onLineEnd;
    private TextMeshProUGUI _dialogText;
    private float _dialogTextFontSize;
    readonly char[] _delimeters = new char[3] { ' ', '.', ',' };
    private readonly Color32 _firstColorWhite = new Color32(81, 81, 81, 255);
    private readonly Color32 _firstColorBlack = new Color32(147, 147, 147, 147);
    // private string _hexColorWhite = "DBDBDB";
    // private string _hexColorBlack = "#343434";

    public void SetLineOptions(TextMeshProUGUI dialogText, OnLineEnd onLineEnd)
    {
        _dialogText = dialogText;
        _dialogTextFontSize = _dialogText.fontSize;
        _onLineEnd = onLineEnd;
    }

    public void SayLine(TextMeshProUGUI dialogText, OnLineEnd onLineEnd, float slowdown = 1)
    {
        SetLineOptions(dialogText, onLineEnd);
        SayLine(dialogText.text, slowdown);
    }

    int _lineIndex;
    bool _firstEntry = false;
    float _timePerChar;
    private readonly string _readColor = "<color=#343434>";
    private readonly string _readColorEnd = "</color>";
    private readonly string _boldStart = "<b>";
    private readonly string _boldEnd = "</b>";
    private string _sizeStart;
    private readonly string _sizeEnd = "</size>";
    private IEnumerator _sayLineByChar;

    void SayLine(string line, float slowdown = 1f)
    {
        _lineIndex = -1;
        _firstEntry = true;
        _sizeStart = "<size=" + (_dialogTextFontSize + 1) + ">";

        if (doDebug == false)
        {
            StartSayLine();
        }
    }

    private void StartSayLine()
    {
        StopSayLine();
        _sayLineByChar = SayLineByChar();
        StartCoroutine(_sayLineByChar);
    }

    private void StopSayLine()
    {
        if (_sayLineByChar != null)
        {
            StopCoroutine(_sayLineByChar);
            _sayLineByChar = null;
        }
    }

    void AdvanceInLine()
    {
        _lineIndex++;
        if (IsLineEnding())
        {
            StopSayLine();
            _onLineEnd();
            return;
        }
        var charToInsert = GetCharToInsert();
        // Debug.Log(charToInsert);
        SetTimePerChar(charToInsert);
        var newWord = GetNewWord(charToInsert);
        // Debug.Log(newWord);
        UpdateDialogText(newWord);
        // Debug.Log("=> " + _dialogText.text);
        if (_firstEntry)
        {
            _lineIndex += (_readColor.Length);
            _firstEntry = false;
        }
    }

    private bool IsLineEnding()
    {
        if (_firstEntry)
        {
            return false;
        }
        int removableCount = (_readColorEnd.Length
            + _boldStart.Length
            + _boldEnd.Length
            + _sizeStart.Length
            + _sizeEnd.Length);
        int cutLength = (_dialogText.text.Length - removableCount);
        // Debug.Log("_lineIndex: " + _lineIndex + " - cutLength: " + cutLength);
        if (_lineIndex >= cutLength)
        {
            return true;
        }
        return false;
    }

    private char GetCharToInsert()
    {
        _dialogText.text = _dialogText.text.Replace(_readColorEnd, "");
        _dialogText.text = _dialogText.text.Replace(_boldStart, "");
        _dialogText.text = _dialogText.text.Replace(_boldEnd, "");
        _dialogText.text = _dialogText.text.Replace(_sizeStart, "");
        _dialogText.text = _dialogText.text.Replace(_sizeEnd, "");
        return _dialogText.text[_lineIndex];
    }

    private void SetTimePerChar(char charToInsert)
    {
        if (_delimeters.Contains(charToInsert))
        {
            _timePerChar = 0.19f;
            return;
        }
        _timePerChar = 0.08f;
    }

    private string GetNewWord(char charToInsert)
    {
        return (_firstEntry ? _readColor : "")
            + _sizeStart
            + _boldStart
            + charToInsert
            + _boldEnd
            + _sizeEnd
            + _readColorEnd;
    }

    private void UpdateDialogText(string newWord)
    {
        StringBuilder stringBuilder = new StringBuilder(_dialogText.text);
        stringBuilder.Remove(_lineIndex, 1);
        stringBuilder.Insert(_lineIndex, newWord);

        _dialogText.text = stringBuilder.ToString();
    }

    private IEnumerator SayLineByChar()
    {
        AdvanceInLine();

        yield return new WaitForSeconds(_timePerChar);

        StartSayLine();
    }

    void Update()
    {
        if (doDebug)
        {
            if (Input.GetKeyUp(KeyCode.Return) || Input.GetKeyUp(KeyCode.KeypadEnter))
            {
                AdvanceInLine();
            }
        }
    }

    private string FixLine(string line)
    {
        line = line.Replace("\\n", "\n");
        line = line.Replace("\n ", "\n");
        return line;
    }
}
