using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Assets.Scripts.Utils;

public class ActController : MonoBehaviour
{
    private static ActController _instance;
    public static ActController Instance { get { return _instance; } }

    public List<IAct> Acts;

    IAct CurrentAct;

    private int ActIndex;
    private int ActsLength;
    public string StartAtAct;
    public Overlay Overlay;
    public bool RequestedContinueAction = false;
    public GameObject TapRequestGo;

    private TStyle _tStyle;

    void Awake()
    {
        _instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        MainStart();
    }

    /*
    MAIN Class
    ----------------------------------------------------
    */
    void MainStart()
    {
        Overlay.gameObject.SetActive(true);
        Overlay.SetTransition(OverlayTransition.Transparent);
        TapRequestGo.SetActive(false);

        PrefabManager.Instance().Init();
        StartCoroutine(InitDependency1());
    }

    private IEnumerator InitDependency1()
    {
        yield return new WaitForSeconds(0.1f);
        MusicManager.Instance.Init();
        DialogueController.Instance.Init();
        StartCoroutine(InitDependency2());
    }

    private IEnumerator InitDependency2()
    {
        yield return new WaitForSeconds(0.1f);
        ActManager.Instance().Init();
        StartStory();
    }

    void StartStory()
    {
        Acts = new List<IAct>();

        if (string.IsNullOrWhiteSpace(StartAtAct))
        {
            StartAtAct = ActManager.Instance().GetActDict().ElementAt(0).Key;
            ActIndex = 0;
        }
        else
        {
            if (UsefullUtils.IsDigitsOnly(StartAtAct))
            {
                ActIndex = System.Convert.ToInt32(StartAtAct);
                StartAtAct = ActManager.Instance().GetActDict().ElementAt(ActIndex).Key;
            }
            else
            {
                ActIndex = System.Array.IndexOf(ActManager.Instance().GetActDict().Keys.ToArray(), StartAtAct);
            }
        }
        Debug.Log(ActIndex);
        StartAct();
    }

    public void SetTransitionStyle(TStyle tStyle)
    {
        _tStyle = tStyle;
    }

    private void OnActEnd()
    {
        ActIndex++;

        if (ActIndex >= ActManager.Instance().GetActDict().Count)
        {
            return;
        }

        if (_tStyle == TStyle.None)
        {
            StartAct();
        }
        else
        {
            Overlay.Transition(OverlayTransition.Complete, 1f, () => { StartAct(); });
        }

        _tStyle = TStyle.None;
    }

    private void StartAct()
    {
        if (CurrentAct != null)
        {
            CurrentAct.GameObject.SetActive(false);
        }

        CurrentAct = ActManager.Instance().GetActObject(
            ActManager.Instance().GetActDict().ElementAt(ActIndex).Key
        );

        Acts.Add(CurrentAct);
        CurrentAct.InitAct(OnActEnd);
        ContinueStory();
    }

    public void ContinueStory()
    {
        CurrentAct.ContinueAct();
    }

    public void ContinueStoryAction()
    {
        CurrentAct.ContinueCurrentAction();
    }

    public void RequestContinue()
    {
        RequestedContinueAction = true;
        TapRequestGo.SetActive(true);
    }

    public void OnAdvanceTap()
    {
        if (RequestedContinueAction)
        {
            RequestedContinueAction = false;
            TapRequestGo.SetActive(false);
            ContinueStoryAction();
            return;
        }
        ContinueStory();
    }

    public void SetStartAtAct(string actName)
    {
        StartAtAct = actName;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            OnAdvanceTap();
        }
    }
}

public enum SpeedSettings
{
    ProductionSpeed,
    DevSpeed
};
