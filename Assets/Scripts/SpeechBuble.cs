using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public delegate void SayLineFinished();

public class SpeechBuble : MonoBehaviour
{
    public TextMeshProUGUI DialogText;
    private Image _image;
    public bool IsWhite;
    public float Slowdown;
    public bool IsInUse;
    public bool IsConnector;
    public int Index;
    private SayLineFinished _onSayLineFinished;
    private int? _animationId;
    private IEnumerator _sayLineCo;

    public void InitLine(string text, int index, SayLineFinished onSayLineFinished)
    {
        Slowdown = 1.0f;
        DialogText.text = text;
        Index = index;
        _onSayLineFinished = onSayLineFinished;

        if (_image == null)
        {
            _image = GetComponent<Image>();
        }
    }

    public void SayLine()
    {
        if (_sayLineCo != null)
        {
            StopCoroutine(_sayLineCo);
            _sayLineCo = null;
        }
        _sayLineCo = SayLineCo();
        StartCoroutine(_sayLineCo);
    }

    private IEnumerator SayLineCo()
    {
        yield return new WaitForSeconds(GameHiddenOptions.Instance.TimeBeforeReadingLine);

        SayItService.Instance().WhiteColor = IsWhite;
        SayItService.Instance().SayLine(DialogText, () =>
        {
            IsInUse = false;
            _onSayLineFinished();
        }, Slowdown);
    }

    public void SetActive()
    {
        if (_image == null)
        {
            _image = GetComponent<Image>();
        }
        _image.color = GameHiddenOptions.Instance.WhiteTransparentColor;
        gameObject.SetActive(true);
        if (_animationId.HasValue)
        {
            LeanTween.cancel(_animationId.Value);
            _animationId = null;
        }
        _animationId = LeanTween.alpha(
            _image.GetComponent<RectTransform>(),
            1f,
            GameHiddenOptions.Instance.TimeToLoadDialogueBoxes
        ).id;
        LeanTween.descr(_animationId.Value).setEase(LeanTweenType.easeOutSine);
    }

    public void SetDisabled()
    {
        if (gameObject.activeSelf == false)
        {
            return;
        }
        if (_animationId.HasValue)
        {
            LeanTween.cancel(_animationId.Value);
            _animationId = null;
        }
        if (_image == null)
        {
            _image = GetComponent<Image>();
        }
        _animationId = LeanTween.alpha(
            _image.GetComponent<RectTransform>(),
            0f,
            GameHiddenOptions.Instance.TimeToLoadDialogueBoxes
        ).id;
        LeanTween.descr(_animationId.Value).setEase(LeanTweenType.easeInQuart);
        LeanTween.descr(_animationId.Value).setOnComplete(() =>
        {
            gameObject.SetActive(false);
        });
    }
}
