using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabManager : MonoBehaviour
{
    private static PrefabManager _instance;
    public static PrefabManager Instance()
    {
        return _instance;
    }

    public RectTransform PanelsParent;

    void Awake()
    {
        _instance = this;
    }

    public void Init() {
        foreach (Transform child in PanelsParent.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }
}
