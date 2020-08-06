using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Overlay : MonoBehaviour
{
    private Image _panel;
    void Awake()
    {
        _panel = GetComponent<Image>();
    }
    private int? _animationId;
    private OverlayTransition _overlayTransition;
    private float _time;
    public delegate void MiddleTransition();
    private MiddleTransition _onMiddleTransition;
    private IEnumerator _middleTransitionCo;

    public void SetTransition(OverlayTransition overlayTransition)
    {
        if (overlayTransition == OverlayTransition.Transparent)
        {
            _panel.color = GameHiddenOptions.Instance.BlackTransparentColor;
        }
        else
        {
            _panel.color = GameHiddenOptions.Instance.BlackColor;
        }
    }
    public void Transition(OverlayTransition overlayTransition, float time = 1f, MiddleTransition onMiddleTransition = null)
    {
        if (_animationId.HasValue)
        {
            Debug.LogWarning("Overlay Transition in progress.");
            return;
            // LeanTween.cancel(_animationId.Value);
            // _animationId = null;
        }

        if (onMiddleTransition != null)
        {
            _onMiddleTransition = onMiddleTransition;
            time = time - 0.1f;
        }
        _overlayTransition = overlayTransition;
        _time = overlayTransition == OverlayTransition.Complete ? (time / 2) : time;

        var fromColor = GameHiddenOptions.Instance.BlackTransparentColor;
        var toColor = GameHiddenOptions.Instance.BlackColor;
        if (overlayTransition == OverlayTransition.Black)
        {
            fromColor = GameHiddenOptions.Instance.BlackColor;
            toColor = GameHiddenOptions.Instance.BlackTransparentColor;
        }

        _panel.color = fromColor;
        _animationId = LeanTween.color(
            _panel.GetComponent<RectTransform>(),
            toColor,
            time
        ).id;
        LeanTween.descr(_animationId.Value).setEase(LeanTweenType.linear);
        LeanTween.descr(_animationId.Value).setOnComplete(OnTransitionEnd);

        Debug.Log("Play transition");
    }

    void OnTransitionEnd()
    {
        LeanTween.cancel(_animationId.Value);
        _animationId = null;
        if (_overlayTransition == OverlayTransition.Complete)
        {
            if (_onMiddleTransition != null)
            {
                _onMiddleTransition();
                _onMiddleTransition = null;

                if (_middleTransitionCo != null)
                {
                    StopCoroutine(_middleTransitionCo);
                }
                _middleTransitionCo = InternalWaitFunction();
                StartCoroutine(_middleTransitionCo);
            }
        }
    }

    private IEnumerator InternalWaitFunction()
    {
        yield return new WaitForSeconds(0.1f);
        Transition(OverlayTransition.Black, _time);
    }
}

public enum OverlayTransition
{
    Transparent,
    Black,
    Complete
}
