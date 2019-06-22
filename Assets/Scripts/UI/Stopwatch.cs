using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stopwatch : MonoBehaviour
{
    public float value = 0;

    public Text secondText;
    public Text miliSecondText;

    public bool paused = false;
    // Start is called before the first frame update
    void Start()
    {
        secondText.text = "";
        miliSecondText.text = "";
    }
    
    // Update is called once per frame
    void Update()
    {
        if (paused) { return; }

        value += Time.deltaTime;
        if (Setting.showStopwatch)
        {
            ShowText();
        }
    }
    public void ShowText()
    {
        secondText.text = value.ToString().Split(',')[0] + '.';
        miliSecondText.text = value.ToString().Split(',')[1].Substring(0, 3);

    }
}
