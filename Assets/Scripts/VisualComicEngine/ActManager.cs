using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActManager : MonoBehaviour
{
    private static ActManager _instance;
    public static ActManager Instance()
    {
        return _instance;
    }
    void Awake()
    {
        _instance = this;
    }
    public Dictionary<string, ActGameObject> Prefabs;
    public void Init()
    {
        Prefabs = GetActDict();
    }

    public ActGameObject AuthorAct;
    public ActGameObject HelloAct_1;
    public ActGameObject C_FromSpaceToGround;
    public ActGameObject cc_Inside_Car;

    // [HideInInspector]
    // public string[] ActDefs = new string[4] {
    //     "AuthorAct",
    //     "HelloAct_1",
    //     "C_FromSpaceToGround",
    //     "cc_Inside_Car"
    // };
    private Dictionary<string, ActGameObject> _actDefs;
    public Dictionary<string, ActGameObject> GetActDict()
    {
        if (_actDefs == null)
        {
            _actDefs = new Dictionary<string, ActGameObject>() {
                { "AuthorAct", AuthorAct },
                { "HelloAct_1", HelloAct_1},
                { "C_FromSpaceToGround", C_FromSpaceToGround },
                { "cc_Inside_Car", cc_Inside_Car }
            };
        }
        return _actDefs;
    }

    public IAct GetActObject(string actName)
    {
        GameObject go = null;
        if (Prefabs.ContainsKey(actName))
        {
            go = GameHiddenOptions.Instance.GetAnInstantiated(Prefabs[actName].Go);
        }

        go.name = actName;
        go.transform.SetParent(PrefabManager.Instance().PanelsParent, false);
        go.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);

        return go.GetComponent<IAct>();
    }
}
