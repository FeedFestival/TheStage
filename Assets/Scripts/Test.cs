using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public SpeechBuble SpeechBuble;
    // Start is called before the first frame update
    void Start()
    {
        SpeechBuble.InitLine("Ana \nare \nmere.", 0, () => { });
        SpeechBuble.SayLine();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.A))
        {

        }
    }
}
