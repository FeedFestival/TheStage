using System.Collections;
using System.Collections.Generic;
using Hellmade.Sound;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AuthorAction : MonoBehaviour, IAction
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
        PlayTestAnimation();
    }

    void IAction.ContinueAction()
    {
        _onFinish();
    }
    //---------------------------------------------------------------------------------------
    #endregion

    public Image Panel;
    public TextMeshProUGUI AuthorName;
    public float AnimationTime;

    private int? _animationId;
    private int? _textAnimationId;

    void PlayTestAnimation()
    {
        MusicManager.Instance.PlayBackgroundMusic("CreepyMusic1");

        if (_animationId.HasValue)
        {
            LeanTween.cancel(_animationId.Value);
            _animationId = null;
        }

        if (AnimationTime == 0)
        {
            AnimationTime = 2;
        }

        Panel.color = GameHiddenOptions.Instance.BlackColor;
        AuthorName.alpha = 0f;

        StartCoroutine(PlayTestAnimationCo());
    }

    IEnumerator PlayTestAnimationCo()
    {
        yield return new WaitForSeconds(7);

        _textAnimationId = LeanTween.value(
            AuthorName.gameObject,
            0f,
            1f,
            5f
        ).id;
        LeanTween.descr(_textAnimationId.Value).setOnUpdate((float value) => {
            AuthorName.alpha = value;
        });
        LeanTween.descr(_textAnimationId.Value).setEase(LeanTweenType.linear);

        yield return new WaitForSeconds(2);

        _animationId = LeanTween.color(
            Panel.GetComponent<RectTransform>(),
            GameHiddenOptions.Instance.RedColor,
            AnimationTime
        ).id;
        LeanTween.descr(_animationId.Value).setEase(LeanTweenType.linear);
        LeanTween.descr(_animationId.Value).setOnComplete(OnTestAnimationEnd);
    }

    void OnTestAnimationEnd()
    {
        LeanTween.cancel(_animationId.Value);
        _animationId = null;

        MusicManager.Instance.PlayAmbient("TrainAmbient");

        _onFinish();
        // ActController.Instance.RequestContinue();
    }
}
