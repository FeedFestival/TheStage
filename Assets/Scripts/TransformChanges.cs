using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformChanges : MonoBehaviour
{
    [SerializeField]
    public bool TransformAnchorPos;
    [SerializeField]
    public bool TransformDelta;
    [SerializeField]
    public bool TransformRotation;

    //
    [SerializeField]
    public bool OriginalFoldout = true;
    [SerializeField]
    public bool OriginalLocked = true;
    [SerializeField]
    public Vector3 OriginalAnchorPos;
    [SerializeField]
    public Vector2 OriginalScaleDelta;
    [SerializeField]
    public Vector3 OriginalRotation;
    
    //
    [SerializeField]
    public bool ToSecondFoldout = true;
    [SerializeField]
    public bool SecondLocked = false;
    [SerializeField]
    public Vector3 SecondAnchorPos;
    [SerializeField]
    public Vector2 SecondScaleDelta;
    [SerializeField]
    public Vector3 SecondRotation;

    [SerializeField]
    public bool MoreThenOne = false;
    [SerializeField]
    public bool UseThird = false;
    [SerializeField]
    public bool ToThirdFoldout = true;
    [SerializeField]
    public bool ThirdLocked = false;
    [SerializeField]
    public Vector3 ThirdAnchorPos;
    [SerializeField]
    public Vector2 ThirdScaleDelta;
    [SerializeField]
    public Vector3 ThirdRotation;

    public bool UseForth = false;

    public void SetCurrent()
    {
        var rt = GetComponent<RectTransform>();
        OriginalAnchorPos = rt.anchoredPosition;
        OriginalScaleDelta = rt.sizeDelta;
        OriginalRotation = rt.eulerAngles;
    }

    public void ViewNow()
    {
        var rt = GetComponent<RectTransform>();
        rt.anchoredPosition = OriginalAnchorPos;
        rt.sizeDelta = OriginalScaleDelta;
        rt.eulerAngles = OriginalRotation;
    }

    public void SetCurrentSecondary()
    {
        var rt = GetComponent<RectTransform>();
        SecondAnchorPos = rt.anchoredPosition;
        SecondScaleDelta = rt.sizeDelta;
        SecondRotation = rt.eulerAngles;
    }

    public void ViewNowSecondary()
    {
        var rt = GetComponent<RectTransform>();
        rt.anchoredPosition = SecondAnchorPos;
        rt.sizeDelta = SecondScaleDelta;
        rt.eulerAngles = SecondRotation;
    }

    public void SetCurrentThird()
    {
        var rt = GetComponent<RectTransform>();
        ThirdAnchorPos = rt.anchoredPosition;
        ThirdScaleDelta = rt.sizeDelta;
        ThirdRotation = rt.eulerAngles;
    }

    public void ViewNowThird()
    {
        var rt = GetComponent<RectTransform>();
        rt.anchoredPosition = ThirdAnchorPos;
        rt.sizeDelta = ThirdScaleDelta;
        rt.eulerAngles = ThirdRotation;
    }
}
