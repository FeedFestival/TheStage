using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ActGameObject//: MonoBehaviour
{
    [SerializeField]
    public GameObject Go;
    [SerializeField]
    public bool _editor_foldout = true;
    [SerializeField]
    public int _choiceIndex = 0;
}
